using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using NServiceBus.ObjectBuilder;

class EndpointValidatorTypeCache : IValidatorTypeCache
{
    ConcurrentDictionary<Type, ValidatorInfo> typeCache = new ConcurrentDictionary<Type, ValidatorInfo>();

    static Type validatorType = typeof(IValidator<>);

    public bool TryGetValidators(Type messageType, IBuilder builder, out IEnumerable<IValidator> validators)
    {
        var validatorInfo = typeCache.GetOrAdd(messageType,
            type =>
            {
                var makeGenericType = validatorType.MakeGenericType(type);
                var all = builder.BuildAll(makeGenericType)
                    .Cast<IValidator>()
                    .ToList();
                return new ValidatorInfo
                {
                    Validators = all,
                    HasValidators = all.Any()
                };
            });


        validators = validatorInfo.Validators;
        return validatorInfo.HasValidators;
    }

    class ValidatorInfo
    {
        public bool HasValidators;
        public List<IValidator> Validators;
    }
}