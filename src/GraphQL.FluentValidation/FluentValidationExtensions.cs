using FluentValidation;
using GraphQL.FluentValidation;
using GraphQL.Types;

namespace GraphQL
{
    /// <summary>
    /// Extensions to GraphQL to enable FluentValidation.
    /// </summary>
    public static partial class FluentValidationExtensions
    {
        /// <summary>
        /// Validate an instance against the current cached validators defined by <see cref="ValidatorTypeCache"/>.
        /// </summary>
        public static void ValidateInstance<TSource, TInstance>(this ResolveFieldContext<TSource> context, TInstance input)
        {
            Guard.AgainstNull(context, nameof(context));
            Guard.AgainstNull(input, nameof(input));
            var type = input.GetType();
            ArgumentValidation.Validate(ArgumentTypeCacheBag.GetCache(context), type, input, context.UserContext);
        }

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
            executionOptions.FieldMiddleware.Use(next => { return context => validationMiddleware.Resolve(context, next); });
        }
    }
}