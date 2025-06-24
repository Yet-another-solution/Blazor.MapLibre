using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Camera;

public class FlyToOptions : IAnimationOptions, ICameraOptions
{
    /// <summary>
    /// The zooming "curve" that will occur along the flight path.
    /// </summary>
    /// <remarks>
    /// A high value maximizes zooming for an exaggerated animation, while a low value minimizes zooming for an effect closer to <c>Map#easeTo</c>.
    /// <para>1.42 is the average value selected in a user study discussed in van Wijk (2003).</para>
    /// <para>A value of <c>Math.Pow(6, 0.25)</c> would be equivalent to the root mean squared average velocity.</para>
    /// <para>A value of <c>1</c> would produce a circular motion.</para>
    /// </remarks>
    [JsonPropertyName("curve")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Curve { get; set; }

    /// <summary>
    /// The animation's maximum duration, measured in milliseconds.
    /// </summary>
    /// <remarks>
    /// If the duration exceeds the maximum duration, it resets to <c>0</c>.
    /// </remarks>
    [JsonPropertyName("maxDuration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? MaxDuration { get; set; }

    /// <summary>
    /// The zero-based zoom level at the peak of the flight path.
    /// </summary>
    /// <remarks>
    /// If <c>options.curve</c> is specified, this option is ignored.
    /// </remarks>
    [JsonPropertyName("minZoom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? MinZoom { get; set; }

    /// <summary>
    /// The amount of padding in pixels to add to the given bounds.
    /// </summary>
    [JsonPropertyName("padding")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Padding { get; set; }

    /// <summary>
    /// The average speed of the animation measured in screenfuls per second, assuming a linear timing curve.
    /// </summary>
    /// <remarks>
    /// If <c>options.speed</c> is specified, this option is ignored.
    /// </remarks>
    [JsonPropertyName("screenSpeed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? ScreenSpeed { get; set; }

    /// <summary>
    /// The average speed of the animation defined in relation to <c>options.curve</c>.
    /// </summary>
    /// <remarks>
    /// A speed of <c>1.2</c> means that the map appears to move along the flight path by 1.2 times <c>options.curve</c> screenfuls every second.
    /// A screenful is the map's visible span and does not correspond to a fixed physical distance, but varies by zoom level.
    /// </remarks>
    [JsonPropertyName("speed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Speed { get; set; }

    #region Interfaces

    /// <inheritdoc/>
    [JsonPropertyName("animate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Animate { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("duration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Duration { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("easing")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Func<double, double>? Easing { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("essential")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Essential { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("freezeElevation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FreezeElevation { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("offset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PointLike? Offset { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("center")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LngLat? Center { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("zoom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Zoom { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("bearing")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Bearing { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("pitch")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Pitch { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("roll")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Roll { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("elevation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Elevation { get; set; }

    #endregion
}
