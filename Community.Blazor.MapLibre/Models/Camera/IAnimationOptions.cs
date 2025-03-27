using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Camera;

/// <summary>
/// Defines options common to map movement methods that involve animation, such as <c>Map#panBy</c> and <c>Map#easeTo</c>.
/// </summary>
/// <remarks>
/// Controls the duration and easing function of the animation. All properties are optional.
/// </remarks>
public interface IAnimationOptions
{
    /// <summary>
    /// Determines whether animation occurs.
    /// </summary>
    /// <remarks>
    /// If set to <c>false</c>, no animation will occur.
    /// </remarks>
    [JsonPropertyName("animate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    bool? Animate { get; set; }

    /// <summary>
    /// The animation's duration, measured in milliseconds.
    /// </summary>
    [JsonPropertyName("duration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    double? Duration { get; set; }

    /// <summary>
    /// A function that takes a time in the range <c>0..1</c> and returns a number where <c>0</c> is the initial state and <c>1</c> is the final state.
    /// </summary>
    [JsonPropertyName("easing")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    Func<double, double>? Easing { get; set; }

    /// <summary>
    /// If <c>true</c>, the animation is considered essential and will not be affected by <c>prefers-reduced-motion</c>.
    /// </summary>
    [JsonPropertyName("essential")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    bool? Essential { get; set; }

    /// <summary>
    /// Determines whether the camera remains at a constant height based on sea level in 3D maps.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>. If set to <c>true</c>, the zoom level is recalculated after the animation
    /// to maintain the correct distance from the camera to the center-coordinate-altitude.
    /// </remarks>
    [JsonPropertyName("freezeElevation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    bool? FreezeElevation { get; set; }

    /// <summary>
    /// The offset of the target center relative to the real map container center at the end of the animation.
    /// </summary>
    [JsonPropertyName("offset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    PointLike? Offset { get; set; }
}
