using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Visibility
{
    [JsonStringEnumMemberName("visible")]
    Visible,

    [JsonStringEnumMemberName("none")]
    None
}
