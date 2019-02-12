using System;
using GraphQL.Types;

namespace GraphQL
{
    public static partial class FluentValidationExtensions
    {
        public static TArgument GetValidatedArgument<TArgument>(this ResolveFieldContext context, string name, TArgument defaultValue = default)
        {
            Guard.AgainstNull(context, nameof(context));
            var argument = context.GetArgument(name, defaultValue);
            ArgumentValidation.Validate(typeof(TArgument), argument, context.UserContext);
            return argument;
        }

        public static object GetValidatedArgument(this ResolveFieldContext context, Type argumentType, string name, object defaultValue = null)
        {
            Guard.AgainstNull(context, nameof(context));
            var argument = context.GetArgument(argumentType, name, defaultValue);
            ArgumentValidation.Validate(argumentType, argument, context.UserContext);
            return argument;
        }

        public static TArgument GetValidatedArgument<TSource, TArgument>(this ResolveFieldContext<TSource> context, string name, TArgument defaultValue = default)
        {
            Guard.AgainstNull(context, nameof(context));
            var argument = context.GetArgument(name, defaultValue);
            ArgumentValidation.Validate(typeof(TArgument), argument, context.UserContext);
            return argument;
        }

        public static object GetValidatedArgument<TSource>(this ResolveFieldContext<TSource> context, Type argumentType, string name, object defaultValue = null)
        {
            Guard.AgainstNull(context, nameof(context));
            var argument = context.GetArgument(argumentType, name, defaultValue);
            ArgumentValidation.Validate(argumentType, argument, context.UserContext);
            return argument;
        }

        public static TArgument GetValidatedArgument<TArgument>(this ResolveFieldContext<object> context, string name, TArgument defaultValue = default)
        {
            Guard.AgainstNull(context, nameof(context));
            var argument = context.GetArgument(name, defaultValue);
            ArgumentValidation.Validate(typeof(TArgument), argument, context.UserContext);
            return argument;
        }

        public static object GetValidatedArgument(this ResolveFieldContext<object> context, Type argumentType, string name, object defaultValue = null)
        {
            Guard.AgainstNull(context, nameof(context));
            var argument = context.GetArgument(argumentType, name, defaultValue);
            
            ArgumentValidation.Validate(argumentType, argument, context.UserContext);
            return argument;
        }
    }
}