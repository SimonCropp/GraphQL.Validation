using System.Threading.Tasks;
using GraphQL;

static class QueryExecutor
{
    public static async Task<ExecutionResult> ExecuteQuery(string queryString, Inputs inputs)
    {
        queryString = queryString.Replace("'", "\"");
        using (var schema = new Schema())
        {
            var documentExecuter = new DocumentExecuter();

            var executionOptions = new ExecutionOptions
            {
                Schema = schema,
                Query = queryString,
                UserContext = new MyUserContext(),
                Inputs = inputs
            };
            executionOptions.UseFluentValidation();

            return await documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);
        }
    }
}