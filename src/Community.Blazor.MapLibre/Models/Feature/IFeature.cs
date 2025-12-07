using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Feature;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(FeatureCollection), "FeatureCollection")]
[JsonDerivedType(typeof(FeatureFeature), "Feature")]
public interface IFeature
{
    /// <summary>
    /// Gets the bounding box of the geometry.
    /// </summary>
    /// <returns>
    /// A <see cref="LngLatBounds"/> object representing the bounding box of the geometry.
    /// </returns>
    LngLatBounds GetBounds();
}
