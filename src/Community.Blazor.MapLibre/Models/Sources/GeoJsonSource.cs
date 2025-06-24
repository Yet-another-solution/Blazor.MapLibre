using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Models.Feature;

namespace Community.Blazor.MapLibre.Models.Sources;

/// <summary>
/// Represents a GeoJSON source. GeoJSON sources provide either inline GeoJSON data or a URL to a GeoJSON file.
/// They can support clustering and other custom behaviors for point features.
/// </summary>
public class GeoJsonSource : ISource
{
    /// <inheritdoc />
    [JsonPropertyName("type")]
    public string Type => "geojson";

    /// <summary>
    /// The GeoJSON data, either as an inline object or a URL to an external GeoJSON file. Required.
    /// </summary>
    [JsonPropertyName("data")]
    public required IFeature Data { get; set; }
}
