using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Converter;
using OneOf;

namespace Community.Blazor.MapLibre.Models.Layers;

public class SymbolLayer : Layer<SymbolLayerLayout, SymbolLayerPaint>
{
    /// <summary>
    ///  <inheritdoc/>
    /// </summary>
    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public override LayerType Type => LayerType.Symbol;

    /// <summary>
    /// Gets or sets the name of the source to be used for this layer.
    /// </summary>
    [JsonPropertyName("source")]
    public required string Source { get; set; }
}

public class SymbolLayerLayout
{
    /// <summary>
    /// Gets or sets the label placement relative to its geometry.
    /// </summary>
    /// <remarks>
    /// Optional enum. Possible values: "point", "line", "line-center". Defaults to "point".
    ///
    /// <list type="table">
    ///   <item>
    ///     <term>point</term>
    ///     <description>The label is placed at the point where the geometry is located.</description>
    ///   </item>
    ///   <item>
    ///     <term>line</term>
    ///     <description>The label is placed along the line of the geometry. Can only be used on LineString and Polygon geometries.</description>
    ///   </item>
    ///   <item>
    ///     <term>line-center</term>
    ///     <description>The label is placed at the center of the line of the geometry. Can only be used on LineString and Polygon geometries.
    ///     Note that a single feature in a vector tile may contain multiple line geometries.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [JsonPropertyName("symbol-placement")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SymbolPlacement { get; set; }

    /// <summary>
    /// Defines the distance between two symbol anchors.
    /// </summary>
    /// <remarks>
    /// Optional number in the range [1, ∞). Units are in pixels. Defaults to <c>250</c>.
    /// Requires <c>symbol-placement</c> to be <c>line</c>. Supports <c>interpolate</c> expressions.
    ///
    /// <list type="table">
    ///   <listheader>
    ///     <term>Condition</term>
    ///     <description>Effect</description>
    ///   </listheader>
    ///   <item>
    ///     <term>Value in range [1, ∞)</term>
    ///     <description>Specifies the spacing in pixels between symbol anchors.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>symbol-placement</c> is not <c>line</c></term>
    ///     <description>The property has no effect.</description>
    ///   </item>
    ///   <item>
    ///     <term>Uses <c>interpolate</c> expressions</term>
    ///     <description>Allows dynamic adjustments to symbol anchor distances.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [JsonPropertyName("symbol-spacing")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? SymbolSpacing { get; set; }

    /// <summary>
    /// Determines whether symbols should avoid crossing tile edges to prevent mutual collisions.
    /// </summary>
    /// <remarks>
    /// If set to <c>true</c>, symbols will not cross tile edges, reducing the likelihood of mutual collisions.
    /// This is recommended for layers that lack sufficient padding in the vector tile to prevent collisions,
    /// or for point symbol layers placed after a line symbol layer.
    ///
    /// <list type="table">
    ///   <listheader>
    ///     <term>Condition</term>
    ///     <description>Effect</description>
    ///   </listheader>
    ///   <item>
    ///     <term>Insufficient padding in the vector tile</term>
    ///     <description>Prevents symbols from colliding at tile edges.</description>
    ///   </item>
    ///   <item>
    ///     <term>Point symbol layer placed after a line symbol layer</term>
    ///     <description>Ensures better label placement and visibility.</description>
    ///   </item>
    ///   <item>
    ///     <term>Using a client with global collision detection (e.g., MapLibre GL JS 0.42.0 or greater)</term>
    ///     <description>Enabling this property is unnecessary to prevent clipped labels at tile boundaries.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [JsonPropertyName("symbol-avoid-edges")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<bool>))]
    public OneOf<bool, JsonArray>? SymbolAvoidEdges { get; set; }

    /// <summary>
    /// Specifies the sorting order of features based on their assigned value.
    /// </summary>
    /// <remarks>
    /// Features are sorted in ascending order based on this value. Features with lower sort keys are drawn and placed first.
    ///
    /// <list type="table">
    ///   <listheader>
    ///     <term>Condition</term>
    ///     <description>Effect</description>
    ///   </listheader>
    ///   <item>
    ///     <term><c>icon-allow-overlap</c> or <c>text-allow-overlap</c> is <c>false</c></term>
    ///     <description>Features with a lower sort key have priority during placement.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>icon-allow-overlap</c> or <c>text-allow-overlap</c> is <c>true</c></term>
    ///     <description>Features with a higher sort key will overlap features with a lower sort key.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [JsonPropertyName("symbol-sort-key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? SymbolSortKey { get; set; }

