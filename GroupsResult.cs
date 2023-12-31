using System.Text.Json.Serialization;

namespace MicrosoftGraphBenchmark;
public class GroupsResult
{
    public List<Group>? Value { get; set; }
    [JsonPropertyName("@odata.nextLink")]
    public string? NextLink { get; set; }
}