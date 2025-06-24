namespace Community.Blazor.MapLibre.Models.Marker;

using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;

/// <summary>
/// Options for configuring a marker on the map.
/// </summary>
public class MarkerOptions
{
    /// <summary>
    /// A string indicating the part of the Marker that should be positioned closest to the coordinate.
    /// Options are 'center', 'top', 'bottom', 'left', 'right', 'top-left', 'top-right', 'bottom-left', and 'bottom-right'.
    /// </summary>
    [JsonPropertyName("anchor")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MarkerAnchor? Anchor { get; set; }

    /// <summary>
    /// Space-separated CSS class names to add to marker element.
    /// </summary>
    [JsonPropertyName("className")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClassName { get; set; }

    /// <summary>
    /// The max number of pixels a user can shift the mouse pointer during a click on the marker for it to be 
    /// considered a valid click (as opposed to a marker drag). The default is to inherit map's clickTolerance.
    /// </summary>
    [JsonPropertyName("clickTolerance")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? ClickTolerance { get; set; }

    /// <summary>
    /// The color to use for the default marker if options.element is not provided. The default is light blue.
    /// </summary>
    [JsonPropertyName("color")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Color { get; set; }

    /// <summary>
    /// A boolean indicating whether or not a marker is able to be dragged to a new position on the map.
    /// </summary>
    [JsonPropertyName("draggable")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Draggable { get; set; }

    /// <summary>
    /// DOM element to use as a marker. The default is a light blue, droplet-shaped SVG marker.
    /// </summary>
    [JsonIgnore]
    public ElementReference? Element { get; set; }

    /// <summary>
    /// The offset in pixels as a [x, y] array to apply relative to the element's center. 
    /// Negatives indicate left and up.
    /// </summary>
    [JsonPropertyName("offset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double[]? Offset { get; set; }

    /// <summary>
    /// Marker's opacity when it's in clear view (not behind 3D terrain).
    /// </summary>
    [JsonPropertyName("opacity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Opacity { get; set; }

    /// <summary>
    /// Marker's opacity when it's behind 3D terrain.
    /// </summary>
    [JsonPropertyName("opacityWhenCovered")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OpacityWhenCovered { get; set; }

    /// <summary>
    /// 'map' aligns the Marker to the plane of the map.
    /// 'viewport' aligns the Marker to the plane of the viewport.
    /// 'auto' automatically matches the value of rotationAlignment.
    /// </summary>
    [JsonPropertyName("pitchAlignment")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MarkerAlignment? PitchAlignment { get; set; }

    /// <summary>
    /// The rotation angle of the marker in degrees, relative to its respective rotationAlignment setting.
    /// A positive value will rotate the marker clockwise.
    /// </summary>
    [JsonPropertyName("rotation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Rotation { get; set; }

    /// <summary>
    /// 'map' aligns the Marker's rotation relative to the map, maintaining a bearing as the map rotates.
    /// 'viewport' aligns the Marker's rotation relative to the viewport, agnostic to map rotations.
    /// 'auto' is equivalent to 'viewport'.
    /// </summary>
    [JsonPropertyName("rotationAlignment")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MarkerAlignment? RotationAlignment { get; set; }

    /// <summary>
    /// The scale to use for the default marker if options.element is not provided. 
    /// The default scale corresponds to a height of 41px and a width of 27px.
    /// </summary>
    [JsonPropertyName("scale")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Scale { get; set; }

    /// <summary>
    /// If true, rounding is disabled for placement of the marker, allowing for
    /// subpixel positioning and smoother movement when the marker is translated.
    /// </summary>
    [JsonPropertyName("subpixelPositioning")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? SubpixelPositioning { get; set; }
}
