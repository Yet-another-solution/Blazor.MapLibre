using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Sprite;

/// <summary>
/// Supporting type to add validation to another style related type
/// </summary>
public class StyleSetterOptions
{
    /// <summary>
    /// Whether to check if the filter conforms to the MapLibre Style Specification. Disabling validation is a performance optimization that should only be used if you have previously validated the values you will be passing to this function.
    /// </summary>
    [JsonPropertyName("validate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Validate { get; set; }
}