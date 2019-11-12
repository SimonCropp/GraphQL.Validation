﻿using System;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;

[Route("[controller]")]
[ApiController]
public class GraphQlController :
    Controller
{
    IDocumentExecuter executer;
    ISchema schema;

    public GraphQlController(ISchema schema, IDocumentExecuter executer)
    {
        this.schema = schema;
        this.executer = executer;
    }

    [HttpPost]
    public Task<ExecutionResult> Post(
        [BindRequired, FromBody] PostBody body,
        CancellationToken cancellation)
    {
        return Execute(body.Query, body.OperationName, body.Variables, cancellation);
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
        var options = new ExecutionOptions
        {
            Schema = schema,
            Query = query,
            OperationName = operationName,
            Inputs = variables?.ToInputs(),
            CancellationToken = cancellation,
#if (DEBUG)
            ExposeExceptions = true,
            EnableMetrics = true,
#endif
        };
        options.UseFluentValidation(ValidatorCacheBuilder.ValidatorTypeCache);

        return executer.ExecuteAsync(options);
    }

    static JObject? ParseVariables(string? variables)
    {
        if (variables == null)
        {
            return null;
        }

        try
        {
            return JObject.Parse(variables);
        }
        catch (Exception exception)
        {
            throw new Exception("Could not parse variables.", exception);
        }
    }
}