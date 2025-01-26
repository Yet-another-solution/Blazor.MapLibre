namespace Community.Blazor.MapLibre.Models.Camera;

public class CameraOptions : ICameraOptions
{
    public double? Pitch { get; set; }
    public double? Roll { get; set; }
    public double? Elevation { get; set; }
    public LngLat? Center { get; set; }
    public double? Zoom { get; set; }
    public double? Bearing { get; set; }
}