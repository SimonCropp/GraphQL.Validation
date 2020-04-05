using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add all fluent validators types that implement <seealso cref="IValidator"/> in the calling assembly
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceLifetime">The service lifetime to register the fluent validators types with</param>
        /// <returns>Reference to <paramref name="services"/>.</returns>
        public static IServiceCollection AddFluentValidation(
            this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddFluentValidation(Assembly.GetCallingAssembly(), serviceLifetime);

        /// <summary>
        /// Add all fluent validators types that implement <seealso cref="IValidator"/> in the assembly which <paramref name="typeFromAssembly"/> belongs to
        /// </summary>
        /// <param name="services"></param>
        /// <param name="typeFromAssembly">The type from assembly to register all fluent validators types from</param>
        /// <param name="serviceLifetime">The service lifetime to register the fluent validators types with</param>
        /// <returns>Reference to <paramref name="services"/>.</returns>
        public static IServiceCollection AddFluentValidation(
            this IServiceCollection services,
            Type typeFromAssembly,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            => services.AddFluentValidation(typeFromAssembly.Assembly, serviceLifetime);

        /// <summary>
        /// Add all fluent validators types that implement <seealso cref="IValidator"/> in the specified assembly
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly">The assembly to register all fluent validators types from</param>
        /// <param name="serviceLifetime">The service lifetime to register the fluent validators types with</param>
        /// <returns>Reference to <paramref name="services"/>.</returns>
        public static IServiceCollection AddFluentValidation(
            this IServiceCollection services,
            Assembly assembly,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            var results = AssemblyScanner.FindValidatorsInAssembly(assembly).ToList();

            foreach (var result in results.Where(r => !r.ValidatorType.IsAbstract))
            {
                services.TryAdd(new ServiceDescriptor(result.ValidatorType, result.ValidatorType, serviceLifetime));
            }

            return services;
        }
    }
}
