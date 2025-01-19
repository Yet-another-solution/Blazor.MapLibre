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
    /// Gets or sets the attribution text. Optional.
    /// </summary>
    string? Attribution { get; set; }
}