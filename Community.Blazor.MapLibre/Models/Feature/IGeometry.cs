using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Converter;

namespace Community.Blazor.MapLibre.Models.Feature;

[JsonConverter(typeof(GeometryConverter))]
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
