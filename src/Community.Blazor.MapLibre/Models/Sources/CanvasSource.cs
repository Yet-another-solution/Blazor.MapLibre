using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Sources;

/// <summary>
/// Represents a canvas source. Canvas sources provide content from an HTML canvas element to be displayed on the map.
/// The canvas content is copied to the map on each frame, enabling animation.
/// </summary>
public class CanvasSource : ISource
{
    /// <inheritdoc />
    [JsonPropertyName("type")]
    public string Type => "canvas";

    /// <summary>
    /// The geographical coordinates of the four corners of the canvas, specified in clockwise order:
    /// top left, top right, bottom right, bottom left. Required.
    /// Each corner is specified as an array containing `[longitude, latitude]`.
    /// </summary>
    [JsonPropertyName("coordinates")]
    public List<List<double>> Coordinates { get; set; } = [];

    /// <summary>
    /// HTML canvas element ID or reference. Optional.
    /// </summary>
    [JsonPropertyName("canvas")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Canvas { get; set; }

    /// <summary>
    /// Whether the canvas source is animated. If true, the canvas is copied to the map on each frame.
    /// If false, the canvas is only copied once. Default is true. Optional.
    /// </summary>
    [JsonPropertyName("animate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Animate { get; set; }
}
