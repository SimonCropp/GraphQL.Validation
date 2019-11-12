using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

#region GraphQlControllerTests

public class GraphQlControllerTests :
    XunitApprovalBase
{
    static HttpClient client = null!;

    static GraphQlControllerTests()
    {
        var server = GetTestServer();
        client = server.CreateClient();
    }

    [Fact]
    public async Task RunQuery()
    {
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
        var serializeObject = JsonConvert.SerializeObject(body);
        using var content = new StringContent(serializeObject, Encoding.UTF8, "application/json");
        using var request = new HttpRequestMessage(HttpMethod.Post, "graphql"){Content = content};
        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        Approvals.VerifyJson(await response.Content.ReadAsStringAsync());
    }

    static TestServer GetTestServer()
    {
        var hostBuilder = new WebHostBuilder();
        hostBuilder.UseStartup<Startup>();
        return new TestServer(hostBuilder);
    }

    public GraphQlControllerTests(ITestOutputHelper output) :
        base(output)
    {
    }
}

#endregion