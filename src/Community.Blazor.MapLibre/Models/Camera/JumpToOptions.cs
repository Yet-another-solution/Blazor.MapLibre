using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Models.Padding;

namespace Community.Blazor.MapLibre.Models.Camera;

public class JumpToOptions : CameraOptions
{
    /// <summary>
    /// Dimensions in pixels applied on each side of the viewport for shifting the vanishing point.
    /// </summary>
    [JsonPropertyName("padding")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PaddingOptions? Padding { get; set; }

}