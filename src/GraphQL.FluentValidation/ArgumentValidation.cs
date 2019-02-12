using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using GraphQL.FluentValidation;

static class ArgumentValidation
{
    public static async Task Validate(object instance, object userContext)
    {
        if (!ValidatorTypeCache.TryGetValidators(instance.GetType(), out var buildAll))
        {
            return;
        }

        var results = new List<ValidationFailure>();
        var validationContext = new ValidationContext(instance);
        validationContext.RootContextData.Add("UserContext", userContext);
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