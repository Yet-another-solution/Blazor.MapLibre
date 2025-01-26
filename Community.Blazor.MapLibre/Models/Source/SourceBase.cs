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
    /// Contains attribution text for the source, displayed on the map UI to credit the data provider.
    /// </summary>
    public string? Attribution { get; set; } = "";
}