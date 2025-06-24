using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models;

public class Popup
{
    [JsonPropertyName("content")]
    public required string Content { get; set; }

    [JsonPropertyName("lngLat")]
    public required LngLat Coordinates { get; set; }
}
