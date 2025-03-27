using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models;

public enum Visibility
{
    [JsonStringEnumMemberName("visible")]
    Visible,

    [JsonStringEnumMemberName("none")]
    None
}
