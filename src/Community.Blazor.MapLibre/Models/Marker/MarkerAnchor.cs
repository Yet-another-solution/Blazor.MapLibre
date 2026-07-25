using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Marker;

/// <summary>
/// Enumeration for marker anchor positions.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MarkerAnchor
{
    [JsonStringEnumMemberName("center")]
    Center,

    [JsonStringEnumMemberName("top")]
    Top,

    [JsonStringEnumMemberName("bottom")]
    Bottom,

    [JsonStringEnumMemberName("left")]
    Left,

    [JsonStringEnumMemberName("right")]
    Right,

    [JsonStringEnumMemberName("top-left")]
    TopLeft,

    [JsonStringEnumMemberName("top-right")]
    TopRight,

    [JsonStringEnumMemberName("bottom-left")]
    BottomLeft,

    [JsonStringEnumMemberName("bottom-right")]
    BottomRight
}