using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Converter;
using OneOf;

namespace Community.Blazor.MapLibre.Models.Layers;

public class HeatMapLayer : Layer<HeatMapLayerLayout, HeatMapLayerPaint>
{
    /// <summary>
    /// Gets or sets the name of the source to be used for this layer.
    /// </summary>
    [JsonPropertyName("source")]
    public required string Source { get; set; }

    /// <summary>
    /// Gets or sets the layer to use from a vector tile source.
    /// </summary>
    /// <remarks>
    /// Required for vector tile sources. Specifies the layer within the vector tiles to use for this layer.
    /// </remarks>
    [JsonPropertyName("source-layer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SourceLayer { get; set; }

    [JsonPropertyName("visibility")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Visibility? Visibility { get; set; }
}

public class HeatMapLayerLayout;

public class HeatMapLayerPaint
{
    /// <summary>
    /// Number in range [1, ∞). Units in pixels. Defaults to 30. Supports feature-state and interpolate
    /// expressions. Transitionable.
    /// Radius of influence of one heatmap point in pixels. Increasing the value makes the heatmap
    /// smoother, but less detailed.
    /// </summary>
    [JsonPropertyName("heatmap-radius")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? HeatmapRadius { get; set; }

    /// <summary>
    /// Number in range [0, ∞). Defaults to 1. Supports feature-state and interpolate expressions.
    /// A measure of how much an individual point contributes to the heatmap. A value of 10 would be
    /// equivalent to having 10 points of weight 1 in the same spot. Especially useful when combined
    /// with clustering.
    /// </summary>
    [JsonPropertyName("heatmap-weight")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? HeatmapWeight { get; set; }

    /// <summary>
    /// Number in range [0, ∞). Defaults to 1. Supports interpolate expressions. Transitionable.
    /// Similar to <c>heat-map-weight</c> but controls the intensity of the heatmap globally.
    /// Primarily used for adjusting the heatmap based on zoom level.
    /// </summary>
    [JsonPropertyName("heatmap-intensity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? HeatmapIntensity { get; set; }

    /// <summary>
    /// Number in range [0, 1]. Defaults to 1. Supports interpolate expressions. Transitionable.
    /// The global opacity at which the heatmap layer will be drawn.
    /// </summary>
    [JsonPropertyName("heatmap-opacity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? HeatmapOpacity { get; set; }

    /// <summary>
    /// Defines the color of each pixel based on its density value in a heatmap. Should be an expression
    /// that uses <c>["heatmap-density"]</c> as input (e.g. an <c>"interpolate"</c> expression).
    /// Defaults to a blue-to-red interpolation. Unlike other paint properties, this is always an
    /// expression and cannot be set to a flat value.
    /// </summary>
    [JsonPropertyName("heatmap-color")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JsonArray? HeatmapColor { get; set; }
}
