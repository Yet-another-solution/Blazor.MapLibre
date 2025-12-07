using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Models.LayerFeatures;

namespace Community.Blazor.MapLibre.Models.Event;

public class MapMouseEvent
{
    [JsonPropertyName("point")]
    public required PointLike Point { get; set; }

    [JsonPropertyName("lngLat")]
    public required LngLat LngLat { get; set; }

    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required EventType Type { get; set; }

    [JsonPropertyName("_defaultPrevented")]
    public bool? DefaultPrevented { get; set; }

    [JsonPropertyName("features")]
    public LayerFeature[] Features { get; set; } = [];
}