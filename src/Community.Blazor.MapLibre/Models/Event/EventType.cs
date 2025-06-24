using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Event;

public enum EventType
{
    [JsonStringEnumMemberName("click")]
    Click,

    [JsonStringEnumMemberName("contextmenu")]
    ContextMenu,

    [JsonStringEnumMemberName("dblclick")]
    DblClick,

    [JsonStringEnumMemberName("mousedown")]
    MouseDown,

    [JsonStringEnumMemberName("mouseenter")]
    MouseEnter,

    [JsonStringEnumMemberName("mouseleave")]
    MouseLeave,

    [JsonStringEnumMemberName("mousemove")]
    MouseMove,

    [JsonStringEnumMemberName("mouseout")]
    MouseOut,

    [JsonStringEnumMemberName("mouseover")]
    MouseOver,

    [JsonStringEnumMemberName("mouseup")]
    MouseUp
}
