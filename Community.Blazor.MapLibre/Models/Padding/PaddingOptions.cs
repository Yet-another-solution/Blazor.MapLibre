using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Padding;

public class PaddingOptions
{
    /// <summary>
    /// Padding in pixels from the top of the map canvas.
    /// </summary>
    [JsonPropertyName("top")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Top { get; set; }

    /// <summary>
    /// Padding in pixels from the bottom of the map canvas.
    /// </summary>
    [JsonPropertyName("bottom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Bottom { get; set; }

    /// <summary>
    /// Padding in pixels from the right of the map canvas.
    /// </summary>
    [JsonPropertyName("right")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Right { get; set; }

    /// <summary>
    /// Padding in pixels from the left of the map canvas.
    /// </summary>
    [JsonPropertyName("left")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Left { get; set; }

}