using Azure.Core;
using Azure.Identity;
using BenchmarkDotNet.Attributes;
using Microsoft.Graph;

namespace MicrosoftGraphBenchmark;
public class GroupBenchmark
{
    private string? token;
    private GraphServiceClient? graphClient;

    [GlobalSetup]
    public void Setup()
    {
        string tenantId = "YOUR TENANT ID HERE";
        string clientId = "YOUR CLIENT ID HERE";
        string clientSecret = "YOUR CLIENT SECRET HERE";

        var scopes = new[] { "https://graph.microsoft.com/.default" };
        var options = new TokenCredentialOptions
        {
            AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
        };
        var clientSecretCredential = new ClientSecretCredential(
            tenantId, clientId, clientSecret, options);

        this.graphClient = new GraphServiceClient(clientSecretCredential, scopes);

        this.token = clientSecretCredential.GetToken(new TokenRequestContext(scopes) { }).Token;
    }

    [Benchmark]
    public async Task GetGroupsUsingMicrosoftSdkAsyncBenchmark() => await Program.GetGroupsUsingMicrosoftSdkAsync(graphClient!);

    [Benchmark]
    public async Task GetGroupsUsingMicrosoftRestApiAsyncBenchmark() => await Program.GetGroupsUsingMicrosoftRestApiAsync(token!);
}