﻿using FluentValidation;
using GraphQL;
using GraphQL.Execution;

#region QueryTests

public class QueryTests
{
    [Fact]
    public async Task RunInputQuery()
    {
        var field = new Query().GetField("inputQuery")!;

        var userContext = new GraphQLUserContext();
        FluentValidationExtensions.AddCacheToContext(
            userContext,
            ValidatorCacheBuilder.Instance);

        var input = new MyInput
        {
            Content = "TheContent"
        };
        var fieldContext = new ResolveFieldContext
        {
            Arguments = new Dictionary<string, ArgumentValue>
            {
                {
                    "input", new(input, ArgumentSource.Variable)
                }
            },
            UserContext = userContext
        };
        var result = await field.Resolver!.ResolveAsync(fieldContext);
        await Verify(result);
    }

    [Fact]
    public Task RunInvalidInputQuery()
    {
        Thread.CurrentThread.CurrentUICulture = new("en-US");
        var field = new Query().GetField("inputQuery")!;

        var userContext = new GraphQLUserContext();
        FluentValidationExtensions.AddCacheToContext(
            userContext,
            ValidatorCacheBuilder.Instance);

        var input = new MyInput
        {
            Content = null!
        };
        var fieldContext = new ResolveFieldContext
        {
            Arguments = new Dictionary<string, ArgumentValue>
            {
                {
                    "input", new(input, ArgumentSource.Variable)
                }
            },
            UserContext = userContext
        };
        var exception = Assert.Throws<ValidationException>(
            () => field.Resolver!.ResolveAsync(fieldContext));
        return Verify(exception.Message);
    }
}

#endregion