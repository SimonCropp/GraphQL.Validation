namespace GraphQL
{
    /// <summary>
    /// Extensions to FluentValidation.
    /// </summary>
    public static partial class FluentValidationExtensions
    {
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