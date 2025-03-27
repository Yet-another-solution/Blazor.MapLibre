
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
    public string Type { get; }
}
