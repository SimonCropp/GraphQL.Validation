using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.NewtonsoftJson;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;

[Route("[controller]")]
[ApiController]
public class GraphQLController :
    Controller
{
    ISchema schema;
    IDocumentExecuter executer;
    IDocumentWriter writer;

    public GraphQLController(ISchema schema, IDocumentExecuter executer, IDocumentWriter writer)
    {
        this.schema = schema;
        this.executer = executer;
        this.writer = writer;
    }

    [HttpPost]
    public async Task<string> Post(
        [BindRequired, FromBody] PostBody body,
        CancellationToken cancellation)
    {
        var result = await Execute(body.Query, body.OperationName, body.Variables, cancellation);
        return await writer.WriteToStringAsync(result);
    }

    public class PostBody
    {
        public string? OperationName;
        public string Query = null!;
        public JObject? Variables;
    }

    Task<ExecutionResult> Execute(string query,
        string? operationName,
        JObject? variables,
        CancellationToken cancellation)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

        var options = new ExecutionOptions
        {
            Schema = schema,
            Query = query,
            OperationName = operationName,
            Inputs = variables?.ToInputs(),
            CancellationToken = cancellation,
#if (DEBUG)
            ThrowOnUnhandledException = true,
            EnableMetrics = true,
#endif
        };
        options.UseFluentValidation(ValidatorCacheBuilder.InstanceDI);

        return executer.ExecuteAsync(options);
    }
}