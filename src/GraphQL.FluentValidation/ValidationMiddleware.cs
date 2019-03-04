using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using GraphQL;
using GraphQL.FluentValidation;
using GraphQL.Instrumentation;
using GraphQL.Types;

class ValidationMiddleware
{
    ValidatorTypeCache cache;

    public ValidationMiddleware(ValidatorTypeCache cache)
    {
        this.cache = cache;
    }

    public async Task<object> Resolve(ResolveFieldContext context, FieldMiddlewareDelegate next)
    {
        if (context.Arguments == null)
        {
            context.Arguments  = new Dictionary<string, object>();
        }
        context.SetCache(cache);
        try
        {
            return await next(context).ConfigureAwait(false);
        }
        catch (ValidationException validationException)
        {
            context.Errors.AddRange(validationException.Errors.Select(ToExecutionError));

            return ReturnTypeFinder.Find(context);
        }
        finally
        {
            context.RemoveCache();
        }
    }

    static ExecutionError ToExecutionError(ValidationFailure failure)
    {
        return new ExecutionError($"{failure.PropertyName}: {failure.ErrorMessage}");
    }
}