using System.Threading.Tasks;
using FluentValidation;
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
            foreach (var error in validationException.Errors)
            {
                context.Errors.Add(new ExecutionError(error.ToString()));
            }
            
            return ReturnTypeFinder.Find(context);
        }
    }
}