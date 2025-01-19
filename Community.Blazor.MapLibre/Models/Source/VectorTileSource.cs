namespace Community.Blazor.MapLibre.Models.Source;

/// <summary>
/// Represents a vector tile source. Vector sources provide tiled vector data in Mapbox Vector Tile format.
/// </summary>
public class VectorTileSource : SourceBase
{
    /// <inheritdoc />
    public override string Type => "vector";

    /// <summary>
    /// URL to a TileJSON resource providing metadata about this source. Optional.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// An array of URLs to the vector tiles. URL patterns can use placeholders like `{z}`, `{x}`, and `{y}`. Optional.
    /// </summary>
    public List<string>? Tiles { get; set; }

    /// <summary>
    /// The bounding box for the source, specified as an array `[sw.lng, sw.lat, ne.lng, ne.lat]`. Optional.
    /// </summary>
    public double[]? Bounds { get; set; }

    /// <summary>
    /// The tiling scheme, either `xyz` (standard Slippy map tilenames) or `tms` (OSGeo TMS). Default is `xyz`. Optional.
    /// </summary>
    public string? Scheme { get; set; } = "xyz";

    /// <summary>
    /// A list of layer IDs included in this vector source. Optional.
    /// </summary>
    public List<string>? VectorLayerIds { get; set; }

    /// <summary>
    /// Indicates if tiles should be re-parsed for higher zoom levels rather than just scaling their contents. Optional.
    /// </summary>
    public bool? ReparseOverscaled { get; set; }
}