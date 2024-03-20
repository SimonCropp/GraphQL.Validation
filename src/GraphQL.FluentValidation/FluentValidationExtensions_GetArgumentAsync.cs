namespace GraphQL;

public static partial class FluentValidationExtensions
{
    /// <summary>
    /// Wraps <see cref="ResolveFieldContextExtensions.GetArgument{TType}"/> to validate the resulting argument instance.
    /// Uses <see cref="IValidator.ValidateAsync(IValidationContext,Cancel)"/> to perform validation.
    /// If a <see cref="ValidationException"/> occurs it will be converted to <see cref="ExecutionError"/>s by a field middleware.
    /// </summary>
    public static Task<TArgument> GetValidatedArgumentAsync<TArgument>(this IResolveFieldContext context, string name) =>
        GetValidatedArgumentAsync<TArgument>(context, name, default!);

    /// <summary>
    /// Wraps <see cref="ResolveFieldContextExtensions.GetArgument{TType}"/> to validate the resulting argument instance.
    /// Uses <see cref="IValidator.ValidateAsync(IValidationContext,Cancel)"/> to perform validation.
    /// If a <see cref="ValidationException"/> occurs it will be converted to <see cref="ExecutionError"/>s by a field middleware.
    /// </summary>
    public static async Task<TArgument> GetValidatedArgumentAsync<TArgument>(this IResolveFieldContext context, string name, TArgument defaultValue)
    {
        var argument = context.GetArgument(name, defaultValue);
        var validatorCache = context.GetCache();
        await ArgumentValidation.ValidateAsync(validatorCache, typeof(TArgument), argument, context.UserContext, context.RequestServices);
        return argument!;
    }

    /// <summary>
    /// Wraps <see cref="ResolveFieldContextExtensions.GetArgument{TType}"/> to validate the resulting argument instance.
    /// Uses <see cref="IValidator.ValidateAsync(IValidationContext,Cancel)"/> to perform validation.
    /// If a <see cref="ValidationException"/> occurs it will be converted to <see cref="ExecutionError"/>s by a field middleware.
    /// </summary>
    public static Task<object> GetValidatedArgumentAsync(this IResolveFieldContext context, Type argumentType, string name) =>
        GetValidatedArgumentAsync(context, argumentType, name, default!);

    /// <summary>
    /// Wraps <see cref="ResolveFieldContextExtensions.GetArgument{TType}"/> to validate the resulting argument instance.
    /// Uses <see cref="IValidator.ValidateAsync(IValidationContext,Cancel)"/> to perform validation.
    /// If a <see cref="ValidationException"/> occurs it will be converted to <see cref="ExecutionError"/>s by a field middleware.
    /// </summary>
    public static async Task<object> GetValidatedArgumentAsync(this IResolveFieldContext context, Type argumentType, string name, object defaultValue)
    {
        var argument = context.GetArgument(argumentType, name, defaultValue);
        var validatorCache = context.GetCache();
        await ArgumentValidation.ValidateAsync(validatorCache, argumentType, argument, context.UserContext, context.RequestServices);
        return argument!;
    }
}