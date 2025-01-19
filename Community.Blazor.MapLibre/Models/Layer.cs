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
    /// Gets or sets the arbitrary metadata properties useful for tracking additional information.
    /// Optional. Should be prefixed to avoid naming collisions.
    /// </summary>
    [JsonPropertyName("metadata")]
    public object? Metadata { get; set; }

    /// <summary>
    /// Gets or sets the name of the source to be used for this layer.
    /// Optional. Required for all layer types except "background".
    /// </summary>
    [JsonPropertyName("source")]
    public string? Source { get; set; }

    /// <summary>
    /// Gets or sets the specific layer within a vector tile source to be used.
    /// Optional. Required for vector tile sources; prohibited for all other source types.
    /// </summary>
    [JsonPropertyName("source-layer")]
    public string? SourceLayer { get; set; }

    /// <summary>
    /// Gets or sets the minimum zoom level for displaying this layer.
    /// Optional. At zoom levels less than minzoom, the layer will be hidden.
    /// </summary>
    [JsonPropertyName("minzoom")]
    public double? MinZoom { get; set; }

    /// <summary>
    /// Gets or sets the maximum zoom level for displaying this layer.
    /// Optional. At zoom levels equal to or greater than maxzoom, the layer will be hidden.
    /// </summary>
    [JsonPropertyName("maxzoom")]
    public double? MaxZoom { get; set; }

    /// <summary>
    /// Gets or sets the filter expression specifying conditions on source features.
    /// Optional. Only features that match the filter are displayed.
    /// </summary>
    [JsonPropertyName("filter")]
    public object? Filter { get; set; }

    /// <summary>
    /// Gets or sets the layout properties for the layer.
    /// Optional. Layout defines how the layer features are placed on the map.
    /// </summary>
    [JsonPropertyName("layout")]
    public object? Layout { get; set; }

    /// <summary>
    /// Gets or sets the paint properties for the layer.
    /// Optional. Paint defines the visual styling of the features.
    /// </summary>
    [JsonPropertyName("paint")]
    public object? Paint { get; set; }
}