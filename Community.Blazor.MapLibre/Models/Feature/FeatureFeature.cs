using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Feature;

public class FeatureFeature : IFeature
{
    [JsonPropertyName("type")]
    public string Type => "Feature";

    [JsonPropertyName("geometry")]
    public required IGeometry Geometry { get; set; }

    [JsonPropertyName("properties")]
    public Dictionary<string, object>? Properties { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public LngLatBounds GetBounds() => Geometry.GetBounds();
}
