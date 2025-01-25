namespace Community.Blazor.MapLibre.Models.Camera;

public class CenterZoomBearing : ICenterZoomBearing
{
    public LngLat? Center { get; set; }
    public double? Zoom { get; set; }
    public double? Bearing { get; set; }
}