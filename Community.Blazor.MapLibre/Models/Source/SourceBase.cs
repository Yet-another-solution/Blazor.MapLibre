using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Source;

/// <summary>
/// Represents the base class for all map data sources. Each source type (e.g., vector, raster, geojson) will inherit from this class.
/// </summary>
public abstract class SourceBase : ISource
{
    /// <summary>
    /// Defines the source type (e.g., vector, raster, geojson, etc.).
    /// </summary>
    public abstract string Type { get; }

    /// <summary>
    /// The unique identifier of the source. This must be unique for every source within the map.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The minimum zoom level for which tiles from this source are available. Default is usually 0.
    /// </summary>
    public double MinZoom { get; set; } = 0;

    /// <summary>
    /// The maximum zoom level for which tiles from this source are available. Default is usually 22.
    /// </summary>
    public double MaxZoom { get; set; } = 22;

    /// <summary>
    /// The size of tiles for this source in pixels. Optional and may vary based on source type. Default is 512.
    /// </summary>
    public int? TileSize { get; set; }

    /// <summary>
    /// Contains attribution text for the source, displayed on the map UI to credit the data provider.
    /// </summary>
    public string? Attribution { get; set; }

    /// <summary>
    /// Indicates whether zoom levels in the source data are rounded to the nearest integer (true) or floored to the nearest integer (false). Optional.
    /// </summary>
    public bool? RoundZoom { get; set; }

    /// <summary>
    /// Determines whether tiles are clipped to their boundaries (true) or can overflow (false). Optional.
    /// </summary>
    public bool? IsTileClipped { get; set; }

    /// <inheritdoc />
    public virtual bool Loaded() => true;

    /// <inheritdoc />
    public virtual bool HasTransition() => false;

    public virtual object Serialize()
    {
        return new
        {
            Type,
            Id,
            MinZoom,
            MaxZoom,
            TileSize,
            Attribution
        };
    }

    /// <inheritdoc />
    public virtual void Prepare()
    {
    }

    /// <inheritdoc />
    public virtual void Fire(string eventName, object eventData)
    {
        Console.WriteLine($"Event Fired: {eventName}");
    }

    /// <inheritdoc />
    public virtual Task LoadTile(object tile)
    {
        throw new NotImplementedException("LoadTile must be implemented in the derived source.");
    }

    /// <inheritdoc />
    public virtual Task AbortTile(object tile)
    {
        throw new NotImplementedException("AbortTile is not supported for this source.");
    }

    /// <inheritdoc />
    public virtual Task UnloadTile(object tile)
    {
        throw new NotImplementedException("UnloadTile is not supported for this source.");
    }
}