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
    /// <inheritdoc />
    [JsonPropertyName("attribution")]
    public string? Attribution { get; set; }
    /// <inheritdoc />
    [JsonPropertyName("id")]
    public string Id { get; set; }
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
