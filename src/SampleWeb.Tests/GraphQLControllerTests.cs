using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;

#region GraphQLControllerTests

[UsesVerify]
public class GraphQLControllerTests
{
    [Fact]
    public async Task RunQuery()
    {
        using var server = GetTestServer();
        using var client = server.CreateClient();
        var query = """
                    {
                      inputQuery(input: {content: "TheContent"}) {
                        data
                      }
                    }
                    """;
        var body = new
        {
            query
        };
        var serialized = JsonConvert.SerializeObject(body);
        using var content = new StringContent(
            serialized,
            Encoding.UTF8,
            "application/json");
        using var request = new HttpRequestMessage(HttpMethod.Post, "graphql")
        {
            Content = content
        };
        using var response = await client.SendAsync(request);
        await Verify(response);
    }

    static TestServer GetTestServer()
    {
        var builder = new WebHostBuilder();
        builder.UseStartup<Startup>();
        return new(builder);
    }
}

#endregion