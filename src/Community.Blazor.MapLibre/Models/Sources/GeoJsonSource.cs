using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Converter;
using Community.Blazor.MapLibre.Models.Feature;
using OneOf;

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
    /// The GeoJSON data, either as an inline GeoJSON object or a URL string to an external GeoJSON file. Required.
    /// Use <see cref="IFeature"/> for inline GeoJSON data or <see cref="string"/> for a URL.
    /// </summary>
    [JsonPropertyName("data")]
    [JsonConverter(typeof(GeoJsonDataConverter))]
    public required OneOf<IFeature, string> Data { get; set; }
}
