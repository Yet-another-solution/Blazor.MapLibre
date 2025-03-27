using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Feature;

[JsonDerivedType(typeof(FeatureCollection))]
[JsonDerivedType(typeof(FeatureFeature))]
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
