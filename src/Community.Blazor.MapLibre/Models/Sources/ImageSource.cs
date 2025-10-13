using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Sources;

/// <summary>
/// Represents an image source. Image sources display a rasterized image stretched to fit a specific set of geographical coordinates.
/// </summary>
public class ImageSource : ISource
{
    /// <inheritdoc />
    [JsonPropertyName("type")]
    public string Type => "image";
    /// <inheritdoc />
    [JsonPropertyName("attribution")]
    public string? Attribution { get; set; }
    /// <inheritdoc />
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    /// <inheritdoc />
    [JsonPropertyName("isTileClipped")]
    public bool? IsTileClipped { get; set; }
    /// <inheritdoc />
    [JsonPropertyName("maxzoom")]
    public float? MaxZoom { get; set; }
    /// <inheritdoc />
    [JsonPropertyName("minzoom")]
    public float? MinZoom { get; set; }
    /// <inheritdoc />
    [JsonPropertyName("reparseOverscaled")]
    public bool? ReparseOverscaled { get; set; }
    /// <inheritdoc />
    [JsonPropertyName("roundZoom")]
    public bool? RoundZoom { get; set; }
    /// <inheritdoc />
    [JsonPropertyName("tileID")]
    public string? TileID { get; set; }
    /// <inheritdoc />
    [JsonPropertyName("tileSize")]
    public float? TileSize { get; set; }
    /// <inheritdoc />
    [JsonPropertyName("vectorLayerIds")]
    public string[]? VectorLayerIds { get; set; }

    /// <summary>
    /// The URL to the image. This is used to load the image to be displayed on the map. Required.
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// The geographical coordinates of the four corners of the image, specified in clockwise order:
    /// top left, top right, bottom right, bottom left. Required.
    /// Each corner is specified as an array containing `[longitude, latitude]`.
    /// </summary>
    [JsonPropertyName("coordinates")]
    public List<List<double>> Coordinates { get; set; } = [];
}
