﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using GraphQL;
using GraphQL.Execution;
using VerifyXunit;
using Xunit;

#region QueryTests

[UsesVerify]
public class QueryTests
{
    [Fact]
    public Task RunInputQuery()
    {
        var field = new Query().GetField("inputQuery");

        GraphQLUserContext userContext = new();
        FluentValidationExtensions.AddCacheToContext(
            userContext,
            ValidatorCacheBuilder.Instance);

        var input = new MyInput
        {
            Content = "TheContent"
        };
        ResolveFieldContext fieldContext = new()
        {
            Arguments = new Dictionary<string, ArgumentValue>
            {
                {
                    "input", new ArgumentValue(input, ArgumentSource.Variable)
                }
            },
            UserContext = userContext
        };
        var result = (Result) field.Resolver.Resolve(fieldContext);
        return Verifier.Verify(result);
    }

    [Fact]
    public Task RunInvalidInputQuery()
    {
        Thread.CurrentThread.CurrentUICulture = new("en-US");
        var field = new Query().GetField("inputQuery");

        GraphQLUserContext userContext = new();
        FluentValidationExtensions.AddCacheToContext(
            userContext,
            ValidatorCacheBuilder.Instance);

        var value = new Dictionary<string, object>();
        ResolveFieldContext fieldContext = new()
        {
            Arguments = new Dictionary<string, ArgumentValue>
            {
                {
                    "input", new ArgumentValue(value, ArgumentSource.Variable)
                }
            },
            UserContext = userContext
        };
        var exception = Assert.Throws<ValidationException>(
            () => field.Resolver.Resolve(fieldContext));
        return Verifier.Verify(exception.Message);
    }
}

#endregion