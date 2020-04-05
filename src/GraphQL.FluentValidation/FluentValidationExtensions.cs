using FluentValidation;
using GraphQL.FluentValidation;
using GraphQL.Instrumentation;

namespace GraphQL
{
    /// <summary>
    /// Extensions to GraphQL to enable FluentValidation.
    /// </summary>
    public static partial class FluentValidationExtensions
    {
        /// <summary>
        /// Adds a FieldMiddleware to the GraphQL pipeline that converts a <see cref="ValidationException"/> to <see cref="ExecutionError"/>s./>
        /// </summary>
        public static void UseFluentValidation(this ExecutionOptions executionOptions, ValidatorTypeCache validatorTypeCache)
        {
            Guard.AgainstNull(executionOptions, nameof(executionOptions));
            Guard.AgainstNull(validatorTypeCache, nameof(validatorTypeCache));

            validatorTypeCache.Freeze();
            executionOptions.SetCache(validatorTypeCache);
            var validationMiddleware = new ValidationMiddleware();
            executionOptions.FieldMiddleware.Use(validationMiddleware);
        }
    }
}