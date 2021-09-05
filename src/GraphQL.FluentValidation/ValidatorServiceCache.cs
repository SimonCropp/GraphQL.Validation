using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.FluentValidation
{
    /// <summary>
    /// Cache for all <see cref="IValidator"/>.
    /// Should only be configured once at startup time.
    /// Uses <see cref="IServiceProvider"/> for resolving <see cref="IValidator"/>s.
    /// </summary>
    public class ValidatorServiceCache : IValidatorCache
    {
        Dictionary<Type, List<Type>>? cache = new();

        public bool IsFrozen { get; private set; }

        public void Freeze()
        {
            IsFrozen = true;
        }

        public bool TryGetValidators(Type argumentType, IServiceProvider? provider, [NotNullWhen(true)] out IEnumerable<IValidator>? validators)
        {
            if (cache!.TryGetValue(argumentType, out var validatorInfo))
            {
                validators = validatorInfo.Select(t => (IValidator)provider!.GetRequiredService(t));
                return true;
            }

            validators = null;
            return false;
        }

        public void AddResult(AssemblyScanner.AssemblyScanResult result)
        {
            var single = result.InterfaceType.GenericTypeArguments.Single();
            if (!cache!.TryGetValue(single, out var list))
            {
                cache[single] = list = new();
            }

            list.Add(result.ValidatorType);
        }
    }
}