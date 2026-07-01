namespace GraphQL;

public static partial class FluentValidationExtensions
{
    /// <summary>
    /// When performing validation via GetValidatedArgument, the <see cref="IResolveFieldContext.RequestServices"/>
    /// instance is added to <see cref="IValidationContext.RootContextData"/> with a key of "ServiceProvider".
    /// During validation that instance can be retrieved from <see cref="IValidationContext"/> using this method.
    /// Returns false when validation is performed without an <see cref="IServiceProvider"/>.
    /// </summary>
    public static bool TryGetServiceProvider(this IValidationContext validationContext, [NotNullWhen(true)] out IServiceProvider? serviceProvider)
    {
        if (validationContext.RootContextData.TryGetValue("ServiceProvider", out var value) &&
            value is IServiceProvider provider)
        {
            serviceProvider = provider;
            return true;
        }

        serviceProvider = null;
        return false;
    }

    /// <summary>
    /// The <see cref="IServiceProvider"/> captured during validation (see <see cref="TryGetServiceProvider"/>).
    /// Throws if validation is performed without an <see cref="IServiceProvider"/>.
    /// </summary>
    public static IServiceProvider GetServiceProvider(this IValidationContext validationContext)
    {
        if (validationContext.TryGetServiceProvider(out var provider))
        {
            return provider;
        }

        throw new("No IServiceProvider is available in the validation context. Validate via IResolveFieldContext.GetValidatedArgument (which passes RequestServices), or pass an IServiceProvider to ArgumentValidation.Validate/ValidateAsync.");
    }

    /// <summary>
    /// Resolves a service of type <typeparamref name="T"/> from the <see cref="IServiceProvider"/> captured during
    /// validation. Throws if no service provider is available (see <see cref="GetServiceProvider"/>) or the service
    /// is not registered.
    /// </summary>
    public static T GetRequiredService<T>(this IValidationContext validationContext)
        where T : notnull =>
        validationContext.GetServiceProvider().GetRequiredService<T>();
}