    /// <summary>
    /// Gets or sets the method for determining the rendering order of overlapping symbols within the same layer.
    /// </summary>
    /// <remarks>
    /// Optional enum. Possible values: <c>auto</c>, <c>viewport-y</c>, <c>source</c>. Defaults to <c>auto</c>.
    ///
    /// Determines whether overlapping symbols in the same layer are rendered in the order that they appear in the data source
    /// or by their y-position relative to the viewport. To control the order and prioritization of symbols otherwise, use <c>symbol-sort-key</c>.
    ///
    /// <list type="table">
    ///   <listheader>
    ///     <term>Value</term>
    ///     <description>Description</description>
    ///   </listheader>
    ///   <item>
    ///     <term>auto</term>
    ///     <description>Sorts symbols by <c>symbol-sort-key</c> if set. Otherwise, sorts symbols by their y-position relative
    ///     to the viewport if <c>icon-allow-overlap</c> or <c>text-allow-overlap</c> is set to <c>true</c> or
    ///     <c>icon-ignore-placement</c> or <c>text-ignore-placement</c> is <c>false</c>.</description>
    ///   </item>
    ///   <item>
    ///     <term>viewport-y</term>
    ///     <description>Sorts symbols by their y-position relative to the viewport if <c>icon-allow-overlap</c> or
    ///     <c>text-allow-overlap</c> is set to <c>true</c> or <c>icon-ignore-placement</c> or <c>text-ignore-placement</c> is <c>false</c>.</description>
    ///   </item>
    ///   <item>
    ///     <term>source</term>
    ///     <description>Sorts symbols by <c>symbol-sort-key</c> if set. Otherwise, no sorting is applied; symbols are rendered
    ///     in the same order as the source data.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [JsonPropertyName("symbol-z-order")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? SymbolZOrder { get; set; }

    /// <summary>
    /// Controls whether the icon remains visible when overlapping other symbols.
    /// </summary>
    /// <remarks>
    /// Optional boolean. Defaults to <c>false</c>. Requires <c>icon-image</c>. Disabled by <c>icon-overlap</c>.
    ///
    /// <list type="table">
    ///   <listheader>
    ///     <term>Value</term>
    ///     <description>Description</description>
    ///   </listheader>
    ///   <item>
    ///     <term><c>true</c></term>
    ///     <description>The icon will be visible even if it collides with other previously drawn symbols.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>false</c></term>
    ///     <description>The icon will be hidden if it collides with other previously drawn symbols.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [JsonPropertyName("icon-allow-overlap")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IconAllowOverlap { get; set; }

    /// <summary>
    /// Gets or sets the behavior for displaying an icon when it overlaps other symbols on the map.
    /// </summary>
    /// <remarks>
    /// If <c>icon-overlap</c> is not set, <c>icon-allow-overlap</c> is used instead.
    ///
    /// Possible values:
    /// <list type="table">
    ///   <item>
    ///     <term>never</term>
    ///     <description>The icon will be hidden if it collides with any other previously drawn symbol.</description>
    ///   </item>
    ///   <item>
    ///     <term>always</term>
    ///     <description>The icon will be visible even if it collides with any other previously drawn symbol.</description>
    ///   </item>
    ///   <item>
    ///     <term>cooperative</term>
    ///     <description>If the icon collides with another previously drawn symbol, the overlap mode for that symbol is checked.
    ///     If the previous symbol was placed using <c>never</c> overlap mode, the new icon is hidden.
    ///     If the previous symbol was placed using <c>always</c> or <c>cooperative</c> overlap mode, the new icon is visible.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [JsonPropertyName("icon-overlap")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? IconOverlap { get; set; }

    /// <summary>
    /// Determines whether other symbols remain visible even if they collide with the icon.
    /// </summary>
    /// <remarks>
    /// Optional boolean. Defaults to <c>false</c>. Requires <c>icon-image</c>.
    ///
    /// <list type="table">
    ///   <listheader>
    ///     <term>Value</term>
    ///     <description>Effect</description>
    ///   </listheader>
    ///   <item>
    ///     <term><c>true</c></term>
    ///     <description>Other symbols can be visible even if they collide with the icon.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>false</c></term>
    ///     <description>Other symbols will be hidden if they collide with the icon.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [JsonPropertyName("icon-ignore-placement")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<bool>))]
    public OneOf<bool, JsonArray>? IconIgnorePlacement { get; set; }

