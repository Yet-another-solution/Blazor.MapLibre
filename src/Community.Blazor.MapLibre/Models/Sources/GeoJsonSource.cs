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

    /// <inheritdoc />
    [JsonPropertyName("attribution")]
    public string? Attribution { get; set; }
    /// <inheritdoc />
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    /// <inheritdoc />
    [JsonPropertyName("isTileClipped")]
    public bool? IsTileClipped { get; set; }
    /// <inheritdoc />
    [JsonPropertyName("maxzoom")]
    public float? MaxZoom { get; set; }
    /// <inheritdoc />
    [JsonPropertyName("minzoom")]
    public float? MinZoom { get; set; }
    /// <inheritdoc />
    [JsonPropertyName("reparseOverscaled")]
    public bool? ReparseOverscaled { get; set; }
    /// <inheritdoc />
    [JsonPropertyName("roundZoom")]
    public bool? RoundZoom { get; set; }
    /// <inheritdoc />
    [JsonPropertyName("tileID")]
    public string? TileID { get; set; }
    /// <inheritdoc />
    [JsonPropertyName("tileSize")]
    public float? TileSize { get; set; }
    /// <inheritdoc />
    [JsonPropertyName("vectorLayerIds")]
    public string[]? VectorLayerIds { get; set; }

    /// <summary>
    /// The GeoJSON data, either as an inline object or a URL to an external GeoJSON file. Required.
    /// </summary>
    [JsonPropertyName("data")]
    public required IFeature Data { get; set; }
}
