using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using GraphQL;
using GraphQL.Instrumentation;
using GraphQL.Types;

static class ValidationMiddleware
{
    public static async Task<object> Resolve(ResolveFieldContext context, FieldMiddlewareDelegate next)
    {
        try
        {
            return await next(context).ConfigureAwait(false);
        }
        catch (ValidationException validationException)
        {
            context.Errors.AddRange(validationException.Errors.Select(ToExecutionError));

            return ReturnTypeFinder.Find(context);
        }
    }

    static ExecutionError ToExecutionError(ValidationFailure failure)
    {
        return new ExecutionError($"{failure.PropertyName}: {failure.ErrorMessage}");
    }
}