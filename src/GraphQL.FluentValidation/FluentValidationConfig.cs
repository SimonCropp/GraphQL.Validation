using System;
using System.Linq;
using System.Reflection;
using FluentValidation;
using NServiceBus.FluentValidation;

namespace NServiceBus
{
    public class FluentValidationConfig
    {
        EndpointConfiguration endpoint;
        DependencyLifecycle dependencyLifecycle;
        internal MessageValidator MessageValidator;

        internal FluentValidationConfig(EndpointConfiguration endpoint, ValidatorLifecycle validatorLifecycle)
        {
            this.endpoint = endpoint;

            if (validatorLifecycle == ValidatorLifecycle.Endpoint)
            {
                dependencyLifecycle = DependencyLifecycle.SingleInstance;
            }
            else
            {
                dependencyLifecycle = DependencyLifecycle.InstancePerCall;
            }
            var validatorTypeCache = GetValidatorTypeCache();
            MessageValidator = new MessageValidator(validatorTypeCache);
        }

        IValidatorTypeCache GetValidatorTypeCache()
        {
            if (dependencyLifecycle == DependencyLifecycle.SingleInstance)
            {
                return new EndpointValidatorTypeCache();
            }

            return new UnitOfWorkValidatorTypeCache();
        }

        public void AddValidatorsFromAssemblyContaining<T>(bool throwForNonPublicValidators = true, bool throwForNoValidatorsFound = true)
        {
            AddValidatorsFromAssemblyContaining(typeof(T), throwForNonPublicValidators, throwForNoValidatorsFound);
        }

        public void AddValidatorsFromAssemblyContaining(Type type, bool throwForNonPublicValidators = true, bool throwForNoValidatorsFound = true)
        {
            AddValidatorsFromAssembly(type.GetTypeInfo().Assembly, throwForNonPublicValidators, throwForNoValidatorsFound);
        }

        public void AddValidatorsFromAssembly(Assembly assembly, bool throwForNonPublicValidators = true, bool throwForNoValidatorsFound = true)
        {
            var assemblyName = assembly.GetName().Name;
            if (throwForNonPublicValidators)
            {
                var openGenericType = typeof(IValidator<>);
                var nonPublicValidators = assembly
                    .GetTypes()
                    .Where(type => !type.IsPublic &&
                                   !type.IsNestedPublic &&
                                   !type.IsAbstract &&
                                   !type.IsGenericTypeDefinition &&
                                   type.GetInterfaces()
                                       .Any(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == openGenericType)
                    )
                    .ToList();
                if (nonPublicValidators.Any())
                {
                    throw new Exception($"Found some non public validators were found in {assemblyName}:{Environment.NewLine}{string.Join(Environment.NewLine, nonPublicValidators.Select(x => x.FullName))}.");
                }
            }

            var results = AssemblyScanner.FindValidatorsInAssembly(assembly).ToList();
            if (throwForNoValidatorsFound && !results.Any())
            {
                throw new Exception($"No validators were found in {assemblyName}.");
            }

            endpoint.RegisterComponents(components =>
            {
                foreach (var result in results)
                {
                    components.ConfigureComponent(result.ValidatorType, dependencyLifecycle);
                }
            });
        }
    }
}