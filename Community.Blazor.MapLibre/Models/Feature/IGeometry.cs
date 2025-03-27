using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Feature;

[JsonDerivedType(typeof(LineGeometry))]
[JsonDerivedType(typeof(MultiLineGeometry))]
[JsonDerivedType(typeof(MultiPointGeometry))]
[JsonDerivedType(typeof(MultiPolygonGeometry))]
[JsonDerivedType(typeof(PointGeometry))]
[JsonDerivedType(typeof(PolygonGeometry))]
public interface IGeometry
{
    [JsonPropertyName("type")]
    string Type { get; }

    /// <summary>
    /// Gets the bounding box of the geometry.
    /// </summary>
    /// <returns>
    /// A <see cref="LngLatBounds"/> object representing the bounding box of the geometry.
    /// </returns>
    LngLatBounds GetBounds();
}
