using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using ObjectApproval;
using Xunit;

public class IntegrationTests
{
    [Fact]
    public async Task Foo()
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
        var result = await RunQuery(queryString, null);
        ObjectApprover.VerifyWithJson(result);
    }

    static async Task<object> RunQuery(string queryString, Inputs inputs)
    {
        return await QueryExecutor.ExecuteQuery(queryString, inputs);
    }

    static IEnumerable<Type> GetGraphQlTypes()
    {
        return typeof(IntegrationTests).Assembly
            .GetTypes()
            .Where(x => !x.IsAbstract && typeof(GraphType).IsAssignableFrom(x));
    }
}