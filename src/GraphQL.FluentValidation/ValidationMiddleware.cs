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
        ExecutionError executionError = new ExecutionError($"{failure.PropertyName}: {failure.ErrorMessage}");

        List<string> fields = new List<string>();
        fields.Add(failure.PropertyName);
        executionError.Path = fields;

        return executionError;
    }
}