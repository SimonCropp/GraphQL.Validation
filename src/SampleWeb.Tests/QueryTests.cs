using System.Collections.Generic;
using FluentValidation;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Xunit;
using Xunit.Abstractions;

#region QueryTests

public class QueryTests :
    XunitApprovalBase
{
    [Fact]
    public void RunInputQuery()
    {
        var field = new Query().GetField("inputQuery");

        var userContext = new GraphQlUserContext();
        FluentValidationExtensions.AddCacheToContext(userContext, ValidatorCacheBuilder.Instance);

        var input = new MyInput
        {
            Content = "TheContent"
        };
        var dictionary = input.AsDictionary();
        var fieldContext = new ResolveFieldContext
        {
            Arguments = new Dictionary<string, object>
            {
                {
                    "input", dictionary
                }
            },
            UserContext = userContext
        };
        var result = (Result) field.Resolver.Resolve(fieldContext);
        ObjectApprover.Verify(result);
    }

    [Fact]
    public void RunInvalidInputQuery()
    {
        var field = new Query().GetField("inputQuery");

        var userContext = new GraphQlUserContext();
        FluentValidationExtensions.AddCacheToContext(userContext, ValidatorCacheBuilder.Instance);
        var fieldContext = new ResolveFieldContext
        {
            Arguments = new Dictionary<string, object>
            {
                {
                    "input", new Dictionary<string, object>()
                }
            },
            UserContext = userContext
        };
        var exception = Assert.Throws<ValidationException>(() => field.Resolver.Resolve(fieldContext));
        ObjectApprover.Verify(exception.Message);
    }

    public QueryTests(ITestOutputHelper output) :
        base(output)
    {
    }
}

#endregion