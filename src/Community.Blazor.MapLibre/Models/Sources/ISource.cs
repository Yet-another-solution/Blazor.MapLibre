
using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Sources;

/// <summary>
/// Represents the base class for all map data sources. Each source type (e.g., vector, raster, geojson) will inherit from this class.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(CanvasSource), "canvas")]
[JsonDerivedType(typeof(GeoJsonSource), "geojson")]
[JsonDerivedType(typeof(ImageSource), "image")]
[JsonDerivedType(typeof(RasterDEMTileSource), "raster-dem")]
[JsonDerivedType(typeof(RasterTileSource), "raster")]
[JsonDerivedType(typeof(VectorTileSource), "vector")]
[JsonDerivedType(typeof(VideoSource), "video")]
public interface ISource;
