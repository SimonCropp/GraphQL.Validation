using System;
using System.Collections.Generic;
using GraphQL;
using GraphQL.FluentValidation;

static class ArgumentTypeCacheBag
{
    const string key = "GraphQL.FluentValidation.ValidatorTypeCache";

    public static void SetCache(this ExecutionOptions options, ValidatorTypeCache cache)
    {
        if (options.UserContext == null)
        {
            options.UserContext = new Dictionary<string, object>
            {
                { key, cache }
            };
            return;
        }

        AddValidatorCache(options.UserContext, cache);
    }

    internal static void AddValidatorCache(this IDictionary<string, object> dictionary, ValidatorTypeCache cache)
    {
        dictionary.Add(key, cache);
    }

    public static ValidatorTypeCache GetCache(this IResolveFieldContext context)
    {
        if (context.UserContext == null)
        {
            throw NotDictionary();
        }

        if (context.UserContext.TryGetValue(key, out var result))
        {
            return (ValidatorTypeCache)result;
        }

        throw new Exception($"Could not extract {nameof(ValidatorTypeCache)} from {nameof(IResolveFieldContext)}.{nameof(IResolveFieldContext.UserContext)}. It is possible {nameof(FluentValidationExtensions)}.{nameof(FluentValidationExtensions.UseFluentValidation)} was not used.");
    }

    static Exception NotDictionary()
    {
        return new Exception("Expected UserContext to be of type IDictionary<string, object>.");
    }
}