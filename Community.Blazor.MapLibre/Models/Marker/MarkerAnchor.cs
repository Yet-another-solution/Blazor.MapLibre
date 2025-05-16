using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Marker;

/// <summary>
/// Enumeration for marker anchor positions.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MarkerAnchor
{
    [EnumMember(Value = "center")]
    Center,
    
    [EnumMember(Value = "top")]
    Top,
    
    [EnumMember(Value = "bottom")]
    Bottom,
    
    [EnumMember(Value = "left")]
    Left,
    
    [EnumMember(Value = "right")]
    Right,
    
    [EnumMember(Value = "top-left")]
    TopLeft,
    
    [EnumMember(Value = "top-right")]
    TopRight,
    
    [EnumMember(Value = "bottom-left")]
    BottomLeft,
    
    [EnumMember(Value = "bottom-right")]
    BottomRight
}