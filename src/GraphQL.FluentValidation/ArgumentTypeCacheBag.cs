static class ArgumentTypeCacheBag
{
    const string key = "GraphQL.FluentValidation.ValidatorTypeCache";

    public static void SetCache(this ExecutionOptions options, IValidatorCache cache)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (options.UserContext == null)
        {
            options.UserContext = new Dictionary<string, object?>
            {
                { key, cache }
            };
            return;
        }

        AddValidatorCache(options.UserContext, cache);
    }

    internal static void AddValidatorCache(this IDictionary<string, object?> dictionary, IValidatorCache cache) =>
        dictionary.Add(key, cache);

    public static IValidatorCache GetCache(this IResolveFieldContext context)
    {
        if (context.UserContext == null)
        {
            throw NotDictionary();
        }

        if (context.UserContext.TryGetValue(key, out var result))
        {
            return (IValidatorCache)result!;
        }

        throw new($"Could not extract {nameof(IValidatorCache)} from {nameof(IResolveFieldContext)}.{nameof(IResolveFieldContext.UserContext)}. It is possible {nameof(FluentValidationExtensions)}.{nameof(FluentValidationExtensions.UseFluentValidation)} was not used.");
    }

    static Exception NotDictionary() =>
        new("Expected UserContext to be of type IDictionary<string, object>.");
}