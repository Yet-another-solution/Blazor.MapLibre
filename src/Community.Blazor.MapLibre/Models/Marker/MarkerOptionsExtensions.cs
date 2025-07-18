using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Marker;

/// <summary>
/// Represents non-Maplibre included marker extensions.
/// </summary>
public sealed class MarkerOptionsExtensions
{
    public MarkerOptionsExtensions()
    {
        
    }

    /// <summary>
    /// The HTML content that will be added to the marker after its creation.
    /// </summary>
    /// <remarks>
    /// If no HTML content is desired, this value should be left empty.
    /// </remarks>
    [JsonPropertyName("htmlContent")]
    public string HtmlContent { get; set; } = string.Empty;
    
    /// <summary>
    /// The HTML content that will be attached to the marker on click interaction.
    /// </summary>
    [JsonPropertyName("popupContent")]
    public string PopupContent { get; set; } = string.Empty;
}
