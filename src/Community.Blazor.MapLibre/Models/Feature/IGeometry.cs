using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Feature;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(LineGeometry), typeDiscriminator: "LineString")]
[JsonDerivedType(typeof(MultiLineGeometry), typeDiscriminator: "MultiLineString")]
[JsonDerivedType(typeof(MultiPointGeometry), typeDiscriminator: "MultiPoint")]
[JsonDerivedType(typeof(MultiPolygonGeometry), typeDiscriminator: "MultiPolygon")]
[JsonDerivedType(typeof(PointGeometry), typeDiscriminator: "Point")]
[JsonDerivedType(typeof(PolygonGeometry), typeDiscriminator: "Polygon")]
public interface IGeometry
{
    [JsonPropertyName("type")]
    GeometryType Type { get; }

    /// <summary>
    /// Gets the bounding box of the geometry.
    /// </summary>
    /// <returns>
    /// A <see cref="LngLatBounds"/> object representing the bounding box of the geometry.
    /// </returns>
    LngLatBounds GetBounds();
}
