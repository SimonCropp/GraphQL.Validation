using System.Threading.Tasks;
using GraphQL.FluentValidation;
using VerifyXunit;
using Xunit;

[UsesVerify]
public class IntegrationTests
{
    static IValidatorCache cache;

    static IntegrationTests()
    {
        cache = new ValidatorInstanceCache(
            type =>
            {
                if (type == typeof(NoValidatorInput))
                {
                    return new NoValidatorInputValidator<string>();
                }

                return null;
            });
        cache.AddValidatorsFromAssemblyContaining<IntegrationTests>();
    }

    [Fact]
    public async Task AsyncValid()
    {
        var queryString = @"
{
  asyncQuery
    (
      input: {
        content: ""TheContent""
      }
    )
  {
    data
  }
}";
        var result = await QueryExecutor.ExecuteQuery(queryString, null, cache);
        await Verifier.VerifyJson(result);
    }

    [Fact]
    public async Task AsyncInvalid()
    {
        var queryString = @"
{
  asyncQuery
    (
      input: {
        content: """"
      }
    )
  {
    data
  }
}";
        var result = await QueryExecutor.ExecuteQuery(queryString, null, cache);
        await Verifier.VerifyJson(result);
    }

    [Fact]
    public async Task ValidNull()
    {
        var queryString = @"
{
  inputQuery
  {
    data
  }
}";
        var result = await QueryExecutor.ExecuteQuery(queryString, null, cache);
        await Verifier.VerifyJson(result);
    }

    [Fact]
    public async Task NoValidatorValid()
    {
        var queryString = @"
{
  noValidatorQuery
    (
      input: {
        content: ""TheContent""
      }
    )
  {
    data
  }
}";
        var result = await QueryExecutor.ExecuteQuery(queryString, null, cache);
        await Verifier.VerifyJson(result);
    }

    [Fact]
    public async Task NoValidatorInvalid()
    {
        var queryString = @"
{
  noValidatorQuery
    (
      input: {
        content: """"
      }
    )
  {
    data
  }
}";
        var result = await QueryExecutor.ExecuteQuery(queryString, null, cache);
        await Verifier.VerifyJson(result);
    }

    [Fact]
    public async Task Valid()
    {
        var queryString = @"
{
  inputQuery
    (
      input: {
        content: ""TheContent""
      }
    )
  {
    data
  }
}";
        var result = await QueryExecutor.ExecuteQuery(queryString, null, cache);
        await Verifier.VerifyJson(result);
    }

    [Fact]
    public async Task Invalid()
    {
        var queryString = @"
{
  inputQuery
    (
      input: {
        content: """"
      }
    )
  {
    data
  }
}";
        var result = await QueryExecutor.ExecuteQuery(queryString, null, cache);
        await Verifier.VerifyJson(result);
    }

    [Fact]
    public async Task ComplexValid()
    {
        var queryString = @"
{
  complexInputQuery
    (
      input: {
        inner: {
          content: ""TheContent""
        },
        items: [
            { id: 1, content: ""Some content 1"" },
            { id: 2, content: ""Some content 2"" }
        ]
      }
    )
  {
    data
  }
}";
        var result = await QueryExecutor.ExecuteQuery(queryString, null, cache);
        await Verifier.VerifyJson(result);
    }

    [Fact]
    public async Task ComplexInvalid()
    {
        var queryString = @"
{
  complexInputQuery
    (
      input: {
        inner: {
          content: """"
        },
        items: []
      }
    )
  {
    data
  }
}";
        var result = await QueryExecutor.ExecuteQuery(queryString, null, cache);
        await Verifier.VerifyJson(result);
    }

    [Fact]
    public async Task DerivedComplexInvalid()
    {
        var queryString = @"
{
  derivedComplexInputQuery
    (
      input: {
        inner: {
          content: """"
        },
        items: []
      }
    )
  {
    data
  }
}";
        var result = await QueryExecutor.ExecuteQuery(queryString, null, cache);
        await Verifier.VerifyJson(result);
    }

    [Fact]
    public async Task ComplexInvalid2()
    {
        var queryString = @"
{
  complexInputQuery
    (
      input: {
        inner: null,
        items: null
      }
    )
  {
    data
  }
}";
        var result = await QueryExecutor.ExecuteQuery(queryString, null, cache);
        await Verifier.VerifyJson(result);
    }

    [Fact]
    public async Task AsyncComplexValid()
    {
        var queryString = @"
{
  asyncComplexInputQuery
    (
      input: {
        inner: {
          content: ""TheContent""
        },
        items: [
            { id: 1, content: ""Some content 1"" },
            { id: 2, content: ""Some content 2"" }
        ]
      }
    )
  {
    data
  }
}";
        var result = await QueryExecutor.ExecuteQuery(queryString, null, cache);
        await Verifier.VerifyJson(result);
    }

    [Fact]
    public async Task AsyncComplexInvalid()
    {
        var queryString = @"
{
  asyncComplexInputQuery
    (
      input: {
        inner: {
          content: """"
        },
        items: null
      }
    )
  {
    data
  }
}";
        var result = await QueryExecutor.ExecuteQuery(queryString, null, cache);
        await Verifier.VerifyJson(result);
    }
}