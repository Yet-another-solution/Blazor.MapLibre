using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Event;

/// <summary>
/// Map-level events that don't carry the richer <see cref="MapMouseEvent"/> payload (mouse position,
/// clicked features, etc.) - just the event type itself.
/// </summary>
public enum MapEventType
{
    [JsonStringEnumMemberName("rotate")]
    Rotate,

    [JsonStringEnumMemberName("pitch")]
    Pitch
}
