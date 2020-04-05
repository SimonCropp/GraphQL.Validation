using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        public static async Task ValidateAsync(ValidatorTypeCache cache, Type type, object? instance, IDictionary<string, object> userContext, CancellationToken cancellation = default)
        {
            Guard.AgainstNull(cache, nameof(cache));
            Guard.AgainstNull(userContext, nameof(userContext));
            Guard.AgainstNull(type, nameof(type));
            Guard.AgainstNull(userContext, nameof(userContext));
            if (!cache.TryGetValidators(type, out var buildAll))
            {
                return;
            }

            var validationContext = BuildValidationContext(instance, userContext);

            var tasks = buildAll.Select(x => x.ValidateAsync(validationContext, cancellation));
            var validationResults = await Task.WhenAll(tasks);

            var results = validationResults
                .SelectMany(result => result.Errors);

            ThrowIfResults(results);
        }

        /// <summary>
        /// Validate an instance
        /// </summary>
        public static void Validate(ValidatorTypeCache cache, Type type, object? instance, IDictionary<string, object> userContext)
        {
            Guard.AgainstNull(cache, nameof(cache));
            Guard.AgainstNull(userContext, nameof(userContext));
            Guard.AgainstNull(type, nameof(type));
            Guard.AgainstNull(userContext, nameof(userContext));
            if (!cache.TryGetValidators(type, out var buildAll))
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

        static ValidationContext BuildValidationContext(object? instance, IDictionary<string, object> userContext)
        {
            var validationContext = new ValidationContext(instance);
            validationContext.RootContextData.Add("UserContext", userContext);
            return validationContext;
        }
    }
}