    /// <summary>
    /// Controls whether text can be displayed independently of its corresponding icon when collisions occur.
    /// </summary>
    /// <remarks>
    /// Optional boolean. Defaults to <c>false</c>. Requires <c>icon-image</c> and <c>text-field</c>.
    ///
    /// <list type="table">
    ///   <listheader>
    ///     <term>Value</term>
    ///     <description>Effect</description>
    ///   </listheader>
    ///   <item>
    ///     <term><c>true</c></term>
    ///     <description>Text will be displayed even if its corresponding icon collides with other symbols, as long as the text itself does not collide.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>false</c></term>
    ///     <description>Text and icon are treated as a single unit; if the icon collides, the text will not be displayed.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [JsonPropertyName("icon-optional")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<bool>))]
    public OneOf<bool, JsonArray>? IconOptional { get; set; }

    /// <summary>
    /// Determines the rotation behavior of icons in combination with <c>symbol-placement</c>.
    /// </summary>
    /// <remarks>
    /// Optional enum. Possible values: <c>map</c>, <c>viewport</c>, <c>auto</c>. Defaults to <c>auto</c>. Requires <c>icon-image</c>.
    ///
    /// <list type="table">
    ///   <listheader>
    ///     <term>Value</term>
    ///     <description>Effect</description>
    ///   </listheader>
    ///   <item>
    ///     <term><c>map</c></term>
    ///     <description>When <c>symbol-placement</c> is <c>point</c>, aligns icons east-west.
    ///     When <c>symbol-placement</c> is <c>line</c> or <c>line-center</c>, aligns icon x-axes with the line.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>viewport</c></term>
    ///     <description>Produces icons whose x-axes are aligned with the x-axis of the viewport, regardless of <c>symbol-placement</c>.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>auto</c></term>
    ///     <description>When <c>symbol-placement</c> is <c>point</c>, behaves like <c>viewport</c>.
    ///     When <c>symbol-placement</c> is <c>line</c> or <c>line-center</c>, behaves like <c>map</c>.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [JsonPropertyName("icon-rotation-alignment")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? IconRotationAlignment { get; set; }

    /// <summary>
    /// Scales the original size of the icon by the specified factor.
    /// </summary>
    /// <remarks>
    /// Optional number in the range [0, ∞). Units are a factor of the original icon size. Defaults to <c>1</c>.
    /// Requires <c>icon-image</c>. Supports <c>interpolate</c> expressions.
    ///
    /// <list type="table">
    ///   <listheader>
    ///     <term>Value</term>
    ///     <description>Effect</description>
    ///   </listheader>
    ///   <item>
    ///     <term><c>1</c></term>
    ///     <description>Keeps the icon at its original size.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>&gt;1</c></term>
    ///     <description>Increases the icon size by the specified factor (e.g., <c>3</c> triples the icon's size).</description>
    ///   </item>
    ///   <item>
    ///     <term><c>&lt;1</c></term>
    ///     <description>Reduces the icon size by the specified factor.</description>
    ///   </item>
    ///   <item>
    ///     <term>Uses <c>interpolate</c> expressions</term>
    ///     <description>Allows dynamic scaling of the icon size based on conditions.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [JsonPropertyName("icon-size")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? IconSize { get; set; }

    /// <summary>
    /// Determines how the icon is scaled to fit around the associated text.
    /// </summary>
    /// <remarks>
    /// Optional enum. Possible values: <c>none</c>, <c>width</c>, <c>height</c>, <c>both</c>. Defaults to <c>none</c>.
    /// Requires <c>icon-image</c> and <c>text-field</c>.
    ///
    /// <list type="table">
    ///   <listheader>
    ///     <term>Value</term>
    ///     <description>Effect</description>
    ///   </listheader>
    ///   <item>
    ///     <term><c>none</c></term>
    ///     <description>The icon is displayed at its intrinsic aspect ratio.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>width</c></term>
    ///     <description>The icon is scaled in the x-dimension to fit the width of the text.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>height</c></term>
    ///     <description>The icon is scaled in the y-dimension to fit the height of the text.</description>
    ///   </item>
    ///   <item>
    ///     <term><c>both</c></term>
    ///     <description>The icon is scaled in both x- and y-dimensions to fit the text.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [JsonPropertyName("icon-text-fit")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? IconTextFit { get; set; }

