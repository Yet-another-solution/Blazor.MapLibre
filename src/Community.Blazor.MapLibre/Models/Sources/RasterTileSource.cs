using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Sources;

/// <summary>
/// Represents a raster tile source. Raster sources provide tiled raster image data, typically used for basemaps.
/// </summary>
public class RasterTileSource : ISource
{
    /// <inheritdoc />
    [JsonPropertyName("type")]
    public string Type => "raster";
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
    /// URL to a TileJSON resource providing metadata about this source. Optional.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// An array of URLs to the raster tiles. URL patterns can use placeholders like `{z}`, `{x}`, and `{y}`. Optional.
    /// </summary>
    [JsonPropertyName("tiles")]
    public List<string>? Tiles { get; set; }

    /// <summary>
    /// The bounding box for the source, specified as an array `[sw.lng, sw.lat, ne.lng, ne.lat]`. Optional.
    /// </summary>
    [JsonPropertyName("bounds")]
    public double[]? Bounds { get; set; }
}
