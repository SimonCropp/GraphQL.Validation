using FluentValidation;
using GraphQL.DI;
using GraphQL.FluentValidation;
using GraphQL.Instrumentation;
using GraphQL.Types;

namespace GraphQL;

/// <summary>
/// Extensions to GraphQL to enable FluentValidation.
/// </summary>
public static partial class FluentValidationExtensions
{
    /// <summary>
    /// Adds a FieldMiddleware to the GraphQL pipeline that converts a <see cref="ValidationException"/> to <see cref="ExecutionError"/>s./>
    /// </summary>
    public static ExecutionOptions UseFluentValidation(this ExecutionOptions executionOptions, IValidatorCache validatorTypeCache)
    {
        validatorTypeCache.Freeze();
        executionOptions.SetCache(validatorTypeCache);
        return executionOptions;
    }

    /// <summary>
    /// Adds a FieldMiddleware to the GraphQL pipeline that converts a <see cref="ValidationException"/> to <see cref="ExecutionError"/>s./>
    /// </summary>
    public static void UseFluentValidation(this ISchema schema)
    {
        ValidationMiddleware validationMiddleware = new();
        schema.FieldMiddleware.Use(validationMiddleware);
    }

    /// <summary>
    /// Configures GraphQL to use FluentValidation, using a custom <see cref="IValidatorCache"/>.
    /// </summary>
    /// <param name="builder">
    /// The GraphQL builder.
    /// </param>
    /// <param name="validatorCache">
    /// The cache used to resolve validator types.
    /// </param>
    /// <returns>
    /// The <paramref name="builder"/> instance;
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> is null.
    /// -or-
    /// <paramref name="validatorCache"/> is null.
    /// </exception>
    public static IGraphQLBuilder UseFluentValidation(this IGraphQLBuilder builder, IValidatorCache validatorCache)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (validatorCache is null)
        {
            throw new ArgumentNullException(nameof(validatorCache));
        }

        builder.UseMiddleware<ValidationMiddleware>();

        validatorCache.Freeze();
        builder.ConfigureExecutionOptions(eo => eo.SetCache(validatorCache));

        return builder;
    }

    /// <summary>
    /// Configures GraphQL to use FluentValidation, with validators resolved using dependency injection.
    /// </summary>
    /// <param name="builder">
    /// The GraphQL builder.
    /// </param>
    /// <param name="assemblies">
    /// The list of assemblies containing validators.  All validators in these assemblies will be loaded.  For full
    /// control over which validators are loaded, you may build your own <see cref="IValidatorCache"/> and use the
    /// <see cref="FluentValidationExtensions.UseFluentValidation(IGraphQLBuilder, IValidatorCache)"/> overload.
    /// </param>
    /// <returns>
    /// The <paramref name="builder"/> instance;
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> is null.
    /// -or-
    /// <paramref name="assemblies"/> is null.
    /// </exception>
    public static IGraphQLBuilder UseFluentValidation(this IGraphQLBuilder builder, params Assembly[] assemblies) =>
        builder.UseFluentValidation((IEnumerable<Assembly>)assemblies);

    /// <summary>
    /// Configures GraphQL to use FluentValidation, with validators resolved using dependency injection.
    /// </summary>
    /// <param name="builder">
    /// The GraphQL builder.
    /// </param>
    /// <param name="assemblies">
    /// The list of assemblies containing validators.  All validators in these assemblies will be loaded.  For full
    /// control over which validators are loaded, you may build your own <see cref="IValidatorCache"/> and use the
    /// <see cref="FluentValidationExtensions.UseFluentValidation(IGraphQLBuilder, IValidatorCache)"/> overload.
    /// </param>
    /// <returns>
    /// The <paramref name="builder"/> instance;
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> is null.
    /// -or-
    /// <paramref name="assemblies"/> is null.
    /// </exception>
    public static IGraphQLBuilder UseFluentValidation(this IGraphQLBuilder builder, IEnumerable<Assembly> assemblies)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (assemblies is null)
        {
            throw new ArgumentNullException(nameof(assemblies));
        }

        var validatorCache = new ValidatorServiceCache();
        foreach (var assembly in assemblies)
        {
            validatorCache.AddValidatorsFromAssembly(assembly);
        }

        return builder.UseFluentValidation(validatorCache);
    }
}