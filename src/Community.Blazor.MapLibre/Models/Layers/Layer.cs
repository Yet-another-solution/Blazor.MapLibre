using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Converter;

namespace Community.Blazor.MapLibre.Models.Layers;

/// <summary>
/// Represents a Layer in the MapLibre map which defines rendering and customization of different map elements.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(BackgroundLayer), "background")]
[JsonDerivedType(typeof(CircleLayer), "circle")]
[JsonDerivedType(typeof(FillExtrusionLayer), "fill-extrusion")]
[JsonDerivedType(typeof(FillLayer), "fill")]
[JsonDerivedType(typeof(HeatMapLayer), "heatmap")]
[JsonDerivedType(typeof(HillShadeLayer), "hillshade")]
[JsonDerivedType(typeof(LineLayer), "line")]
[JsonDerivedType(typeof(RasterLayer), "raster")]
[JsonDerivedType(typeof(SymbolLayer), "symbol")]
public abstract class Layer
{
    /// <summary>
    /// Gets or sets the unique name of the layer. This is required.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonConverter(typeof(StringOrNumberConverter))]
    public required string Id { get; set; }

    /// <summary>
    /// The minimum zoom level for the layer. At zoom levels less than the minzoom, the layer will be hidden.
    /// </summary>
    [JsonPropertyName("minzoom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? MinZoom { get; set; }

    /// <summary>
    /// The maximum zoom level for the layer. At zoom levels equal to or greater than the maxzoom, the layer will be hidden.
    /// </summary>
    [JsonPropertyName("maxzoom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? MaxZoom { get; set; }

    [JsonPropertyName("filter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Filter { get; set; } // ["==", ["get", "color" ], "polygon"]
}

public abstract class Layer<TLayout, TPaint> : Layer
{
    /// <summary>
    /// Gets or sets the layout properties for the layer.
    /// Optional. Layout defines how the layer features are placed on the map.
    /// </summary>
    [JsonPropertyName("layout")]
    [StringSyntax(StringSyntaxAttribute.Json)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TLayout? Layout { get; set; }

    /// <summary>
    /// Gets or sets the paint properties for the layer.
    /// Optional. Paint defines the visual styling of the features.
    /// </summary>
    [JsonPropertyName("paint")]
    [StringSyntax(StringSyntaxAttribute.Json)]
    public TPaint? Paint { get; set; }
}
