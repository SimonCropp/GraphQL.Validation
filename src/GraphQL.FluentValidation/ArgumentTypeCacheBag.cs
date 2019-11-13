using System;
using System.Collections.Generic;
using GraphQL;
using GraphQL.FluentValidation;
using GraphQL.Types;

static class ArgumentTypeCacheBag
{
    const string key = "GraphQL.FluentValidation.ValidatorTypeCache";

    public static ValidatorTypeCache GetCache(this ResolveFieldContext context)
    {
        return GetCache(context.UserContext);
    }

    public static void SetCache(this ExecutionOptions options, ValidatorTypeCache cache)
    {
        if (options.UserContext == null)
        {
            options.UserContext = new Dictionary<string, object>
            {
                {key, cache}
            };
            return;
        }

        var asDictionary = UserContextAsDictionary(options.UserContext);
        AddValidatorCache(asDictionary, cache);
    }

    internal static void AddValidatorCache(this IDictionary<string, object> dictionary, ValidatorTypeCache cache)
    {
        dictionary.Add(key, cache);
    }

    public static ValidatorTypeCache GetCache<T>(this ResolveFieldContext<T> context)
    {
        return GetCache(context.UserContext);
    }

    static ValidatorTypeCache GetCache(object userContext)
    {
        var dictionary = UserContextAsDictionary(userContext);

        if (dictionary.TryGetValue(key, out var result))
        {
            return (ValidatorTypeCache) result;
        }

        throw new Exception($"Could not extract {nameof(ValidatorTypeCache)} from ResolveFieldContext.UserContext. It is possible {nameof(FluentValidationExtensions)}.{nameof(FluentValidationExtensions.UseFluentValidation)} was not used.");
    }

    static IDictionary<string, object> UserContextAsDictionary(object userContext)
    {
        if (userContext is IDictionary<string, object> dictionary)
        {
            return dictionary;
        }

        throw NotDictionary();
    }

    static Exception NotDictionary()
    {
        return new Exception("Expected UserContext to be of type IDictionary<string, object>.");
    }
}