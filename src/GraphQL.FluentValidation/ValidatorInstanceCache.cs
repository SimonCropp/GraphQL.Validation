using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentValidation;

namespace GraphQL.FluentValidation
{
    /// <summary>
    /// Cache for all <see cref="IValidator"/> instances.
    /// Should only be configured once at startup time.
    /// Uses <see cref="Activator.CreateInstance(Type)"/> to create <see cref="IValidator"/>s.
    /// </summary>
    public class ValidatorInstanceCache : IValidatorCache
    {
        Dictionary<Type, List<IValidator>> cache = new();

        public bool IsFrozen { get; private set; }

        public void Freeze()
        {
            IsFrozen = true;
        }

        public bool TryGetValidators(Type argumentType, IServiceProvider? provider, [NotNullWhen(true)] out IEnumerable<IValidator>? validators)
        {
            if (cache.TryGetValue(argumentType, out var validatorInfo))
            {
                validators = validatorInfo;
                return true;
            }

            validators = null;
            return false;
        }

        public void AddResult(AssemblyScanner.AssemblyScanResult result)
        {
            if (result.ValidatorType.GetConstructor(Array.Empty<Type>()) == null)
            {
                Trace.WriteLine($"Ignoring ''{result.ValidatorType.FullName}'' since it does not have a public parameterless constructor.");
                return;
            }

            var single = result.InterfaceType.GenericTypeArguments.Single();
            if (!cache.TryGetValue(single, out var list))
            {
                cache[single] = list = new();
            }

            list.Add((IValidator)Activator.CreateInstance(result.ValidatorType, true)!);
        }
    }
}