namespace Community.Blazor.MapLibre.Models.Camera;

public class EaseToOptions : IAnimationOptions, ICameraOptions
{
    public double? DelayEndEvents { get; set; }
    public object? Padding { get; set; }
    /// <summary>
    /// If `zoom` is specified, `around` determines the point around which the zoom is centered.
    /// </summary>
    public LngLat? Around { get; set; }
    public string? EaseId { get; set; }
    public bool? NoMoveStart { get; set; }
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