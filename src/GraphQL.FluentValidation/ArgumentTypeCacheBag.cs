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
        var userContext = context.UserContext;
        if (userContext == null)
        {
            throw new("Expected UserContext to be of type IDictionary<string, object>.");
        }

        if (!userContext.TryGetValue(key, out var result))
        {
            throw new($"Could not extract {nameof(IValidatorCache)} from {nameof(IResolveFieldContext)}.{nameof(IResolveFieldContext.UserContext)}. It is possible {nameof(FluentValidationExtensions)}.{nameof(FluentValidationExtensions.UseFluentValidation)} was not used.");
        }

        return (IValidatorCache)result!;
    }
}