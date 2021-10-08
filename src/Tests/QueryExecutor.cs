using GraphQL;
using GraphQL.FluentValidation;
using GraphQL.NewtonsoftJson;

static class QueryExecutor
{
    public static async Task<string> ExecuteQuery(string queryString, Inputs? inputs, IValidatorCache cache)
    {
        Thread.CurrentThread.CurrentUICulture = new("en-US");

        queryString = queryString.Replace("'", "\"");
        using Schema schema = new();
        schema.UseFluentValidation();
        DocumentExecuter documentExecuter = new();
        ExecutionOptions executionOptions = new()
        {
            Schema = schema,
            Query = queryString,
            Inputs = inputs
        };
        executionOptions.UseFluentValidation(cache);

        var result = await documentExecuter.ExecuteAsync(executionOptions);
        return await new DocumentWriter(indent: true).WriteToStringAsync(result);
    }
}