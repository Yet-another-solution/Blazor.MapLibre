namespace Community.Blazor.MapLibre.Models.Camera;

/// <summary>
/// Holds center, zoom and bearing properties
/// </summary>
public interface ICenterZoomBearing
{
    /// <summary>
    /// The desired center.
    /// </summary>
    public LngLat? Center { get; set; }

    /// <summary>
    /// The desired mercator zoom level.
    /// </summary>
    public double? Zoom { get; set; }

    /// <summary>
    /// The desired bearing in degrees. The bearing is the compass direction that
    ///  is "up". For example, `bearing: 90` orients the map so that east is up.
    /// </summary>
    public double? Bearing { get; set; }
}