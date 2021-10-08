using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using VerifyXunit;
using Xunit;

#region GraphQLControllerTests
[UsesVerify]
public class GraphQLControllerTests
{
    [Fact]
    public async Task RunQuery()
    {
        using var server = GetTestServer();
        using var client = server.CreateClient();
        var query = @"
{
  inputQuery(input: {content: ""TheContent""}) {
    data
  }
}
";
        var body = new
        {
            query
        };
        var serialized = JsonConvert.SerializeObject(body);
        using StringContent content = new(
            serialized,
            Encoding.UTF8,
            "application/json");
        using HttpRequestMessage request = new(HttpMethod.Post, "graphql")
        {
            Content = content
        };
        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        await Verifier.Verify(await response.Content.ReadAsStringAsync());
    }

    static TestServer GetTestServer()
    {
        WebHostBuilder hostBuilder = new();
        hostBuilder.UseStartup<Startup>();
        return new(hostBuilder);
    }
}

#endregion