using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models;

/// <summary>
/// Enumeration of possible values for <c>StyleImageMetadata.textFitWidth</c> and <c>textFitHeight</c>.
/// </summary>
public enum TextFit
{
    /// <summary>
    /// The image will be resized on the specified axis to fit the content rectangle to the target text
    /// and will resize the other axis to maintain the aspect ratio of the content rectangle.
    /// </summary>
    [JsonStringEnumMemberName("proportional")]
    Proportional,

    /// <summary>
    /// The image will be resized on the specified axis to fit the content rectangle to the target text,
    /// but will not fall below the aspect ratio of the original content rectangle if the other axis is set to <c>proportional</c>.
    /// </summary>
    [JsonStringEnumMemberName("stretchOnly")]
    StretchOnly,

    /// <summary>
    /// The image will be resized on the specified axis to tightly fit the content rectangle to the target text.
    /// This is the same as not being defined.
    /// </summary>
    [JsonStringEnumMemberName("stretchOrShrink")]
    StretchOrShrink
}
