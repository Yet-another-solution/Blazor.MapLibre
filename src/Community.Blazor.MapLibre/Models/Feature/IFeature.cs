using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Feature;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(FeatureCollection), typeDiscriminator: "FeatureCollection")]
[JsonDerivedType(typeof(FeatureFeature), typeDiscriminator: "Feature")]
public interface IFeature
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
