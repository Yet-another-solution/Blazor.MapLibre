using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Converter;
using OneOf;

namespace Community.Blazor.MapLibre.Models.Layers;

public class LineLayer : Layer<LineLayerLayout, LineLayerPaint>
{
    /// <summary>
    ///  <inheritdoc/>
    /// </summary>
    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public override LayerType Type => LayerType.Line;

    /// <summary>
    /// Gets or sets the name of the source to be used for this layer.
    /// </summary>
    [JsonPropertyName("source")]
    public required string Source { get; set; }
}

public class LineLayerLayout
{
    /// <summary>
    /// The display of line endings.
    /// Possible values: butt, round, square. Defaults to butt.
    /// </summary>
    [JsonPropertyName("line-cap")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? LineCap { get; set; }

    /// <summary>
    /// The display of lines when joining.
    /// Possible values: bevel, round, miter. Defaults to miter.
    /// </summary>
    [JsonPropertyName("line-join")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? LineJoin { get; set; }

    /// <summary>
    /// Used to automatically convert miter joins to bevel joins for sharp angles.
    /// Defaults to 2.
    /// </summary>
    [JsonPropertyName("line-miter-limit")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? LineMiterLimit { get; set; }

    /// <summary>
    /// Used to automatically convert round joins to miter joins for shallow angles.
    /// Defaults to 1.05.
    /// </summary>
    [JsonPropertyName("line-round-limit")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? LineRoundLimit { get; set; }

    /// <summary>
    /// Sorts features in ascending order based on this value.
    /// </summary>
    [JsonPropertyName("line-sort-key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? LineSortKey { get; set; }

    /// <summary>
    /// Whether this layer is displayed.
    /// Possible values: visible, none. Defaults to visible.
    /// </summary>
    [JsonPropertyName("visibility")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? Visibility { get; set; }
}

public class LineLayerPaint
{
      /// <summary>
    /// The opacity at which the line will be drawn. Defaults to 1.
    /// Range: [0, 1].
    /// </summary>
    [JsonPropertyName("line-opacity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? LineOpacity { get; set; }

    /// <summary>
    /// The color with which the line will be drawn. Defaults to #000000.
    /// </summary>
    [JsonPropertyName("line-color")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? LineColor { get; set; }

    /// <summary>
    /// The geometry's offset. Values are [x, y] in pixels.
    /// </summary>
    [JsonPropertyName("line-translate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double[]>))]
    public OneOf<double[], JsonArray>? LineTranslate { get; set; }

    /// <summary>
    /// Controls the frame of reference for line-translate.
    /// Possible values: map, viewport. Defaults to map.
    /// </summary>
    [JsonPropertyName("line-translate-anchor")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? LineTranslateAnchor { get; set; }

    /// <summary>
    /// Stroke thickness. Defaults to 1.
    /// </summary>
    [JsonPropertyName("line-width")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? LineWidth { get; set; }

    /// <summary>
    /// Draws a line casing outside of a line's actual path. Defaults to 0.
    /// </summary>
    [JsonPropertyName("line-gap-width")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? LineGapWidth { get; set; }

    /// <summary>
    /// The line's offset. Defaults to 0.
    /// </summary>
    [JsonPropertyName("line-offset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? LineOffset { get; set; }

    /// <summary>
    /// Blur applied to the line, in pixels. Defaults to 0.
    /// </summary>
    [JsonPropertyName("line-blur")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? LineBlur { get; set; }

    /// <summary>
    /// Specifies the lengths of the alternating dashes and gaps that form the dash pattern.
    /// </summary>
    [JsonPropertyName("line-dasharray")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double[]>))]
    public OneOf<double[], JsonArray>? LineDasharray { get; set; }

    /// <summary>
    /// Name of image in sprite to use for drawing image lines.
    /// </summary>
    [JsonPropertyName("line-pattern")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? LinePattern { get; set; }

    /// <summary>
    /// Defines a gradient with which to color a line feature.
    /// </summary>
    [JsonPropertyName("line-gradient")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? LineGradient { get; set; }
}