    [JsonPropertyName("icon-text-fit-padding")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double[]>))]
    public OneOf<double[], JsonArray>? IconTextFitPadding { get; set; }

    [JsonPropertyName("icon-image")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? IconImage { get; set; }

    [JsonPropertyName("icon-rotate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? IconRotate { get; set; }

    [JsonPropertyName("icon-padding")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? IconPadding { get; set; }

    [JsonPropertyName("icon-keep-upright")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<bool>))]
    public OneOf<bool, JsonArray>? IconKeepUpright { get; set; }

    [JsonPropertyName("icon-offset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double[]>))]
    public OneOf<double[], JsonArray>? IconOffset { get; set; }

    [JsonPropertyName("icon-anchor")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? IconAnchor { get; set; }

    [JsonPropertyName("icon-pitch-alignment")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? IconPitchAlignment { get; set; }

    [JsonPropertyName("text-pitch-alignment")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? TextPitchAlignment { get; set; }

    [JsonPropertyName("text-rotation-alignment")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? TextRotationAlignment { get; set; }

    [JsonPropertyName("text-field")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? TextField { get; set; }

    [JsonPropertyName("text-font")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string[]>))]
    public OneOf<string[], JsonArray>? TextFont { get; set; }

    [JsonPropertyName("text-size")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? TextSize { get; set; }

    [JsonPropertyName("text-max-width")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? TextMaxWidth { get; set; }

    [JsonPropertyName("text-line-height")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? TextLineHeight { get; set; }

    [JsonPropertyName("text-letter-spacing")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? TextLetterSpacing { get; set; }

    [JsonPropertyName("text-justify")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? TextJustify { get; set; }

    [JsonPropertyName("text-radial-offset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? TextRadialOffset { get; set; }

    [JsonPropertyName("text-variable-anchor")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string[]>))]
    public OneOf<string[], JsonArray>? TextVariableAnchor { get; set; }

    [JsonPropertyName("text-variable-anchor-offset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double[]>))]
    public OneOf<double[], JsonArray>? TextVariableAnchorOffset { get; set; }

    [JsonPropertyName("text-anchor")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? TextAnchor { get; set; }

    [JsonPropertyName("text-max-angle")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? TextMaxAngle { get; set; }

    [JsonPropertyName("text-writing-mode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string[]>))]
    public OneOf<string[], JsonArray>? TextWritingMode { get; set; }

    [JsonPropertyName("text-rotate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? TextRotate { get; set; }

    [JsonPropertyName("text-padding")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? TextPadding { get; set; }

    [JsonPropertyName("text-keep-upright")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<bool>))]
    public OneOf<bool, JsonArray>? TextKeepUpright { get; set; }

    [JsonPropertyName("text-transform")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? TextTransform { get; set; }

    [JsonPropertyName("text-offset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double[]>))]
    public OneOf<double[], JsonArray>? TextOffset { get; set; }

    [JsonPropertyName("text-allow-overlap")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<bool>))]
    public OneOf<bool, JsonArray>? TextAllowOverlap { get; set; }

    [JsonPropertyName("text-overlap")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? TextOverlap { get; set; }

    [JsonPropertyName("text-ignore-placement")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<bool>))]
    public OneOf<bool, JsonArray>? TextIgnorePlacement { get; set; }

    [JsonPropertyName("text-optional")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<bool>))]
    public OneOf<bool, JsonArray>? TextOptional { get; set; }

    [JsonPropertyName("visibility")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? Visibility { get; set; }
}

public class SymbolLayerPaint
{
    [JsonPropertyName("icon-opacity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? IconOpacity { get; set; }

    [JsonPropertyName("icon-color")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? IconColor { get; set; }

    [JsonPropertyName("icon-halo-color")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? IconHaloColor { get; set; }

    [JsonPropertyName("icon-halo-width")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? IconHaloWidth { get; set; }

    [JsonPropertyName("icon-halo-blur")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? IconHaloBlur { get; set; }

    [JsonPropertyName("icon-translate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double[]>))]
    public OneOf<double[], JsonArray>? IconTranslate { get; set; }

    [JsonPropertyName("icon-translate-anchor")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? IconTranslateAnchor { get; set; }

    [JsonPropertyName("text-opacity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? TextOpacity { get; set; }

    [JsonPropertyName("text-color")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? TextColor { get; set; }

    [JsonPropertyName("text-halo-color")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? TextHaloColor { get; set; }

    [JsonPropertyName("text-halo-width")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? TextHaloWidth { get; set; }

    [JsonPropertyName("text-halo-blur")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double>))]
    public OneOf<double, JsonArray>? TextHaloBlur { get; set; }

    [JsonPropertyName("text-translate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<double[]>))]
    public OneOf<double[], JsonArray>? TextTranslate { get; set; }

    [JsonPropertyName("text-translate-anchor")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<MapViewport>))]
    public OneOf<MapViewport, JsonArray>? TextTranslateAnchor { get; set; }
}
