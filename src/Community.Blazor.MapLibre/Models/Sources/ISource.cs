
using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Sources;

/// <summary>
/// Represents the base class for all map data sources. Each source type (e.g., vector, raster, geojson) will inherit from this class.
/// </summary>
[JsonDerivedType(typeof(GeoJsonSource))]
[JsonDerivedType(typeof(ImageSource))]
[JsonDerivedType(typeof(RasterTileSource))]
[JsonDerivedType(typeof(VectorTileSource))]
[JsonDerivedType(typeof(VideoSource))]
public interface ISource
{
    /// <summary>
    /// Defines the source type (e.g., vector, raster, geojson, etc.).
    /// </summary>
    [JsonPropertyName("type")]
    string Type { get; }

    /// <summary>
    /// The attribution for the source.
    /// </summary>
    [JsonPropertyName("attribution")]
    string? Attribution { get; set; }

    /// <summary>
    /// The id for the source. Must not be used by any existing source.
    /// </summary>
    [JsonPropertyName("id")]
    string Id { get; set; }

    /// <summary>
    /// `false` if tiles can be drawn outside their boundaries, `true` if they cannot.
    /// </summary>
    [JsonPropertyName("isTileClipped")]
    bool? IsTileClipped { get; set; }

    /// <summary>
    /// The maximum zoom level for the source.
    /// </summary>
    [JsonPropertyName("maxzoom")]
    int MaxZoom { get; set; }

    /// <summary>
    /// The minimum zoom level for the source.
    /// </summary>
    [JsonPropertyName("minzoom")]
    int MinZoom { get; set; }

    /// <summary>
    /// `true` if tiles should be sent back to the worker for each overzoomed zoom level, `false` if not.
    /// </summary>
    [JsonPropertyName("reparseOverscaled")]
    bool? ReparseOverscaled { get; set; }

    /// <summary>
    /// `true` if zoom levels are rounded to the nearest integer in the source data, `false` if they are floor-ed to the nearest integer.
    /// </summary>
    [JsonPropertyName("roundZoom")]
    bool? RoundZoom { get; set; }

    /// <summary>
    /// The canonical tile ID for the source.
    /// </summary>
    [JsonPropertyName("tileID")]
    string? TileID { get; set; }

    /// <summary>
    /// The tile size for the source.
    /// </summary>
    [JsonPropertyName("tileSize")]
    int TileSize { get; set; }

    /// <summary>
    /// The vector layer IDs for the source.
    /// </summary>
    [JsonPropertyName("vectorLayerIds")]
    string[]? VectorLayerIds { get; set; }
}
