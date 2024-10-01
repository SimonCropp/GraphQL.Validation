using System.Collections.Immutable;

namespace GraphQL.FluentValidation;

/// <summary>
/// Cache for all <see cref="IValidator"/> instances.
/// Should only be configured once at startup time.
/// Uses <see cref="Activator.CreateInstance(Type)"/> to create <see cref="IValidator"/>s.
/// </summary>
public class ValidatorInstanceCache(Func<Type, IValidator?>? fallback = null) :
    IValidatorCache
{
    ConcurrentDictionary<Type, List<IValidator>> cache = [];

    static TypeComparer typeComparer = new();

    public ImmutableSortedDictionary<Type, IReadOnlyList<IValidator>> GetCurrentValidators()
    {
        var dictionary = new SortedDictionary<Type, IReadOnlyList<IValidator>>(typeComparer);
        foreach (var item in cache)
        {
            dictionary.Add(item.Key, item.Value);
        }

        return dictionary.ToImmutableSortedDictionary(typeComparer);
    }

    public bool IsFrozen { get; private set; }

    public void Freeze() =>
        IsFrozen = true;

    public bool TryGetValidators(Type argumentType, IServiceProvider? provider, [NotNullWhen(true)] out IEnumerable<IValidator>? validators)
    {
        var list = cache.GetOrAdd(
            argumentType,
            type =>
            {
                var validator = fallback?.Invoke(type);
                if (validator == null)
                {
                    return [];
                }

                return [validator];
            });

        validators = list;
        return list.Count != 0;
    }

    public void AddResult(AssemblyScanner.AssemblyScanResult result)
    {
        if (result.ValidatorType.GetConstructor([]) == null)
        {
            Trace.WriteLine($"Ignoring ''{result.ValidatorType.FullName}'' since it does not have a public parameterless constructor.");
            return;
        }

        var single = result.InterfaceType.GenericTypeArguments.Single();
        if (!cache.TryGetValue(single, out var list))
        {
            cache[single] = list = [];
        }

        list.Add((IValidator)Activator.CreateInstance(result.ValidatorType, true)!);
    }

}