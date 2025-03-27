using System.Text.Json.Nodes;
using OneOf;
using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Converter;

namespace Community.Blazor.MapLibre.Models.Layers;

public class CircleLayer : Layer<CircleLayerLayout, CircleLayerPaint>
{
    /// <summary>
    ///  <inheritdoc/>
    /// </summary>
    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public override LayerType Type => LayerType.Circle;

    /// <summary>
    /// Gets or sets the name of the source to be used for this layer.
    /// </summary>
    [JsonPropertyName("source")]
    public required string Source { get; set; }

    [JsonPropertyName("circle-sort-key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? CircleSortKey { get; set; }

    [JsonPropertyName("visibility")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Visibility? Visibility { get; set; }
}

public class CircleLayerLayout;

public class CircleLayerPaint
{
    [JsonPropertyName("circle-radius")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? CircleRadius { get; set; }

    [JsonPropertyName("circle-color")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? CircleColor { get; set; }

    [JsonPropertyName("circle-blur")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? CircleBlur { get; set; }

    [JsonPropertyName("circle-opacity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? CircleOpacity { get; set; }

    [JsonPropertyName("circle-translate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double[]>))]
    public OneOf<double[], JsonArray>? CircleTranslate { get; set; }

    [JsonPropertyName("circle-translate-anchor")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<MapViewport>))]
    public OneOf<MapViewport, JsonArray>? CircleTranslateAnchor { get; set; }

    [JsonPropertyName("circle-pitch-scale")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<MapViewport>))]
    public OneOf<MapViewport, JsonArray>? CirclePitchScale { get; set; }

    [JsonPropertyName("circle-pitch-alignment")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<MapViewport>))]
    public OneOf<MapViewport, JsonArray>? CirclePitchAlignment { get; set; }

    [JsonPropertyName("circle-stroke-width")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? CircleStrokeWidth { get; set; }

    [JsonPropertyName("circle-stroke-color")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? CircleStrokeColor { get; set; }

    [JsonPropertyName("circle-stroke-opacity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? CircleStrokeOpacity { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MapViewport
{
    [JsonStringEnumMemberName("map")]
    Map,

    [JsonStringEnumMemberName("viewport")]
    Viewport
}
