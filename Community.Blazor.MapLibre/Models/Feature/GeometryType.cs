using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Feature;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GeometryType
{
    [JsonStringEnumMemberName("Point")]
    Point,
    [JsonStringEnumMemberName("MultiPoint")]
    MultiPoint,

    [JsonStringEnumMemberName("LineString")]
    Line,
    [JsonStringEnumMemberName("MultiLineString")]
    MultiLine,

    [JsonStringEnumMemberName("Polygon")]
    Polygon,
    [JsonStringEnumMemberName("MultiPolygon")]
    MultiPolygon
}
