using System.Threading.Tasks;
using GraphQL;

static class QueryExecutor
{
    public static async Task<object> ExecuteQuery(string queryString, Inputs inputs)
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

            var executionResult = await documentExecuter.ExecuteWithErrorCheck(executionOptions).ConfigureAwait(false);
            return executionResult.Data;
        }
    }
}