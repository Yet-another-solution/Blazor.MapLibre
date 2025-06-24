using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Camera;

public class FitBoundOptions : FlyToOptions
{
    /// <summary>
    /// If `true`, the map transitions using {@link Map#easeTo}. If `false`, the map transitions using {@link Map#flyTo}.
    /// See those functions and {@link AnimationOptions} for information about options available.
    /// @defaultValue false
    /// </summary>
    [JsonPropertyName("linear")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Linear { get; set; }

    /// <summary>
    /// The maximum zoom level to allow when the map view transitions to the specified bounds.
    /// </summary>
    [JsonPropertyName("maxZoom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? MaxZoom { get; set; }

    /// <summary>
    /// The center of the given bounds relative to the map's center, measured in pixels.
    /// </summary>
    [JsonPropertyName("offset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public new PointLike? Offset { get; set; }
}
