using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Converter;
using OneOf;

namespace Community.Blazor.MapLibre.Models.Layers;

public class FillLayer : Layer<FillLayerLayout, FillLayerPaint>
{
    /// <summary>
    ///  <inheritdoc/>
    /// </summary>
    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public override LayerType Type => LayerType.Fill;

    /// <summary>
    /// Gets or sets the name of the source to be used for this layer.
    /// </summary>
    [JsonPropertyName("source")]
    public required string Source { get; set; }
}

public class FillLayerLayout
{
    /// <summary>
    /// Determines the rendering order of features based on their sort key.
    /// </summary>
    /// <remarks>
    /// Features are sorted in ascending order based on this value. Features with a higher sort key will appear above features with a lower sort key.
    ///
    /// <list type="table">
    ///   <listheader>
    ///     <term>Sort Key</term>
    ///     <description>Rendering Order</description>
    ///   </listheader>
    ///   <item>
    ///     <term>Lower value</term>
    ///     <description>Feature is drawn first and appears beneath features with a higher sort key.</description>
    ///   </item>
    ///   <item>
    ///     <term>Higher value</term>
    ///     <description>Feature is drawn later and appears above features with a lower sort key.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [JsonPropertyName("fill-sort-key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? FillSortKey { get; set; }

    /// <summary>
    /// Controls whether this layer is displayed.
    /// </summary>
    /// <remarks>
    /// Optional enum. Possible values: <c>visible</c>, <c>none</c>. Defaults to <c>visible</c>.
    ///
    /// <list type="table">
    ///   <listheader>
    ///     <term>Value</term>
    ///     <description>Effect</description>
    ///   </listheader>
    ///   <item>
    ///     <term><c>visible</c></term>
    ///     <description>The layer is shown.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>none</c></term>
    ///     <description>The layer is not shown.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [JsonPropertyName("visibility")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? Visibility { get; set; }
}

public class FillLayerPaint
{
    /// <summary>
    /// Determines whether the fill should be antialiased.
    /// </summary>
    /// <remarks>
    /// Optional boolean. Defaults to <c>true</c>.
    ///
    /// <list type="table">
    ///   <listheader>
    ///     <term>Value</term>
    ///     <description>Effect</description>
    ///   </listheader>
    ///   <item>
    ///     <term><c>true</c></term>
    ///     <description>The fill is antialiased, resulting in smoother edges.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>false</c></term>
    ///     <description>The fill is not antialiased, which may result in jagged edges.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [JsonPropertyName("fill-antialias")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? FillAntialias { get; set; }

    /// <summary>
    /// Specifies the opacity of the entire fill layer.
    /// </summary>
    /// <remarks>
    /// Optional number in the range [0, 1]. Defaults to <c>1</c>. Supports <c>feature-state</c> and <c>interpolate</c> expressions. Transitionable.
    ///
    /// The opacity setting affects both the fill color and the 1px stroke around the fill if the stroke is used.
    ///
    /// <list type="table">
    ///   <listheader>
    ///     <term>Value</term>
    ///     <description>Effect</description>
    ///   </listheader>
    ///   <item>
    ///     <term><c>0</c></term>
    ///     <description>The fill layer is fully transparent.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>0.5</c></term>
    ///     <description>The fill layer is 50% transparent.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>1</c></term>
    ///     <description>The fill layer is fully opaque.</description>
    ///   </item>
    ///   <item>
    ///     <term>Uses <c>interpolate</c> expressions</term>
    ///     <description>Allows dynamic adjustments to the fill opacity.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [JsonPropertyName("fill-opacity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? FillOpacity { get; set; }

    /// <summary>
    /// Defines the color of the filled part of this layer.
    /// </summary>
    /// <remarks>
    /// Optional color. Defaults to <c>#000000</c>. Disabled by <c>fill-pattern</c>. Supports <c>feature-state</c> and <c>interpolate</c> expressions. Transitionable.
    ///
    /// This color can be specified as <c>rgba</c> with an alpha component, and the color's opacity will not affect the opacity of the 1px stroke if it is used.
    ///
    /// <list type="table">
    ///   <listheader>
    ///     <term>Value</term>
    ///     <description>Effect</description>
    ///   </listheader>
    ///   <item>
    ///     <term><c>#000000</c> (default)</term>
    ///     <description>The fill color is black.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>rgba(r, g, b, a)</c></term>
    ///     <description>Specifies a custom color with an optional alpha component for transparency.</description>
    ///   </item>
    ///   <item>
    ///     <term>Uses <c>interpolate</c> expressions</term>
    ///     <description>Allows dynamic adjustments to the fill color.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>fill-pattern</c> is set</term>
    ///     <description>The fill color is disabled in favor of a pattern.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [JsonPropertyName("fill-color")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? FillColor { get; set; }

