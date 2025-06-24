using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Camera;

/// <summary>
/// Options common to {@link Map#jumpTo}, {@link Map#easeTo}, and {@link Map#flyTo}, controlling the desired location,
/// zoom, bearing, pitch, and roll of the camera. All properties are optional, and when a property is omitted, the current
/// camera value for that property will remain unchanged.
/// </summary>
public interface ICameraOptions : ICenterZoomBearing
{
    /// <summary>
    /// The desired pitch in degrees. The pitch is the angle towards the horizon
    /// measured in degrees with a range between 0 and 60 degrees. For example, pitch: 0 provides the appearance
    /// of looking straight down at the map, while pitch: 60 tilts the user's perspective towards the horizon.
    /// Increasing the pitch value is often used to display 3D objects.
    /// </summary>
    [JsonPropertyName("pitch")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Pitch { get; set; }

    /// <summary>
    /// The desired roll in degrees. The roll is the angle about the camera boresight.
    /// </summary>
    [JsonPropertyName("roll")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Roll { get; set; }

    /// <summary>
    /// The elevation of the center point in meters above sea level.
    /// </summary>
    [JsonPropertyName("elevation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Elevation { get; set; }
}