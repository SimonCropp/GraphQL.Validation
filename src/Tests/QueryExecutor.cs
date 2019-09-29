using System.Threading.Tasks;
using GraphQL;
using GraphQL.FluentValidation;

static class QueryExecutor
{
    public static async Task<ExecutionResult> ExecuteQuery(string queryString, Inputs? inputs, ValidatorTypeCache cache)
    {
        queryString = queryString.Replace("'", "\"");
        using var schema = new Schema();
        var documentExecuter = new DocumentExecuter();

        var executionOptions = new ExecutionOptions
        {
            Schema = schema,
            Query = queryString,
            Inputs = inputs
        };
        executionOptions.UseFluentValidation(cache);

        return await documentExecuter.ExecuteAsync(executionOptions);
    }
}