using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using GraphQL.Types;

namespace GraphQL
{
    public static partial class FluentValidationExtensions
    {
        /// <summary>
        /// Wraps <see cref="ResolveFieldContext{TSource}.GetArgument{TType}"/> to validate the resulting argument instance.
        /// Uses <see cref="IValidator.ValidateAsync(ValidationContext,CancellationToken)"/> to perform validation.
        /// If a <see cref="ValidationException"/> it will be converted <see cref="ExecutionError"/>s by a field middleware.
        /// </summary>
        public static async Task<TArgument> GetValidatedArgumentAsync<TArgument>(this ResolveFieldContext context, string name, TArgument defaultValue = default)
        {
            Guard.AgainstNull(context, nameof(context));
            var argument = context.GetArgument(name, defaultValue);
            await ArgumentValidation.ValidateAsync(context.GetCache(), typeof(TArgument), argument, context.UserContext);
            return argument;
        }

        /// <summary>
        /// Wraps <see cref="ResolveFieldContext{TSource}.GetArgument"/> to validate the resulting argument instance.
        /// Uses <see cref="IValidator.ValidateAsync(ValidationContext,CancellationToken)"/> to perform validation.
        /// If a <see cref="ValidationException"/> it will be converted <see cref="ExecutionError"/>s by a field middleware.
        /// </summary>
        public static async Task<object> GetValidatedArgumentAsync(this ResolveFieldContext context, Type argumentType, string name, object? defaultValue = null)
        {
            Guard.AgainstNull(context, nameof(context));
            var argument = context.GetArgument(argumentType, name, defaultValue);
            await ArgumentValidation.ValidateAsync(context.GetCache(), argumentType, argument, context.UserContext);
            return argument;
        }

        /// <summary>
        /// Wraps <see cref="ResolveFieldContext{TSource}.GetArgument{TType}"/> to validate the resulting argument instance.
        /// Uses <see cref="IValidator.ValidateAsync(ValidationContext,CancellationToken)"/> to perform validation.
        /// If a <see cref="ValidationException"/> it will be converted <see cref="ExecutionError"/>s by a field middleware.
        /// </summary>
        public static async Task<TArgument> GetValidatedArgumentAsync<TSource, TArgument>(this ResolveFieldContext<TSource> context, string name, TArgument defaultValue = default)
        {
            Guard.AgainstNull(context, nameof(context));
            var argument = context.GetArgument(name, defaultValue);
            await ArgumentValidation.ValidateAsync(ArgumentTypeCacheBag.GetCache(context), typeof(TArgument), argument, context.UserContext);
            return argument;
        }

        /// <summary>
        /// Wraps <see cref="ResolveFieldContext{TSource}.GetArgument"/> to validate the resulting argument instance.
        /// Uses <see cref="IValidator.ValidateAsync(ValidationContext,CancellationToken)"/> to perform validation.
        /// If a <see cref="ValidationException"/> it will be converted <see cref="ExecutionError"/>s by a field middleware.
        /// </summary>
        public static async Task<object> GetValidatedArgumentAsync<TSource>(this ResolveFieldContext<TSource> context, Type argumentType, string name, object? defaultValue = null)
        {
            Guard.AgainstNull(context, nameof(context));
            var argument = context.GetArgument(argumentType, name, defaultValue);
            await ArgumentValidation.ValidateAsync(ArgumentTypeCacheBag.GetCache(context), argumentType, argument, context.UserContext);
            return argument;
        }

        /// <summary>
        /// Wraps <see cref="ResolveFieldContext{TSource}.GetArgument{TType}"/> to validate the resulting argument instance.
        /// Uses <see cref="IValidator.ValidateAsync(ValidationContext,CancellationToken)"/> to perform validation.
        /// If a <see cref="ValidationException"/> it will be converted <see cref="ExecutionError"/>s by a field middleware.
        /// </summary>
        public static async Task<TArgument> GetValidatedArgumentAsync<TArgument>(this ResolveFieldContext<object> context, string name, TArgument defaultValue = default)
        {
            Guard.AgainstNull(context, nameof(context));
            var argument = context.GetArgument(name, defaultValue);
            await ArgumentValidation.ValidateAsync(ArgumentTypeCacheBag.GetCache(context), typeof(TArgument), argument, context.UserContext);
            return argument;
        }

        /// <summary>
        /// Wraps <see cref="ResolveFieldContext{TSource}.GetArgument"/> to validate the resulting argument instance.
        /// Uses <see cref="IValidator.ValidateAsync(ValidationContext,CancellationToken)"/> to perform validation.
        /// If a <see cref="ValidationException"/> it will be converted <see cref="ExecutionError"/>s by a field middleware.
        /// </summary>
        public static async Task<object> GetValidatedArgumentAsync(this ResolveFieldContext<object> context, Type argumentType, string name, object? defaultValue = null)
        {
            Guard.AgainstNull(context, nameof(context));
            var argument = context.GetArgument(argumentType, name, defaultValue);
            await ArgumentValidation.ValidateAsync(ArgumentTypeCacheBag.GetCache(context), argumentType, argument, context.UserContext);
            return argument;
        }
    }
}