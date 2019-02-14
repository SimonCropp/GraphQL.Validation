using FluentValidation;

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
        public static void UseFluentValidation(this ExecutionOptions executionOptions)
        {
            Guard.AgainstNull(executionOptions, nameof(executionOptions));

            executionOptions.FieldMiddleware.Use(next =>
            {
                return context => ValidationMiddleware.Resolve(context, next);
            });
        }
    }
}