using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Marker;

/// <summary>
/// Enumeration for marker alignment options.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MarkerAlignment
{
    [EnumMember(Value = "auto")]
    Auto,
    
    [EnumMember(Value = "map")]
    Map,
    
    [EnumMember(Value = "viewport")]
    Viewport
}