using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Converter;
using OneOf;

namespace Community.Blazor.MapLibre.Models;

/// <summary>
/// Configuration for the map projection. Defines how the map is projected from geographical coordinates to screen coordinates.
/// </summary>
public class ProjectionSpecification
{
    /// <summary>
    /// The projection definition. Can be a string (e.g., "mercator", "globe", "naturalEarth"), 
    /// a transition object, or an expression (e.g., an interpolation by zoom level).
    /// Defaults to <c>"mercator"</c>.
    /// </summary>
    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(OneOfJsonConverter<string>))]
    public OneOf<string, JsonArray>? Type { get; set; }
}