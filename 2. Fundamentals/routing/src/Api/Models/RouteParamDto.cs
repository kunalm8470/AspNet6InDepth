using System.Text.Json.Serialization;

namespace Api.Models;

public class RouteParamDto
{
    [JsonPropertyName("value1")]
    public int Value1 { get; set; }

    [JsonPropertyName("value2")]
    public double Value2 { get; set; }
}
