using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Sources;

/// <summary>
/// The options object used with <see cref="QuerySourceFeatures(string, QuerySourceFeatureOptions?)"/> to filter the results.
/// </summary>
public class QuerySourceFeatureOptions
{
    /// <summary>
    /// A filter to limit query results.
    /// The syntax must follow the MapLibre Style Specification.
    /// </summary>
    [JsonPropertyName("filter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Filter { get; set; }
    
    /// <summary>
    /// The name of the source layer to query.
    /// <para>
    /// For vector tile sources, this parameter is required. For GeoJSON sources, it is ignored.
    /// </para>
    /// </summary>
    [JsonPropertyName("sourceLayer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SourceLayer { get; set; }

    /// <summary>
    /// Whether to validate that the <c>filter</c> conforms to the MapLibre Style Specification.
    /// <para>
    /// Disabling validation can improve performance but should only be done if the values are already known to be valid.
    /// </para>
    /// </summary>
    /// <remarks>Default is <c>true</c>.</remarks>
    [JsonPropertyName("validate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Validate { get; set; }
}
