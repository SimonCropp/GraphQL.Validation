using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace GraphQL.FluentValidation
{
    /// <summary>
    /// Low level validation API for extensibility scenarios.
    /// </summary>
    public static class ArgumentValidation
    {
        /// <summary>
        /// Validate an instance
        /// </summary>
        public static Task ValidateAsync(ValidatorTypeCache cache, Type type, object? instance, object userContext)
            => ValidateAsync(cache, type, instance, userContext, null);

        /// <summary>
        /// Validate an instance
        /// </summary>
        public static async Task ValidateAsync(ValidatorTypeCache cache, Type type, object? instance, object userContext, IServiceProvider? provider)
        {
            Guard.AgainstNull(cache, nameof(cache));
            Guard.AgainstNull(userContext, nameof(userContext));
            Guard.AgainstNull(type, nameof(type));
            Guard.AgainstNull(userContext, nameof(userContext));
            if (!cache.TryGetValidators(type, provider, out var buildAll))
            {
                return;
            }

            var validationContext = BuildValidationContext(instance, userContext);

            var tasks = buildAll.Select(x => x.ValidateAsync(validationContext));
            var validationResults = await Task.WhenAll(tasks);

            var results = validationResults
                .SelectMany(result => result.Errors);

            ThrowIfResults(results);
        }

        /// <summary>
        /// Validate an instance
        /// </summary>
        public static void Validate(ValidatorTypeCache cache, Type type, object? instance, object userContext)
            => Validate(cache, type, instance, userContext, null);

        /// <summary>
        /// Validate an instance
        /// </summary>
        public static void Validate(ValidatorTypeCache cache, Type type, object? instance, object userContext, IServiceProvider? provider)
        {
            Guard.AgainstNull(cache, nameof(cache));
            Guard.AgainstNull(userContext, nameof(userContext));
            Guard.AgainstNull(type, nameof(type));
            Guard.AgainstNull(userContext, nameof(userContext));
            if (!cache.TryGetValidators(type, provider, out var buildAll))
            {
                return;
            }

            var validationContext = BuildValidationContext(instance, userContext);
            var results = buildAll
                .SelectMany(validator => validator.Validate(validationContext).Errors);

            ThrowIfResults(results);
        }

        static void ThrowIfResults(IEnumerable<ValidationFailure> results)
        {
            var list = results.ToList();
            if (list.Any())
            {
                throw new ValidationException(list);
            }
        }

        static ValidationContext BuildValidationContext(object? instance, object userContext)
        {
            var validationContext = new ValidationContext(instance);
            validationContext.RootContextData.Add("UserContext", userContext);
            return validationContext;
        }
    }
}