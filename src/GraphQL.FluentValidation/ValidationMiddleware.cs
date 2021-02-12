using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using GraphQL;
using GraphQL.Instrumentation;

class ValidationMiddleware : IFieldMiddleware
{
    public async Task<object?> Resolve(IResolveFieldContext context, FieldMiddlewareDelegate next)
    {
        try
        {
            return await next(context);
        }
        catch (ValidationException validationException)
        {
            context.Errors.AddRange(validationException.Errors.Select(ToExecutionError));

            return ReturnTypeFinder.Find(context);
        }
    }

    static ExecutionError ToExecutionError(ValidationFailure failure)
    {
        return new($"{failure.PropertyName}: {failure.ErrorMessage}")
        {
            Path = new List<string> {failure.PropertyName},
            Code = failure.ErrorCode
        };
    }
}