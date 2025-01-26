namespace Community.Blazor.MapLibre.Models.Camera;

public class FlyToOptions : IAnimationOptions, ICameraOptions
{
    /// <summary>
    /// The zooming "curve" along the flight path. 
    /// A high value maximizes zooming for an exaggerated animation, 
    /// while a low value minimizes zooming for an effect closer to `EaseTo`.
    /// A value of `1.42` is the default, based on the average value selected in the study from
    /// [van Wijk (2003)](https://www.win.tue.nl/~vanwijk/zoompan.pdf).
    /// Defaults to a value of `1.42`.
    /// </summary>
    public double? Curve { get; set; }
    /// <summary>
    /// The zero-based zoom level at the peak of the flight path. 
    /// If <see cref="Curve"/> is specified, this value is ignored.
    /// </summary>
    public double? MinZoom { get; set; }
    /// <summary>
    /// The average speed of the animation relative to the defined <see cref="Curve"/>.
    /// A speed of `1.2` moves the map faster, completing 1.2 screenfuls every second.
    /// A screenful refers to the map's visible span, which varies by zoom level.
    /// Defaults to a value of `1.2`.
    /// </summary>
    public double? Speed { get; set; }
    /// <summary>
    /// The average speed of the animation in terms of screenfuls per second, 
    /// assuming a linear animation curve. Ignored if <see cref="Speed"/> is specified.
    /// </summary>
    public double? ScreenSpeed { get; set; }
    /// <summary>
    /// The maximum duration of the animation, in milliseconds. 
    /// If the calculated duration exceeds this value, it resets to zero.
    /// </summary>
    public double? MaxDuration { get; set; }
    /// <summary>
    /// The padding amount in pixels added to the given bounds. 
    /// Can be specified as a single value or a more complex <see cref="PaddingOptions"/> object.
    /// </summary>
    public object? Padding { get; set; }

    // Interface
    public double? Duration { get; set; }
    public Func<double, double>? Easing { get; set; }
    public PointLike? Offset { get; set; }
    public bool? Animate { get; set; }
    public bool? Essential { get; set; }
    public bool? FreezeElevation { get; set; }
    public LngLat? Center { get; set; }
    public double? Zoom { get; set; }
    public double? Bearing { get; set; }
    public double? Pitch { get; set; }
    public double? Roll { get; set; }
    public double? Elevation { get; set; }
}