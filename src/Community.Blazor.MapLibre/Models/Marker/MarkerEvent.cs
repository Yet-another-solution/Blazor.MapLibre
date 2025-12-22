namespace Community.Blazor.MapLibre.Models.Marker;

using System.Text.Json.Serialization;

/// <summary>
/// Represents an event that occurred on a marker.
/// </summary>
public class MarkerEvent
{
    /// <summary>
    /// The type of event (e.g., "click", "dragstart", "drag", "dragend").
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// The geographical position of the marker when the event occurred.
    /// </summary>
    [JsonPropertyName("lngLat")]
    public LngLat LngLat { get; set; } = new();
}