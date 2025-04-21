using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Camera;

public class CameraOptions : ICameraOptions
{
    [JsonPropertyName("pitch")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Pitch { get; set; }
    [JsonPropertyName("roll")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Roll { get; set; }
    [JsonPropertyName("elevation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Elevation { get; set; }
    [JsonPropertyName("center")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LngLat? Center { get; set; }
    [JsonPropertyName("zoom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Zoom { get; set; }
    [JsonPropertyName("bearing")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Bearing { get; set; }
}