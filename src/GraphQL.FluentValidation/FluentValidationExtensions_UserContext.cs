using FluentValidation;
using FluentValidation.Validators;

namespace GraphQL
{
    public static partial class FluentValidationExtensions
    {
        public static T UserContext<T>(this CustomContext customContext)
        {
            Guard.AgainstNull(customContext, nameof(customContext));
            return customContext.ParentContext.UserContext<T>();
        }

        public static T UserContext<T>(this ValidationContext validationContext)
        {
            Guard.AgainstNull(validationContext, nameof(validationContext));
            return (T) validationContext.RootContextData["UserContext"];
        }
        
    }
}