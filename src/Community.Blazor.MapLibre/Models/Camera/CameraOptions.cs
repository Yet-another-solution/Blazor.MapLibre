using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Camera;

/// <summary>
/// Options controlling the desired location, zoom, bearing, pitch, roll, and elevation of the camera.
/// Used with <c>JumpTo</c>, <c>EaseTo</c>, and <c>FlyTo</c>. All properties are optional; 
/// if omitted, the current camera values remain unchanged.
/// </summary>
public class CameraOptions : ICameraOptions
{

    // <inheritdoc />
    [JsonPropertyName("center")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LngLat? Center { get; set; }

    // <inheritdoc />
    [JsonPropertyName("zoom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Zoom { get; set; }

    // <inheritdoc />
    [JsonPropertyName("bearing")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Bearing { get; set; }

    // <inheritdoc />
    [JsonPropertyName("elevation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Elevation { get; set; }
    
    // <inheritdoc />
    [JsonPropertyName("pitch")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Pitch { get; set; }
    
    // <inheritdoc />
    [JsonPropertyName("roll")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Roll { get; set; }
}
