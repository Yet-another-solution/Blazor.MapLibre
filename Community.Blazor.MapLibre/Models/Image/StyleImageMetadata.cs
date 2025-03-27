using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Image;

public class StyleImageMetadata
{
    /// <summary>
    /// Defines the part of the image that can be covered by the content in <c>text-field</c> when <c>icon-text-fit</c> is used.
    /// </summary>
    [JsonPropertyName("content")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double[]? Content { get; set; }

    /// <summary>
    /// The ratio of pixels in the image to physical pixels on the screen.
    /// </summary>
    [JsonPropertyName("pixelRatio")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? PixelRatio { get; set; }

    /// <summary>
    /// Determines whether the image should be interpreted as an SDF (Signed Distance Field) image.
    /// </summary>
    [JsonPropertyName("sdf")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Sdf { get; set; }

    /// <summary>
    /// Defines the part(s) of the image that can be stretched horizontally when <c>icon-text-fit</c> is used.
    /// </summary>
    [JsonPropertyName("stretchX")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double[][]? StretchX { get; set; }

    /// <summary>
    /// Defines the part(s) of the image that can be stretched vertically when <c>icon-text-fit</c> is used.
    /// </summary>
    [JsonPropertyName("stretchY")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double[][]? StretchY { get; set; }

    /// <summary>
    /// Defines constraints on the vertical scaling of the image when <c>icon-text-fit</c> is used.
    /// </summary>
    [JsonPropertyName("textFitHeight")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TextFit? TextFitHeight { get; set; }

    /// <summary>
    /// Defines constraints on the horizontal scaling of the image when <c>icon-text-fit</c> is used.
    /// </summary>
    [JsonPropertyName("textFitWidth")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TextFit? TextFitWidth { get; set; }

}
