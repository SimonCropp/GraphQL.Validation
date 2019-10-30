using System.Threading.Tasks;
using GraphQL.FluentValidation;
using Xunit;
using Xunit.Abstractions;

public class IntegrationTests :
    XunitApprovalBase
{
    static ValidatorTypeCache typeCache;

    static IntegrationTests()
    {
        typeCache = new ValidatorTypeCache();
        typeCache.AddValidatorsFromAssemblyContaining<IntegrationTests>();
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
        var result = await QueryExecutor.ExecuteQuery(queryString, null, typeCache);
        ObjectApprover.Verify(result);
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
        var result = await QueryExecutor.ExecuteQuery(queryString, null, typeCache);
        ObjectApprover.Verify(result);
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
        var result = await QueryExecutor.ExecuteQuery(queryString, null, typeCache);
        ObjectApprover.Verify(result);
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
        var result = await QueryExecutor.ExecuteQuery(queryString, null, typeCache);
        ObjectApprover.Verify(result);
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
        }
      }
    )
  {
    data
  }
}";
        var result = await QueryExecutor.ExecuteQuery(queryString, null, typeCache);
        ObjectApprover.Verify(result);
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
        }
      }
    )
  {
    data
  }
}";
        var result = await QueryExecutor.ExecuteQuery(queryString, null, typeCache);
        ObjectApprover.Verify(result);
    }

    [Fact]
    public async Task ComplexInvalid2()
    {
        var queryString = @"
{
  complexInputQuery
    (
      input: {
        inner: null
      }
    )
  {
    data
  }
}";
        var result = await QueryExecutor.ExecuteQuery(queryString, null, typeCache);
        ObjectApprover.Verify(result);
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
        }
      }
    )
  {
    data
  }
}";
        var result = await QueryExecutor.ExecuteQuery(queryString, null, typeCache);
        ObjectApprover.Verify(result);
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
        }
      }
    )
  {
    data
  }
}";
        var result = await QueryExecutor.ExecuteQuery(queryString, null, typeCache);
        ObjectApprover.Verify(result);
    }


    public IntegrationTests(ITestOutputHelper output) :
        base(output)
    {
    }
}