using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.FluentValidation
{
    /// <summary>
    /// Cache for all <see cref="IValidator"/>.
    /// Should only be configured once at startup time.
    /// </summary>
    public class ValidatorTypeCache
    {
        Dictionary<Type, List<IValidator>>? typeCache;
        Dictionary<Type, List<Type>>? typeCacheDI;

        /// <summary>
        /// Create cache with default DI behavior i.e. cache creates all validators itself.
        /// </summary>
        public ValidatorTypeCache() : this(false)
        {
        }

        /// <summary>
        /// Create cache with specified DI behavior.
        /// </summary>
        /// <param name="useDependencyInjection">
        /// <c>true</c> to use schema's ServiceProvider for resolving validators,
        /// <c>false</c> to use Activator.CreateInstance instead.
        /// </param>
        public ValidatorTypeCache(bool useDependencyInjection)
        {
            if (useDependencyInjection)
            {
                typeCacheDI = new();
            }
            else
            {
                typeCache = new();
            }
        }

        private bool UseDI => typeCacheDI != null;

        public bool IsFrozen { get; private set; }

        public void Freeze()
        {
            IsFrozen = true;
        }

        internal bool TryGetValidators(Type argumentType, IServiceProvider? provider, [NotNullWhen(true)] out IEnumerable<IValidator>? validators)
        {
            if (UseDI)
            {
                if (typeCacheDI!.TryGetValue(argumentType, out var validatorInfo))
                {
                    validators = validatorInfo.Select(t => (IValidator)provider!.GetRequiredService(t));
                    return true;
                }

                validators = null;
                return false;
            }
            else
            {
                if (typeCache!.TryGetValue(argumentType, out var validatorInfo))
                {
                    validators = validatorInfo;
                    return true;
                }

                validators = null;
                return false;
            }
        }

        public void AddResult(AssemblyScanner.AssemblyScanResult result)
        {
            if (UseDI)
            {
                var single = result.InterfaceType.GenericTypeArguments.Single();
                if (!typeCacheDI!.TryGetValue(single, out var list))
                {
                    typeCacheDI[single] = list = new();
                }

                list.Add(result.ValidatorType);
            }
            else
            {
                if (result.ValidatorType.GetConstructor(Array.Empty<Type>()) == null)
                {
                    Trace.WriteLine($"Ignoring ''{result.ValidatorType.FullName}'' since it does not have a public parameterless constructor.");
                    return;
                }

                var single = result.InterfaceType.GenericTypeArguments.Single();
                if (!typeCache!.TryGetValue(single, out var list))
                {
                    typeCache[single] = list = new();
                }

                list.Add((IValidator)Activator.CreateInstance(result.ValidatorType, true)!);
            }
        }
    }
}