using System.Text.Json;
using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Models.Feature;

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

    /// This is currently a <see cref="SimpleFeature"/>, as we cannot fully deserialize <see cref="IFeature"/> atm.
    /// Once layers can be successfully deserialize, this can be changed to <see cref="IFeature"/>.
    [JsonPropertyName("features")]
    public List<SimpleFeature> Features { get; set; } = [];
}

public class SimpleFeature
{
    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("geometry")]
    public required IGeometry Geometry { get; set; }

    [JsonPropertyName("properties")]
    public Dictionary<string, JsonElement> Properties { get; set; } = [];
}
