using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Layers;

public enum LayerType
{
    [JsonStringEnumMemberName("fill")]
    Fill,

    [JsonStringEnumMemberName("line")]
    Line,

    [JsonStringEnumMemberName("symbol")]
    Symbol,

    [JsonStringEnumMemberName("circle")]
    Circle,

    [JsonStringEnumMemberName("heatmap")]
    Heatmap,

    [JsonStringEnumMemberName("fill-extrusion")]
    FillExtrusion,

    [JsonStringEnumMemberName("raster")]
    Raster,

    [JsonStringEnumMemberName("hillshade")]
    Hillshade,

    [JsonStringEnumMemberName("background")]
    Background
}
