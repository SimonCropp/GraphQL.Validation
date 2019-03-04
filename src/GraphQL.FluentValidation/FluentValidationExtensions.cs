using FluentValidation;
using GraphQL.FluentValidation;

namespace GraphQL
{
    /// <summary>
    /// Extensions to FluentValidation.
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

            var validationMiddleware = new ValidationMiddleware(validatorTypeCache);
            validatorTypeCache.Freeze();
            executionOptions.FieldMiddleware.Use(next => { return context => validationMiddleware.Resolve(context, next); });
        }
    }
}