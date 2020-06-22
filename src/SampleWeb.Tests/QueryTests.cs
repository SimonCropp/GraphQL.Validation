using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using GraphQL;
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
        return Verifier.Verify(result);
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
        return Verifier.Verify(exception.Message);
    }
}

#endregion