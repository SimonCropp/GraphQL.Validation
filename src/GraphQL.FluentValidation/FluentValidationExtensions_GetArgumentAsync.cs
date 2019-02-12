using System;
using System.Threading.Tasks;
using GraphQL.Types;

namespace GraphQL
{
    public static partial class FluentValidationExtensions
    {
        public static async Task<TArgument> GetValidatedArgumentAsync<TArgument>(this ResolveFieldContext context, string name, TArgument defaultValue = default)
        {
            Guard.AgainstNull(context, nameof(context));
            var argument = context.GetArgument(name, defaultValue);
            await ArgumentValidation.ValidateAsync(typeof(TArgument), argument, context.UserContext)
                .ConfigureAwait(false);
            return argument;
        }

        public static async Task<object> GetValidatedArgumentAsync(this ResolveFieldContext context, Type argumentType, string name, object defaultValue = null)
        {
            Guard.AgainstNull(context, nameof(context));
            var argument = context.GetArgument(argumentType, name, defaultValue);
            await ArgumentValidation.ValidateAsync(argumentType, argument, context.UserContext)
                .ConfigureAwait(false);
            return argument;
        }

        public static async Task<TArgument> GetValidatedArgumentAsync<TSource, TArgument>(this ResolveFieldContext<TSource> context, string name, TArgument defaultValue = default)
        {
            Guard.AgainstNull(context, nameof(context));
            var argument = context.GetArgument(name, defaultValue);
            await ArgumentValidation.ValidateAsync(typeof(TArgument), argument, context.UserContext)
                .ConfigureAwait(false);
            return argument;
        }

        public static async Task<object> GetValidatedArgumentAsync<TSource>(this ResolveFieldContext<TSource> context, Type argumentType, string name, object defaultValue = null)
        {
            Guard.AgainstNull(context, nameof(context));
            var argument = context.GetArgument(argumentType, name, defaultValue);
            await ArgumentValidation.ValidateAsync(argumentType, argument, context.UserContext)
                .ConfigureAwait(false);
            return argument;
        }

        public static async Task<TArgument> GetValidatedArgumentAsync<TArgument>(this ResolveFieldContext<object> context, string name, TArgument defaultValue = default)
        {
            Guard.AgainstNull(context, nameof(context));
            var argument = context.GetArgument(name, defaultValue);
            await ArgumentValidation.ValidateAsync(typeof(TArgument), argument, context.UserContext)
                .ConfigureAwait(false);
            return argument;
        }

        public static async Task<object> GetValidatedArgumentAsync(this ResolveFieldContext<object> context, Type argumentType, string name, object defaultValue = null)
        {
            Guard.AgainstNull(context, nameof(context));
            var argument = context.GetArgument(argumentType, name, defaultValue);
            await ArgumentValidation.ValidateAsync(argumentType, argument, context.UserContext)
                .ConfigureAwait(false);
            return argument;
        }
    }
}