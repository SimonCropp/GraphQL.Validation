namespace GraphQL
{
    /// <summary>
    /// Extensions to control message validation with FluentValidation.
    /// </summary>
    public static class FluentValidationConfigurationExtensions
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


//class ValidationRule : GraphQL.Validation.IValidationRule
//{
//    public INodeVisitor Validate(ValidationContext context)
//    {
//        return new EnterLeaveListener(delegate(EnterLeaveListener _)
//        {
//            _.Match(delegate(Argument argument)
//            {
//                var value = argument.Value.Value;
//                ArgumentValidator.Validate()
//                if (ValidatorTypeCache.TryGetValidators(value.GetType(), out var validators))
//                {

//                }
//            });
//        });
//    }
//}