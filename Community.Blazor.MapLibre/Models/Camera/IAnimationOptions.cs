namespace Community.Blazor.MapLibre.Models.Camera;

/// <summary>
/// Options common to map movement methods that involve animation, controlling the duration and easing function
/// of the animation. All properties are optional.
/// </summary>
public interface IAnimationOptions
{
    /// <summary>
    /// The animation's duration, measured in milliseconds.
    /// </summary>
    double? Duration { get; set; }

    /// <summary>
    /// A function taking a time in the range 0..1 and returning a number where 0 is
    /// the initial state and 1 is the final state.
    /// </summary>
    Func<double, double>? Easing { get; set; }

    /// <summary>
    /// The offset of the target center relative to the real map container center at the end of animation.
    /// </summary>
    PointLike? Offset { get; set; }

    /// <summary>
    /// If false, no animation will occur.
    /// </summary>
    bool? Animate { get; set; }

    /// <summary>
    /// If true, then the animation is considered essential and will not be affected by
    /// 'prefers-reduced-motion' media feature.
    /// </summary>
    bool? Essential { get; set; }

    /// <summary>
    /// Default false. Needed in 3D maps to let the camera stay in a constant
    /// height based on sea-level. After the animation finishes, the zoom level will be recalculated
    /// in respect to the distance from the camera to the center-coordinate-altitude.
    /// </summary>
    bool? FreezeElevation { get; set; }
}