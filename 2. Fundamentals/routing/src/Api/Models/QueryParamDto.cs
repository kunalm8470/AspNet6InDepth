using System.Text.Json.Serialization;

namespace Api.Models;

public class QueryParamDto
{
    [JsonPropertyName("query1")]
    public string Query1 { get; set; }

    [JsonPropertyName("query2")]
    public string Query2 { get; set; }
}
