using System;
using System.Net.Http;
using System.Net.Http.Headers;
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
        using var response = await ClientQueryExecutor.ExecutePost(client, query);
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

  public static class ClientQueryExecutor
    {
        static string uri = "graphql";

        public static Task<HttpResponseMessage> ExecutePost(HttpClient client, string query, object? variables = null, Action<HttpHeaders>? headerAction = null)
        {
            var body = new
            {
                query,
                variables
            };
            var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(ToJson(body), Encoding.UTF8, "application/json")
            };
            headerAction?.Invoke(request.Headers);
            return client.SendAsync(request);
        }

        static string ToJson(object? target)
        {
            if (target == null)
            {
                return string.Empty;
            }

            return JsonConvert.SerializeObject(target);
        }

    }