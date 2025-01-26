namespace Community.Blazor.MapLibre.Models.Camera;

public class FitBoundOptions : FlyToOptions
{
    /// <summary>
    /// If `true`, the map transitions using {@link Map#easeTo}. If `false`, the map transitions using {@link Map#flyTo}.
    /// See those functions and {@link AnimationOptions} for information about options available.
    /// @defaultValue false
    /// </summary>
    public bool Linear { get; set; } = false;
    /// <summary>
    /// The center of the given bounds relative to the map's center, measured in pixels.
    /// </summary>
    public new PointLike? Offset { get; set; }
    /// <summary>
    /// The maximum zoom level to allow when the map view transitions to the specified bounds.
    /// </summary>
    public double? MaxZoom { get; set; }
}