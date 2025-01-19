namespace Community.Blazor.MapLibre.Models.Source;

/// <summary>
/// Represents a raster tile source. Raster sources provide tiled raster image data, typically used for basemaps.
/// </summary>
public class RasterTileSource : SourceBase
{
    /// <inheritdoc />
    public override string Type => "raster";

    /// <summary>
    /// URL to a TileJSON resource providing metadata about this source. Optional.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// An array of URLs to the raster tiles. URL patterns can use placeholders like `{z}`, `{x}`, and `{y}`. Optional.
    /// </summary>
    public List<string>? Tiles { get; set; }

    /// <summary>
    /// The bounding box for the source, specified as an array `[sw.lng, sw.lat, ne.lng, ne.lat]`. Optional.
    /// </summary>
    public double[]? Bounds { get; set; }
}