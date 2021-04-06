using System.Collections.Generic;
using FluentValidation;
using GraphQL.FluentValidation;

namespace GraphQL
{
    public static partial class FluentValidationExtensions
    {
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

        /// <summary>
        /// Injects a <see cref="ValidatorTypeCache" /> instance into a user context for testing purposes.
        /// </summary>
        public static void AddCacheToContext<T>(T userContext, ValidatorTypeCache cache)
            where T : Dictionary<string, object>
        {
            userContext.AddValidatorCache(cache);
        }
    }
}