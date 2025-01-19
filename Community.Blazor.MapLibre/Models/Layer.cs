using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models;

/// <summary>
/// Represents a Layer in the MapLibre map which defines rendering and customization of different map elements.
/// </summary>
public class Layer
{
    /// <summary>
    /// Gets or sets the unique name of the layer. This is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; } 

    /// <summary>
    /// Gets or sets the rendering type of the layer. This is required.
    /// Possible values: "fill", "line", "symbol", "circle", "heatmap", "fill-extrusion", "raster", "hillshade", "background".
    /// </summary>
    [JsonPropertyName("type")]
    public required string Type { get; set; }

    /// <summary>
    /// Gets or sets the name of the source to be used for this layer.
    /// Optional. Required for all layer types except "background".
    /// </summary>
    [JsonPropertyName("source")]
    public string? Source { get; set; }

    /// <summary>
    /// Gets or sets the layout properties for the layer.
    /// Optional. Layout defines how the layer features are placed on the map.
    /// </summary>
    [JsonPropertyName("layout")]
    public object Layout { get; set; } = new { };

    /// <summary>
    /// Gets or sets the paint properties for the layer.
    /// Optional. Paint defines the visual styling of the features.
    /// </summary>
    [JsonPropertyName("paint")]
    public object Paint { get; set; } = new { };
}