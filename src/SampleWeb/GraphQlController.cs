using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using GraphQL.Transport;

[Route("[controller]")]
[ApiController]
public class GraphQLController :
    Controller
{
    ISchema schema;
    IDocumentExecuter executer;
    IGraphQLSerializer serializer;

    public GraphQLController(ISchema schema, IDocumentExecuter executer, IGraphQLSerializer serializer)
    {
        this.schema = schema;
        this.executer = executer;
        this.serializer = serializer;
    }

    [HttpPost]
    public async Task Post(
        [BindRequired, FromBody] GraphQLRequest request,
        CancellationToken cancellation)
    {
        var result = await Execute(request.Query, request.OperationName, request.Variables, cancellation);
        await serializer.WriteAsync(Response.Body, result, cancellation);
    }

    Task<ExecutionResult> Execute(
        string query,
        string? operationName,
        Inputs? variables,
        CancellationToken cancellation)
    {
        Thread.CurrentThread.CurrentUICulture = new("en-US");

        var options = new ExecutionOptions
        {
            Schema = schema,
            Query = query,
            OperationName = operationName,
            Variables = variables,
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