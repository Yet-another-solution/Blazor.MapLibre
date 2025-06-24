using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Control;

/// <summary>
/// A position defintion for the control to be placed, can be in one of the corners of the map. When two or more controls are places in the same location they are stacked toward the center of the map.
/// </summary>
public enum ControlPosition
{
    [JsonStringEnumMemberName("top-left")]
    TopLeft,
    
    [JsonStringEnumMemberName("top-right")]
    TopRight,
    
    [JsonStringEnumMemberName("bottom-left")]
    BottomLeft,
    
    [JsonStringEnumMemberName("bottom-right")]
    BottomRight
}
