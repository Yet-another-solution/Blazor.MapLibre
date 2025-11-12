using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Converter;
using Community.Blazor.MapLibre.Models.Feature;
using OneOf;

namespace Community.Blazor.MapLibre.Models.Sources;

/// <summary>
/// Represents a GeoJSON source. GeoJSON sources provide either inline GeoJSON data or a URL to a GeoJSON file.
/// They can support clustering and other custom behaviors for point features.
/// </summary>
public class GeoJsonSource : ISource
{
    /// <inheritdoc />
    [JsonPropertyName("type")]
    public string Type => "geojson";

    /// <summary>
    /// The GeoJSON data, either as an inline GeoJSON object or a URL string to an external GeoJSON file. Required.
    /// Use <see cref="IFeature"/> for inline GeoJSON data or <see cref="string"/> for a URL.
    /// </summary>
    [JsonPropertyName("data")]
    [JsonConverter(typeof(GeoJsonDataConverter))]
    public required OneOf<IFeature, string> Data { get; set; }

    /// <summary>
    /// If true, the GeoJSON data will be clustered into groups when zoomed out. Default is false. Optional.
    /// </summary>
    [JsonPropertyName("cluster")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Cluster { get; set; }

    /// <summary>
    /// Maximum zoom level at which clusters are generated. Default is one zoom level less than maxzoom (so that last zoom features are not clustered). Optional.
    /// </summary>
    [JsonPropertyName("clusterMaxZoom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? ClusterMaxZoom { get; set; }

    /// <summary>
    /// Radius of each cluster when clustering points. Default is 50. Optional.
    /// </summary>
    [JsonPropertyName("clusterRadius")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? ClusterRadius { get; set; }

    /// <summary>
    /// Minimum number of points necessary to form a cluster. Default is 2. Optional.
    /// </summary>
    [JsonPropertyName("clusterMinPoints")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ClusterMinPoints { get; set; }

    /// <summary>
    /// An object defining custom properties on the generated clusters. Optional.
    /// </summary>
    [JsonPropertyName("clusterProperties")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object>? ClusterProperties { get; set; }

    /// <summary>
    /// Whether to generate ids for the GeoJSON features. When enabled, the feature.id property will be auto assigned based on its index in the features array. Default is false. Optional.
    /// </summary>
    [JsonPropertyName("generateId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? GenerateId { get; set; }

    /// <summary>
    /// Size of the tile buffer on each side. A value of 0 produces no buffer. A value of 512 produces a buffer as wide as the tile itself. Default is 128. Optional.
    /// </summary>
    [JsonPropertyName("buffer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Buffer { get; set; }

    /// <summary>
    /// Douglas-Peucker simplification tolerance (higher means simpler geometries and faster performance). Default is 0.375. Optional.
    /// </summary>
    [JsonPropertyName("tolerance")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? Tolerance { get; set; }

    /// <summary>
    /// Whether to calculate line distance metrics. This is required for line layers that specify line-gradient values. Default is false. Optional.
    /// </summary>
    [JsonPropertyName("lineMetrics")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? LineMetrics { get; set; }

    /// <summary>
    /// A property to use as a feature id (for feature state). Either a property name, or an object of the form {"{layer}": "{property}"}. Optional.
    /// </summary>
    [JsonPropertyName("promoteId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? PromoteId { get; set; }

    /// <summary>
    /// Minimum zoom level for which tiles are available.
    /// Default is 0. Optional.
    /// </summary>
    [JsonPropertyName("minzoom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? MinZoom { get; set; }

    /// <summary>
    /// Maximum zoom level for which tiles are available. Data from tiles at the maxzoom are used when displaying the map at higher zoom levels.
    /// Default is 18 for GeoJSON sources. Optional.
    /// </summary>
    [JsonPropertyName("maxzoom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? MaxZoom { get; set; }

    /// <summary>
    /// The size of the tiles in pixels. Default is 512. Optional.
    /// </summary>
    [JsonPropertyName("tileSize")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TileSize { get; set; }

    /// <summary>
    /// Contains an attribution to be displayed when the map is shown to a user. Optional.
    /// </summary>
    [JsonPropertyName("attribution")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Attribution { get; set; }
}
