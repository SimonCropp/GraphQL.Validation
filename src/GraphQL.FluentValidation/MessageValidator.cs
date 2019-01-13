#pragma warning disable AsyncFixer02
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using NServiceBus.Extensibility;
using NServiceBus.ObjectBuilder;

class MessageValidator
{
    IValidatorTypeCache validatorTypeCache;

    public MessageValidator(IValidatorTypeCache validatorTypeCache)
    {
        this.validatorTypeCache = validatorTypeCache;
    }

    public async Task Validate(Type messageType, IBuilder contextBuilder, object instance, Dictionary<string, string> headers, ContextBag contextBag)
    {
        if (!validatorTypeCache.TryGetValidators(messageType, contextBuilder, out var buildAll))
        {
            return;
        }

        var results = new List<ValidationFailure>();
        var validationContext = new ValidationContext(instance);
        validationContext.RootContextData.Add("Headers", headers);
        validationContext.RootContextData.Add("ContextBag", contextBag);
        foreach (var validator in buildAll)
        {
            if (AsyncValidatorChecker.IsAsync(validator, validationContext))
            {
                var result = await validator.ValidateAsync(validationContext)
                    .ConfigureAwait(false);
                results.AddRange(result.Errors);
            }
            else
            {
                var result = validator.Validate(validationContext);
                results.AddRange(result.Errors);
            }
        }

        if (results.Any())
        {
            throw new ValidationException(results);
        }
    }
}