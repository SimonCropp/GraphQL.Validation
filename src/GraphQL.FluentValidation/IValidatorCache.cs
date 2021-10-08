using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace GraphQL.FluentValidation;

public interface IValidatorCache
{
    bool IsFrozen { get; }
    void Freeze();
    bool TryGetValidators(Type argumentType, IServiceProvider? provider, [NotNullWhen(true)] out IEnumerable<IValidator>? validators);
    void AddResult(AssemblyScanner.AssemblyScanResult result);
}