    /// <summary>
    /// Specifies the outline color of the fill.
    /// </summary>
    /// <remarks>
    /// Optional color. Disabled by <c>fill-pattern</c>. Requires <c>fill-antialias</c> to be <c>true</c>. Supports <c>feature-state</c> and <c>interpolate</c> expressions. Transitionable.
    ///
    /// If unspecified, the outline color matches the value of <c>fill-color</c>.
    ///
    /// <list type="table">
    ///   <listheader>
    ///     <term>Condition</term>
    ///     <description>Effect</description>
    ///   </listheader>
    ///   <item>
    ///     <term>Value not set</term>
    ///     <description>Defaults to the value of <c>fill-color</c>.</description>
    ///   </item>
    ///   <item>
    ///     <term>Custom color set</term>
    ///     <description>Uses the specified color for the outline.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>fill-pattern</c> is set</term>
    ///     <description>The outline color is disabled.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>fill-antialias</c> is <c>false</c></term>
    ///     <description>The outline color is not applied.</description>
    ///   </item>
    ///   <item>
    ///     <term>Uses <c>interpolate</c> expressions</term>
    ///     <description>Allows dynamic adjustments to the outline color.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [JsonPropertyName("fill-outline-color")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? FillOutlineColor { get; set; }

    /// <summary>
    /// Specifies the geometry's offset in pixels.
    /// </summary>
    /// <remarks>
    /// Optional array. Units are in pixels. Defaults to <c>[0, 0]</c>. Supports <c>interpolate</c> expressions. Transitionable.
    ///
    /// Values are specified as <c>[x, y]</c>, where negative values indicate left and up, respectively.
    ///
    /// <list type="table">
    ///   <listheader>
    ///     <term>Value</term>
    ///     <description>Effect</description>
    ///   </listheader>
    ///   <item>
    ///     <term><c>[0, 0]</c> (default)</term>
    ///     <description>No offset is applied.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>[x, y]</c> with positive x</term>
    ///     <description>Moves the geometry to the right.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>[x, y]</c> with negative x</term>
    ///     <description>Moves the geometry to the left.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>[x, y]</c> with positive y</term>
    ///     <description>Moves the geometry downward.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>[x, y]</c> with negative y</term>
    ///     <description>Moves the geometry upward.</description>
    ///   </item>
    ///   <item>
    ///     <term>Uses <c>interpolate</c> expressions</term>
    ///     <description>Allows dynamic adjustments to the offset values.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [JsonPropertyName("fill-translate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double[]>))]
    public OneOf<double[], JsonArray>? FillTranslate { get; set; }

    /// <summary>
    /// Controls the frame of reference for <c>fill-translate</c>.
    /// </summary>
    /// <remarks>
    /// Optional enum. Possible values: <c>map</c>, <c>viewport</c>. Defaults to <c>map</c>. Requires <c>fill-translate</c>.
    ///
    /// <list type="table">
    ///   <listheader>
    ///     <term>Value</term>
    ///     <description>Effect</description>
    ///   </listheader>
    ///   <item>
    ///     <term><c>map</c> (default)</term>
    ///     <description>The fill is translated relative to the map.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>viewport</c></term>
    ///     <description>The fill is translated relative to the viewport.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [JsonPropertyName("fill-translate-anchor")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<MapViewport>))]
    public OneOf<MapViewport, JsonArray>? FillTranslateAnchor { get; set; }

    /// <summary>
    /// Specifies the name of the image in the sprite to use for drawing image fills.
    /// </summary>
    /// <remarks>
    /// Optional <c>resolvedImage</c>. Transitionable.
    ///
    /// For seamless patterns, the image width and height must be a power of two (e.g., 2, 4, 8, ..., 512).
    /// Note that zoom-dependent expressions will be evaluated only at integer zoom levels.
    ///
    /// <list type="table">
    ///   <listheader>
    ///     <term>Condition</term>
    ///     <description>Effect</description>
    ///   </listheader>
    ///   <item>
    ///     <term>Image width and height are a power of two</term>
    ///     <description>Ensures seamless pattern rendering.</description>
    ///   </item>
    ///   <item>
    ///     <term>Zoom-dependent expressions</term>
    ///     <description>Evaluated only at integer zoom levels.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [JsonPropertyName("fill-pattern")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<object>))]
    public OneOf<object, JsonArray>? FillPattern { get; set; }
}
