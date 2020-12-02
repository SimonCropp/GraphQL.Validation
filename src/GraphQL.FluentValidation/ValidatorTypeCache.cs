using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using FluentValidation;
using GraphQL.Utilities;

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
        bool isFrozen;

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
                typeCacheDI = new Dictionary<Type, List<Type>>();
            else
                typeCache = new Dictionary<Type, List<IValidator>>();
        }

        private bool UseDI => typeCacheDI != null;

        internal void Freeze()
        {
            isFrozen = true;
        }

        void ThrowIfFrozen()
        {
            if (isFrozen)
            {
                throw new InvalidOperationException($"{nameof(ValidatorTypeCache)} cannot be changed once it has been used. Use a new instance instead.");
            }
        }

        internal bool TryGetValidators(Type argumentType, IServiceProvider? provider, [NotNullWhen(true)] out IEnumerable<IValidator>? validators)
        {
            if (UseDI)
            {
                Guard.AgainstNull(provider, nameof(provider));

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

        /// <summary>
        /// Add all <see cref="IValidator"/>s from the assembly that contains <typeparamref name="T"/>.
        /// </summary>
        public ValidatorTypeCache AddValidatorsFromAssemblyContaining<T>(bool throwIfNoneFound = true)
        {
            return AddValidatorsFromAssemblyContaining(typeof(T), throwIfNoneFound);
        }

        /// <summary>
        /// Add all <see cref="IValidator"/>s from the assembly that contains <paramref name="type"/>.
        /// </summary>
        public ValidatorTypeCache AddValidatorsFromAssemblyContaining(Type type, bool throwIfNoneFound = true)
        {
            Guard.AgainstNull(type, nameof(type));
            return AddValidatorsFromAssembly(type.Assembly, throwIfNoneFound);
        }

        /// <summary>
        /// Add all <see cref="IValidator"/>s in <paramref name="assembly"/>.
        /// </summary>
        public ValidatorTypeCache AddValidatorsFromAssembly(Assembly assembly, bool throwIfNoneFound = true)
        {
            Guard.AgainstNull(assembly, nameof(assembly));
            ThrowIfFrozen();

            var results = AssemblyScanner.FindValidatorsInAssembly(assembly).ToList();
            if (!results.Any())
            {
                if (throwIfNoneFound)
                {
                    throw new Exception($"No validators were found in {assembly.GetName().Name}.");
                }
                return this;
            }

            foreach (var result in results)
            {
                var validatorType = result.ValidatorType;
                if (validatorType.IsAbstract)
                {
                    continue;
                }

                if (UseDI)
                {
                    var single = result.InterfaceType.GenericTypeArguments.Single();
                    if (!typeCacheDI!.TryGetValue(single, out var list))
                    {
                        typeCacheDI[single] = list = new List<Type>();
                    }

                    list.Add(validatorType);
                }
                else
                {
                    if (validatorType.GetConstructor(Array.Empty<Type>()) == null)
                    {
                        Trace.WriteLine($"Ignoring ''{validatorType.FullName}'' since it does not have a public parameterless constructor.");
                        continue;
                    }
                    var single = result.InterfaceType.GenericTypeArguments.Single();
                    if (!typeCache!.TryGetValue(single, out var list))
                    {
                        typeCache[single] = list = new();
                    }

                    list.Add((IValidator)Activator.CreateInstance(validatorType, true)!);
                }
            }

            return this;
        }
    }
}