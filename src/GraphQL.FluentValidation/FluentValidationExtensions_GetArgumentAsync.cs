using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using GraphQL.FluentValidation;
using GraphQL.Types;

namespace GraphQL
{
    public static partial class FluentValidationExtensions
    {
        /// <summary>
        /// Wraps <see cref="IResolveFieldContext.GetArgument{TType}"/> to validate the resulting argument instance.
        /// Uses <see cref="IValidator.ValidateAsync(ValidationContext,CancellationToken)"/> to perform validation.
        /// If a <see cref="ValidationException"/> it will be converted <see cref="ExecutionError"/>s by a field middleware.
        /// </summary>
        public static async Task<TArgument> GetValidatedArgumentAsync<TArgument>(this IResolveFieldContext context, string name, TArgument defaultValue = default)
        {
            Guard.AgainstNull(context, nameof(context));
            var argument = context.GetArgument(name, defaultValue);
            var validatorCache = context.GetCache();
            await ArgumentValidation.ValidateAsync(validatorCache, typeof(TArgument), argument, context.UserContext);
            return argument;
        }

        /// <summary>
        /// Wraps <see cref="IResolveFieldContext.GetArgument"/> to validate the resulting argument instance.
        /// Uses <see cref="IValidator.ValidateAsync(ValidationContext,CancellationToken)"/> to perform validation.
        /// If a <see cref="ValidationException"/> it will be converted <see cref="ExecutionError"/>s by a field middleware.
        /// </summary>
        public static async Task<object> GetValidatedArgumentAsync(this IResolveFieldContext context, Type argumentType, string name, object? defaultValue = null)
        {
            Guard.AgainstNull(context, nameof(context));
            var argument = context.GetArgument(argumentType, name, defaultValue);
            var validatorCache = context.GetCache();
            await ArgumentValidation.ValidateAsync(validatorCache, argumentType, argument, context.UserContext);
            return argument;
        }
    }
}