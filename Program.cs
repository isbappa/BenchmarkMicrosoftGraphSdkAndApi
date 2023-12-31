using BenchmarkDotNet.Running;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MicrosoftGraphBenchmark;
public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<GroupBenchmark>();
        Console.WriteLine(summary);
    }

    // Get groups using Microsoft Graph SDK with paging
    public static async Task<ConcurrentBag<Microsoft.Graph.Models.Group>?> GetGroupsUsingMicrosoftSdkAsync(GraphServiceClient graphClient)
    {
        var groupsList = new ConcurrentBag<Microsoft.Graph.Models.Group>();

        var requestGroup = graphClient.Groups
            .GetAsync()
            .GetAwaiter()
            .GetResult();

        if (requestGroup is null)
        {
            Console.WriteLine("No group found in Azure AD");
            return null;
        }

        var pageIterator = PageIterator<Microsoft.Graph.Models.Group, GroupCollectionResponse>
            .CreatePageIterator(graphClient, requestGroup,
            group =>
            {
                groupsList.Add(group);
                return true;
            });
        await pageIterator.IterateAsync();

        return groupsList;
    }


    // Get groups using Microsoft REST API with paging
    public static async Task<List<Group>> GetGroupsUsingMicrosoftRestApiAsync(string token)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        string requestUrl = "https://graph.microsoft.com/v1.0/groups?$top=999"; // Fetch 999 groups at a time

        var groups = new ConcurrentBag<Group>();

        while (!string.IsNullOrEmpty(requestUrl)) // Continue until there are no more pages
        {
            var response = await client.GetAsync(requestUrl);
            var content = await response.Content.ReadAsStringAsync();

            // Deserialize the response to a GroupsResult
            var groupsResult = JsonSerializer.Deserialize<GroupsResult>(content, options);

            // Process the groups in parallel
            Parallel.ForEach(groupsResult?.Value!, (group) =>
            {
                // Do something with group...
                groups.Add(group);
            });

            // Get the next page of groups
            requestUrl = groupsResult?.NextLink!;
        }

        return groups.ToList();
    }
}

