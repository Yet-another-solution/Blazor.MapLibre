using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models;

public enum PositionAnchor
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
