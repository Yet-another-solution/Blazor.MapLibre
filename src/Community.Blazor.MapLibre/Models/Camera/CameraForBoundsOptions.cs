using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Models.Padding;
using OneOf;

namespace Community.Blazor.MapLibre.Models.Camera;

public class CameraForBoundsOptions : CameraOptions
{
    /// <summary>
    /// The maximum zoom level to allow when the camera would transition to the specified bounds.
    /// </summary>
    [JsonPropertyName("maxZoom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public decimal? MaxZoom { get; set; }
    /// <summary>
    /// The center of the given bounds relative to the map's center, measured in pixels.
    /// </summary>
    [JsonPropertyName("offset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PointLike? Offset { get; set; }
    /// <summary>
    /// The amount of padding in pixels to add to the given bounds.
    /// </summary>
    [JsonPropertyName("padding")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public OneOf<decimal, PaddingOptions> Padding { get; set; }
}