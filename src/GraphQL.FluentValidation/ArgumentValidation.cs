﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using GraphQL.FluentValidation;
#pragma warning disable 1591

namespace GraphQL
{
    public static class ArgumentValidation
    {
        public static async Task ValidateAsync(ValidatorTypeCache typeCache, Type type, object instance, object userContext)
        {
            Guard.AgainstNull(typeCache, nameof(typeCache));
            Guard.AgainstNull(userContext, nameof(userContext));
            Guard.AgainstNull(type, nameof(type));
            Guard.AgainstNull(userContext, nameof(userContext));
            if (!typeCache.TryGetValidators(type, out var buildAll))
            {
                return;
            }

            var validationContext = BuildValidationContext(instance, userContext);

            var tasks = buildAll.Select(x => x.ValidateAsync(validationContext));
            var validationResults = await Task.WhenAll(tasks);

            var results = validationResults
                .SelectMany(result => result.Errors)
                .ToList();

            ThrowIfResults(results);
        }

        public static void Validate(ValidatorTypeCache typeCache, Type type, object instance, object userContext)
        {
            Guard.AgainstNull(typeCache, nameof(typeCache));
            Guard.AgainstNull(userContext, nameof(userContext));
            Guard.AgainstNull(type, nameof(type));
            Guard.AgainstNull(userContext, nameof(userContext));
            if (!typeCache.TryGetValidators(type, out var buildAll))
            {
                return;
            }

            var validationContext = BuildValidationContext(instance, userContext);
            var results = buildAll
                .SelectMany(validator => validator.Validate(validationContext).Errors)
                .ToList();

            ThrowIfResults(results);
        }

        static void ThrowIfResults(List<ValidationFailure> results)
        {
            if (results.Any())
            {
                throw new ValidationException(results);
            }
        }

        static ValidationContext BuildValidationContext(object instance, object userContext)
        {
            var validationContext = new ValidationContext(instance);
            validationContext.RootContextData.Add("UserContext", userContext);
            return validationContext;
        }
    }
}