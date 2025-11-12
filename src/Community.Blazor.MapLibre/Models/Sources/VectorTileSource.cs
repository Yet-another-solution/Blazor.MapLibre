using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Sources;

/// <summary>
/// Represents a vector tile source. Vector sources provide tiled vector data in Mapbox Vector Tile format.
/// </summary>
public class VectorTileSource : ISource
{
    /// <inheritdoc />
    [JsonPropertyName("type")]
    public string Type => "vector";

    /// <summary>
    /// URL to a TileJSON resource providing metadata about this source. Optional.
    /// </summary>
    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url { get; set; }

    /// <summary>
    /// An array of URLs to the vector tiles. URL patterns can use placeholders like `{z}`, `{x}`, and `{y}`. Optional.
    /// </summary>
    [JsonPropertyName("tiles")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Tiles { get; set; }

    /// <summary>
    /// The bounding box for the source, specified as an array `[sw.lng, sw.lat, ne.lng, ne.lat]`. Optional.
    /// </summary>
    [JsonPropertyName("bounds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double[]? Bounds { get; set; }

    /// <summary>
    /// The tiling scheme, either `xyz` (standard Slippy map tilenames) or `tms` (OSGeo TMS). Default is `xyz`. Optional.
    /// </summary>
    [JsonPropertyName("scheme")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Scheme { get; set; } = "xyz";

    /// <summary>
    /// Minimum zoom level for which tiles are available, as in the TileJSON spec.
    /// Default is 0. Optional.
    /// </summary>
    [JsonPropertyName("minzoom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? MinZoom { get; set; }

    /// <summary>
    /// Maximum zoom level for which tiles are available, as in the TileJSON spec. Data from tiles at the maxzoom are used when displaying the map at higher zoom levels.
    /// Default is 22. Optional.
    /// </summary>
    [JsonPropertyName("maxzoom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? MaxZoom { get; set; }
}
