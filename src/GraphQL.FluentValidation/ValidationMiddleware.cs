using System.Diagnostics;
using System.Threading.Tasks;
using GraphQL.Instrumentation;
using GraphQL.Types;

static class ValidationMiddleware
{
    public static async Task<object> Resolve(ResolveFieldContext context, FieldMiddlewareDelegate next)
    {
        if (context.Arguments != null)
        {
            foreach (var argument in context.Arguments)
            {
                Debug.WriteLine(argument);
            }
        }

        return await next(context).ConfigureAwait(false);
    }
}