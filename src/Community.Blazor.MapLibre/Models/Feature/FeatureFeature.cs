using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Converter;

namespace Community.Blazor.MapLibre.Models.Feature;

public class FeatureFeature : IFeature
{
    [JsonPropertyName("id")]
    [JsonConverter(typeof(StringOrNumberConverter))]
    public string? Id { get; set; }

    [JsonPropertyName("geometry")]
    public required IGeometry Geometry { get; set; }

    [JsonPropertyName("properties")]
    public Dictionary<string, object>? Properties { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public LngLatBounds GetBounds() => Geometry.GetBounds();
}
