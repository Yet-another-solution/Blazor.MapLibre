namespace Community.Blazor.MapLibre.Models.Source;

/// <summary>
/// Represents an image source. Image sources display a rasterized image stretched to fit a specific set of geographical coordinates.
/// </summary>
public class ImageSource : SourceBase
{
    /// <inheritdoc />
    public override string Type => "image";

    /// <summary>
    /// The URL to the image. This is used to load the image to be displayed on the map. Required.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// The geographical coordinates of the four corners of the image, specified in clockwise order:
    /// top left, top right, bottom right, bottom left. Required.
    /// Each corner is specified as an array containing `[longitude, latitude]`.
    /// </summary>
    public List<List<double>> Coordinates { get; set; } = new List<List<double>>();
}