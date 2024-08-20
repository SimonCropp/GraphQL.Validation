namespace GraphQL.FluentValidation;

/// <summary>
/// Cache for all <see cref="IValidator"/>.
/// Should only be configured once at startup time.
/// Uses <see cref="IServiceProvider"/> for resolving <see cref="IValidator"/>s.
/// </summary>
public class ValidatorServiceCache : IValidatorCache
{
    Dictionary<Type, List<Type>> cache = [];

    public bool IsFrozen { get; private set; }

    public void Freeze() =>
        IsFrozen = true;

    public bool TryGetValidators(Type argumentType, IServiceProvider? provider, [NotNullWhen(true)] out IEnumerable<IValidator>? validators)
    {
        if (cache.TryGetValue(argumentType, out var validatorInfo))
        {
            validators = validatorInfo.Select(_ => (IValidator)provider!.GetRequiredService(_));
            return true;
        }

        validators = null;
        return false;
    }

    public void AddResult(AssemblyScanner.AssemblyScanResult result)
    {
        var single = result.InterfaceType.GenericTypeArguments.Single();
        if (!cache.TryGetValue(single, out var list))
        {
            cache[single] = list = [];
        }

        list.Add(result.ValidatorType);
    }
}