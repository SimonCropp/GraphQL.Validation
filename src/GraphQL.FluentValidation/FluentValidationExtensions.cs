using FluentValidation;
using GraphQL.FluentValidation;
using GraphQL.Instrumentation;
using GraphQL.Types;

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
        public static ExecutionOptions UseFluentValidation(this ExecutionOptions executionOptions, ValidatorTypeCache validatorTypeCache)
        {
            Guard.AgainstNull(executionOptions, nameof(executionOptions));
            Guard.AgainstNull(validatorTypeCache, nameof(validatorTypeCache));

            validatorTypeCache.Freeze();
            executionOptions.SetCache(validatorTypeCache);
            return executionOptions;
        }

        /// <summary>
        /// Adds a FieldMiddleware to the GraphQL pipeline that converts a <see cref="ValidationException"/> to <see cref="ExecutionError"/>s./>
        /// </summary>
        public static void UseFluentValidation(this Schema schema)
        {
            Guard.AgainstNull(schema, nameof(schema));

            ValidationMiddleware validationMiddleware = new();
            schema.FieldMiddleware.Use(validationMiddleware);
        }
    }
}