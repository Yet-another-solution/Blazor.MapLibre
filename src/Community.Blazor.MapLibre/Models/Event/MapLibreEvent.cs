using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Event;

/// <summary>
/// A generic MapLibre GL JS map event payload, for events that don't need more than their type
/// (e.g. "rotate", "pitch"). See <see cref="MapMouseEvent"/> for events that carry mouse/feature data.
/// </summary>
public class MapLibreEvent
{
    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required MapEventType Type { get; set; }
}
