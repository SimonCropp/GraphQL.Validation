using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using GraphQL;
using GraphQL.Types;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

#region QueryTests

public class QueryTests :
    VerifyBase
{
    [Fact]
    public Task RunInputQuery()
    {
        var field = new Query().GetField("inputQuery");

        var userContext = new GraphQLUserContext();
        userContext.AddValidatorCache(ValidatorCacheBuilder.Instance);

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
        return Verify(result);
    }

    [Fact]
    public Task RunInvalidInputQuery()
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        var field = new Query().GetField("inputQuery");

        var userContext = new GraphQLUserContext();
        userContext.AddValidatorCache(ValidatorCacheBuilder.Instance);
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
        var exception = Assert.Throws<ValidationException>(
            () => field.Resolver.Resolve(fieldContext));
        return Verify(exception.Message);
    }

    public QueryTests(ITestOutputHelper output) :
        base(output)
    {
    }
}

#endregion