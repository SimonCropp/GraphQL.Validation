using FluentValidation;
using FluentValidation.Results;
using GraphQL;
using GraphQL.Instrumentation;

class ValidationMiddleware : IFieldMiddleware
{
    static ExecutionError ToExecutionError(ValidationFailure failure) =>
        new($"{failure.PropertyName}: {failure.ErrorMessage}")
        {
            Path = new List<string> {failure.PropertyName},
            Code = failure.ErrorCode
        };

    public async ValueTask<object?> ResolveAsync(IResolveFieldContext context, FieldMiddlewareDelegate next)
    {
        try
        {
            return await next(context);
        }
        catch (ValidationException validationException)
        {
            context.Errors.AddRange(validationException.Errors.Select(ToExecutionError));

            return null;
        }
    }
}