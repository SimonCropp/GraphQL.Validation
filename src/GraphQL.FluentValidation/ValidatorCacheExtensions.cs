using System;
using System.Linq;
using System.Reflection;
using FluentValidation;

namespace GraphQL.FluentValidation
{
    public static class ValidatorCacheExtensions
    {
        static void ThrowIfFrozen(this ValidatorTypeCache cache)
        {
            if (cache.IsFrozen)
            {
                throw new InvalidOperationException($"{nameof(ValidatorTypeCache)} cannot be changed once it has been used. Use a new instance instead.");
            }
        }

        /// <summary>
        /// Add all <see cref="IValidator"/>s from the assembly that contains <typeparamref name="T"/>.
        /// </summary>
        public static ValidatorTypeCache AddValidatorsFromAssemblyContaining<T>(this ValidatorTypeCache cache, bool throwIfNoneFound = true)
        {
            return AddValidatorsFromAssemblyContaining(cache, typeof(T), throwIfNoneFound);
        }

        /// <summary>
        /// Add all <see cref="IValidator"/>s from the assembly that contains <paramref name="type"/>.
        /// </summary>
        public static ValidatorTypeCache AddValidatorsFromAssemblyContaining(this ValidatorTypeCache cache, Type type, bool throwIfNoneFound = true)
        {
            return AddValidatorsFromAssembly(cache, type.Assembly, throwIfNoneFound);
        }

        /// <summary>
        /// Add all <see cref="IValidator"/>s in <paramref name="assembly"/>.
        /// </summary>
        public static ValidatorTypeCache AddValidatorsFromAssembly(this ValidatorTypeCache cache, Assembly assembly, bool throwIfNoneFound = true)
        {
            cache.ThrowIfFrozen();

            var results = AssemblyScanner.FindValidatorsInAssembly(assembly).ToList();
            if (!results.Any())
            {
                if (throwIfNoneFound)
                {
                    throw new($"No validators were found in {assembly.GetName().Name}.");
                }

                return cache;
            }

            foreach (var result in results)
            {
                var validatorType = result.ValidatorType;
                if (validatorType.IsAbstract)
                {
                    continue;
                }

                cache.AddResult(result);
            }

            return cache;
        }
    }
}