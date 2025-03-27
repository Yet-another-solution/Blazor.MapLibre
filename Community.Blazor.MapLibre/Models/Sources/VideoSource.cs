using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Sources;

/// <summary>
/// Represents a video source. Video sources provide video content to be displayed in specific geographical bounds.
/// </summary>
public class VideoSource : ISource
{
    /// <inheritdoc />
    [JsonPropertyName("type")]
    public string Type => "video";

    /// <summary>
    /// URLs to the video content. Multiple URLs should be provided for format compatibility across browsers. Required.
    /// </summary>
    [JsonPropertyName("urls")]
    public List<string> Urls { get; set; } = [];

    /// <summary>
    /// The geographical coordinates of the four corners of the video, specified in clockwise order:
    /// top left, top right, bottom right, bottom left. Required.
    /// </summary>
    [JsonPropertyName("coordinates")]
    public List<List<double>> Coordinates { get; set; } = [];
}
