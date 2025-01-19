namespace Community.Blazor.MapLibre.Models.Source;

/// <summary>
/// Represents a video source. Video sources provide video content to be displayed in specific geographical bounds.
/// </summary>
public class VideoSource : SourceBase
{
    /// <inheritdoc />
    public override string Type => "video";

    /// <summary>
    /// URLs to the video content. Multiple URLs should be provided for format compatibility across browsers. Required.
    /// </summary>
    public List<string> Urls { get; set; } = new List<string>();

    /// <summary>
    /// The geographical coordinates of the four corners of the video, specified in clockwise order:
    /// top left, top right, bottom right, bottom left. Required.
    /// </summary>
    public List<List<double>> Coordinates { get; set; } = new List<List<double>>();
}