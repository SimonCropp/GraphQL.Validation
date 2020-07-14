using FluentValidation;
using FluentValidation.Validators;

namespace GraphQL
{
    public static partial class FluentValidationExtensions
    {
        /// <summary>
        /// When performing validation the <see cref="ExecutionOptions.UserContext"/> instance
        /// will be added to <see cref="IValidationContext.RootContextData"/> with an key of "UserContext".
        /// During validation this instance can be retrieved from <see cref="CustomContext"/> using this method.
        /// </summary>
        public static T UserContext<T>(this CustomContext customContext)
        {
            Guard.AgainstNull(customContext, nameof(customContext));
            return customContext.ParentContext.UserContext<T>();
        }

        /// <summary>
        /// When performing validation the <see cref="ExecutionOptions.UserContext"/> instance
        /// will be added to <see cref="IValidationContext.RootContextData"/> with an key of "UserContext".
        /// During validation this instance can be retrieved from <see cref="IValidationContext"/> using this method.
        /// </summary>
        public static T UserContext<T>(this IValidationContext validationContext)
        {
            Guard.AgainstNull(validationContext, nameof(validationContext));
            return (T)validationContext.RootContextData["UserContext"];
        }
    }
}