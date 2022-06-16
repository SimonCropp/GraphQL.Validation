#nullable enable
using GraphQL.DI;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;

namespace GraphQL.FluentValidation.AspNetCore;

public static class FluentValidationGraphQLBuilderExtensions
{
    private sealed class ConfigureSchema : IConfigureSchema
    {
        private readonly Action<IValidatorCache>? _validatorCacheConfiguration;

        public ConfigureSchema(Action<IValidatorCache>? validatorCacheConfiguration) => _validatorCacheConfiguration = validatorCacheConfiguration;

        public void Configure(ISchema schema, IServiceProvider serviceProvider)
        {
            if (_validatorCacheConfiguration is { } validatorCacheConfiguration)
            {
                validatorCacheConfiguration(serviceProvider
                    .GetRequiredService<IValidatorCache>());
            }

            schema
                .UseFluentValidation();
        }
    }

    private sealed class FluentValidationConfigureExecutionOptions : IConfigureExecutionOptions
    {
        private readonly IValidatorCache? _validatorCache;

        public FluentValidationConfigureExecutionOptions(IValidatorCache validatorCache) => _validatorCache = validatorCache;

        public async Task ConfigureAsync(ExecutionOptions executionOptions)
        {
            if (_validatorCache is { } validatorCache)
            {
                executionOptions
                    .UseFluentValidation(_validatorCache);
            }
        }
    }

    public static IGraphQLBuilder AddFluentValidation(this IGraphQLBuilder builder, Action<IValidatorCache>? validatorCacheConfiguration = null)
    {
        if (builder is IServiceCollection services)
        {
            services
                .AddSingleton<IConfigureSchema>(new ConfigureSchema(validatorCacheConfiguration))
                .AddSingleton<IValidatorCache, ValidatorInstanceCache>()
                .AddSingleton<IConfigureExecutionOptions, FluentValidationConfigureExecutionOptions>();

            return builder;
        }

        throw new InvalidOperationException($"The provided {nameof(IGraphQLBuilder)} does not implement {nameof(IServiceCollection)}.");
    }
}
