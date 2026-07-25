using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Marker;

/// <summary>
/// Enumeration for marker alignment options.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MarkerAlignment
{
    [JsonStringEnumMemberName("auto")]
    Auto,

    [JsonStringEnumMemberName("map")]
    Map,

    [JsonStringEnumMemberName("viewport")]
    Viewport
}