using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using NServiceBus.ObjectBuilder;

class UnitOfWorkValidatorTypeCache : IValidatorTypeCache
{
    ConcurrentDictionary<Type, ValidatorInfo> typeCache = new ConcurrentDictionary<Type, ValidatorInfo>();

    static Type validatorType = typeof(IValidator<>);

    public bool TryGetValidators(Type messageType, IBuilder builder, out IEnumerable<IValidator> validators)
    {
        var validatorInfo = typeCache.GetOrAdd(messageType,
            type => new ValidatorInfo
            {
                ValidatorType = validatorType.MakeGenericType(type)
            });

        if (validatorInfo.HasValidators.HasValue)
        {
            if (!validatorInfo.HasValidators.Value)
            {
                validators = Enumerable.Empty<IValidator>();
                return false;
            }
        }

        validators = builder
            .BuildAll(validatorInfo.ValidatorType)
            .Cast<IValidator>()
            .ToList();

        var any = validators.Any();
        validatorInfo.HasValidators = any;
        return any;
    }

    class ValidatorInfo
    {
        public Type ValidatorType;
        public bool? HasValidators;
    }
}