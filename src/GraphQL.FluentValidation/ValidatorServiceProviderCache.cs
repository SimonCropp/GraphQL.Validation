using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.FluentValidation;

/// <summary>
/// Uses the <see cref="IServiceProvider"/> to determine what validators are available and resolve validator instances.
/// </summary>
sealed class ValidatorServiceProviderCache : IValidatorCache
{
    /// <inheritdoc />
    public bool IsFrozen => true;

    /// <inheritdoc />
    public void Freeze()
    {
        // Intentionally empty.
    }

    /// <inheritdoc />
    public bool TryGetValidators(Type argumentType, IServiceProvider? provider, [NotNullWhen(true)] out IEnumerable<IValidator>? validators)
    {
        var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);
        try
        {
            validators = provider!.GetServices(validatorType).Cast<IValidator>();
            return true;
        }
        catch (InvalidOperationException)
        {
            validators = null;
            return false;
        }
    }

    /// <inheritdoc />
    public void AddResult(AssemblyScanner.AssemblyScanResult result) =>
        throw new InvalidOperationException("Method not supported.  The service provider cache uses IServiceProvider to determine what validators are available.");
}
