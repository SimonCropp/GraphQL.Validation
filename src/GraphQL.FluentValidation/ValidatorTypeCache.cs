using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;

namespace GraphQL.FluentValidation
{
    /// <summary>
    /// Static cache for all <see cref="IValidator"/>.
    /// Should only be configured once at startup time.
    /// </summary>
    public static class ValidatorTypeCache
    {
        static Dictionary<Type, List<IValidator>> typeCache = new Dictionary<Type, List<IValidator>>();

        internal static bool TryGetValidators(Type argumentType, out IEnumerable<IValidator> validators)
        {
            if (typeCache.TryGetValue(argumentType, out var validatorInfo))
            {
                validators = validatorInfo;
                return true;
            }

            validators = Enumerable.Empty<IValidator>();
            return false;
        }

        /// <summary>
        /// Add all <see cref="IValidator"/>s in the assembly that contains <typeparamref name="T"/>.
        /// </summary>
        public static void AddValidatorsFromAssemblyContaining<T>()
        {
            AddValidatorsFromAssemblyContaining(typeof(T));
        }

        /// <summary>
        /// Add all <see cref="IValidator"/>s in the assembly that contains <paramref name="type"/>.
        /// </summary>
        public static void AddValidatorsFromAssemblyContaining(Type type)
        {
            Guard.AgainstNull(type, nameof(type));
            AddValidatorsFromAssembly(type.GetTypeInfo().Assembly);
        }

        /// <summary>
        /// Add all <see cref="IValidator"/>s in <paramref name="assembly"/>.
        /// </summary>
        public static void AddValidatorsFromAssembly(Assembly assembly)
        {
            Guard.AgainstNull(assembly, nameof(assembly));
            var assemblyName = assembly.GetName().Name;

            var results = AssemblyScanner.FindValidatorsInAssembly(assembly).ToList();
            if (!results.Any())
            {
                throw new Exception($"No validators were found in {assemblyName}.");
            }

            foreach (var result in results)
            {
                var validatorType = result.ValidatorType;
                var single = result.InterfaceType.GenericTypeArguments.Single();
                if (!typeCache.TryGetValue(single, out var list))
                {
                    typeCache[single] = list = new List<IValidator>();
                }

                list.Add((IValidator) Activator.CreateInstance(validatorType, true));
            }
        }
    }
}