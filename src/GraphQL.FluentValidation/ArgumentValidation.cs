using FluentValidation.Results;

namespace GraphQL.FluentValidation;

/// <summary>
/// Low level validation API for extensibility scenarios.
/// </summary>
public static class ArgumentValidation
{
    /// <summary>
    /// Validate an instance
    /// </summary>
    public static Task ValidateAsync<TArgument>(IValidatorCache cache, Type type, TArgument instance, IDictionary<string, object?> userContext)
        => ValidateAsync(cache, type, instance, userContext, null);

    /// <summary>
    /// Validate an instance
    /// </summary>
    public static async Task ValidateAsync<TArgument>(IValidatorCache cache, Type type, TArgument instance, IDictionary<string, object?> userContext, IServiceProvider? provider, Cancel cancel = default)
    {
        var currentType = (Type?)type;
        var validationContext = default(ValidationContext<TArgument>);

        while (currentType != null)
        {
            if (cache.TryGetValidators(currentType, provider, out var buildAll))
            {
                validationContext ??= BuildValidationContext(instance, userContext);

                var tasks = buildAll.Select(_ => _.ValidateAsync(validationContext, cancel));
                var validationResults = await Task.WhenAll(tasks);

                var results = validationResults
                    .SelectMany(result => result.Errors);

                ThrowIfResults(results);
            }

            currentType = currentType.BaseType;
        }
    }

    /// <summary>
    /// Validate an instance
    /// </summary>
    public static void Validate<TArgument>(IValidatorCache cache, Type type, TArgument instance, IDictionary<string, object?> userContext)
        => Validate(cache, type, instance, userContext, null);

    /// <summary>
    /// Validate an instance
    /// </summary>
    public static void Validate<TArgument>(IValidatorCache cache, Type type, TArgument instance, IDictionary<string, object?> userContext, IServiceProvider? provider)
    {
        if (instance == null)
        {
            return;
        }

        var currentType = (Type?)type;
        var validationContext = default(ValidationContext<TArgument>);

        while (currentType != null)
        {
            if (cache.TryGetValidators(currentType, provider, out var buildAll))
            {
                validationContext ??= BuildValidationContext(instance, userContext);
                var results = buildAll
                    .SelectMany(validator => validator.Validate(validationContext).Errors);

                ThrowIfResults(results);
            }

            currentType = currentType.BaseType;
        }
    }

    static void ThrowIfResults(IEnumerable<ValidationFailure> results)
    {
        var list = results.ToList();
        if (list.Count != 0)
        {
            throw new ValidationException(list);
        }
    }

    static ValidationContext<TArgument> BuildValidationContext<TArgument>(TArgument instance, IDictionary<string, object?> userContext)
    {
        ValidationContext<TArgument> validationContext = new(instance);
        validationContext.RootContextData.Add("UserContext", userContext);
        return validationContext;
    }
}