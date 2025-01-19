namespace Community.Blazor.MapLibre.Models.Source;

/// <summary>
/// Represents the base interface for all source types.
/// </summary>
public interface ISource
{
    /// <summary>
    /// Gets the type of the source (e.g., vector, raster, geojson, etc.).
    /// </summary>
    string Type { get; }

    /// <summary>
    /// Gets or sets the unique identifier for the source.
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// Gets or sets the minimum zoom level for the source.
    /// </summary>
    double MinZoom { get; set; }

    /// <summary>
    /// Gets or sets the maximum zoom level for the source.
    /// </summary>
    double MaxZoom { get; set; }

    /// <summary>
    /// Gets or sets the tile size in pixels. Optional.
    /// </summary>
    int? TileSize { get; set; }

    /// <summary>
    /// Gets or sets the attribution text. Optional.
    /// </summary>
    string? Attribution { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether zoom levels are rounded or floored. Optional.
    /// </summary>
    bool? RoundZoom { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating if tiles are clipped. Optional.
    /// </summary>
    bool? IsTileClipped { get; set; }

    /// <summary>
    /// Handles the serialization of the source.
    /// </summary>
    /// <returns>A serializable representation of the source.</returns>
    object Serialize();

    /// <summary>
    /// Prepares the source before using it. Optional.
    /// </summary>
    void Prepare();

    /// <summary>
    /// Fires an event to all listeners. Optional.
    /// </summary>
    /// <param name="eventName">The event name.</param>
    /// <param name="eventData">The event data payload.</param>
    void Fire(string eventName, object eventData);

    /// <summary>
    /// Loads a tile for the source.
    /// </summary>
    Task LoadTile(object tile);

    /// <summary>
    /// Aborts a tile loading process. Optional.
    /// </summary>
    Task AbortTile(object tile);

    /// <summary>
    /// Unloads a tile. Optional.
    /// </summary>
    Task UnloadTile(object tile);

    /// <summary>
    /// Determines if the source is loaded.
    /// </summary>
    /// <returns>True if the source is loaded, otherwise false.</returns>
    bool Loaded();

    /// <summary>
    /// Determines if the source has a transition. Optional.
    /// </summary>
    /// <returns>True if the source supports transition, otherwise false.</returns>
    bool HasTransition();
}