using System;
using System.Collections.Generic;
using GraphQL;
using GraphQL.FluentValidation;
using GraphQL.Types;

static class ArgumentTypeCacheBag
{
    public static ValidatorTypeCache GetCache(this ResolveFieldContext context)
    {
        return GetCache(context.Arguments);
    }

    public static void SetCache(this ResolveFieldContext context, ValidatorTypeCache cache)
    {
        context.Arguments.Add("GraphQL.FluentValidation.ValidatorTypeCache", cache);
    }

    public static void RemoveCache(this ResolveFieldContext context)
    {
        context.Arguments.Remove("GraphQL.FluentValidation.ValidatorTypeCache");
    }

    public static ValidatorTypeCache GetCache<T>(ResolveFieldContext<T> context)
    {
        return GetCache(context.Arguments);
    }

    static ValidatorTypeCache GetCache(Dictionary<string, object> arguments)
    {
        if (arguments.TryGetValue("GraphQL.FluentValidation.ValidatorTypeCache", out var result))
        {
            return (ValidatorTypeCache) result;
        }

        throw new Exception($"Could not extract {nameof(ValidatorTypeCache)} from ResolveFieldContext.Arguments. It is possible {nameof(FluentValidationExtensions)}.{nameof(FluentValidationExtensions.UseFluentValidation)} has not been called on {nameof(ExecutionOptions)}.");
    }
}