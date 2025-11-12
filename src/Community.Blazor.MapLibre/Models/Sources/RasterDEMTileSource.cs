using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Sources;

/// <summary>
/// Represents a raster DEM (Digital Elevation Model) tile source. Raster DEM sources provide tiled raster elevation data
/// for use with hillshading and 3D terrain rendering.
/// </summary>
public class RasterDEMTileSource : ISource
{
    /// <inheritdoc />
    [JsonPropertyName("type")]
    public string Type => "raster-dem";

    /// <summary>
    /// URL to a TileJSON resource providing metadata about this source. Optional.
    /// </summary>
    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url { get; set; }

    /// <summary>
    /// An array of URLs to the raster DEM tiles. URL patterns can use placeholders like `{z}`, `{x}`, and `{y}`. Optional.
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

    /// <summary>
    /// The size of the tiles in pixels. Default is 512. Optional.
    /// </summary>
    [JsonPropertyName("tileSize")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TileSize { get; set; }

    /// <summary>
    /// Contains an attribution to be displayed when the map is shown to a user. Optional.
    /// </summary>
    [JsonPropertyName("attribution")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Attribution { get; set; }

    /// <summary>
    /// The encoding used by the raster DEM tiles. Supported values are "mapbox" (Mapbox Terrain RGB) and "terrarium" (Mapzen Terrarium).
    /// Default is "mapbox". Optional.
    /// </summary>
    [JsonPropertyName("encoding")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Encoding { get; set; } = "mapbox";

    /// <summary>
    /// The red channel multiplier for decoding elevation values. Optional.
    /// </summary>
    [JsonPropertyName("redFactor")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? RedFactor { get; set; }

    /// <summary>
    /// The green channel multiplier for decoding elevation values. Optional.
    /// </summary>
    [JsonPropertyName("greenFactor")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? GreenFactor { get; set; }

    /// <summary>
    /// The blue channel multiplier for decoding elevation values. Optional.
    /// </summary>
    [JsonPropertyName("blueFactor")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? BlueFactor { get; set; }

    /// <summary>
    /// The base shift value for decoding elevation values. Optional.
    /// </summary>
    [JsonPropertyName("baseShift")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? BaseShift { get; set; }
}
