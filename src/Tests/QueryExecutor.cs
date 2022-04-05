using GraphQL;
using GraphQL.FluentValidation;
using GraphQL.NewtonsoftJson;

static class QueryExecutor
{
    public static async Task<string> ExecuteQuery(string queryString, Inputs? inputs, IValidatorCache cache)
    {
        Thread.CurrentThread.CurrentUICulture = new("en-US");

        queryString = queryString.Replace("'", "\"");
        using var schema = new Schema();
        schema.UseFluentValidation();
        var documentExecuter = new DocumentExecuter();
        var executionOptions = new ExecutionOptions
        {
            Schema = schema,
            Query = queryString,
            Variables = inputs
        };
        executionOptions.UseFluentValidation(cache);

        var result = await documentExecuter.ExecuteAsync(executionOptions);
        var stream = new MemoryStream();
        await new GraphQLSerializer(indent: true).WriteAsync(stream, result);
        stream.Position = 0;
        var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}