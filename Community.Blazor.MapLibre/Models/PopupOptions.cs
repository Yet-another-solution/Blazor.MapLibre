using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models;

public class PopupOptions
{
    /// <summary>
    /// Space-separated CSS class names to add to the popup container.
    /// </summary>
    [JsonPropertyName("className")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClassName { get; set; }

    /// <summary>
    /// If <c>true</c>, a close button will appear in the top right corner of the popup.
    /// </summary>
    /// <remarks>Default value: <c>true</c></remarks>
    [JsonPropertyName("closeButton")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CloseButton { get; set; }

    /// <summary>
    /// If <c>true</c>, the popup will close when the map is clicked.
    /// </summary>
    /// <remarks>Default value: <c>true</c></remarks>
    [JsonPropertyName("closeOnClick")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CloseOnClick { get; set; }

    /// <summary>
    /// If <c>true</c>, the popup will close when the map moves.
    /// </summary>
    /// <remarks>Default value: <c>false</c></remarks>
    [JsonPropertyName("closeOnMove")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CloseOnMove { get; set; }

    /// <summary>
    /// If <c>true</c>, the popup will try to focus the first focusable element inside the popup.
    /// </summary>
    /// <remarks>Default value: <c>true</c></remarks>
    [JsonPropertyName("focusAfterOpen")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FocusAfterOpen { get; set; }

    /// <summary>
    /// Optional opacity when the location is behind the globe.
    /// If a number is provided, it will be converted to a string.
    /// </summary>
    /// <remarks>Default value: <c>undefined</c></remarks>
    [JsonPropertyName("locationOccludedOpacity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? LocationOccludedOpacity { get; set; }

    /// <summary>
    /// Sets the CSS property of the popup's maximum width, e.g., <c>300px</c>.
    /// To ensure the popup resizes to fit its content, set this property to <c>none</c>.
    /// <para>
    /// See: <see href="https://developer.mozilla.org/en-US/docs/Web/CSS/max-width">CSS max-width documentation</see>.
    /// </para>
    /// </summary>
    /// <remarks>Default value: <c>240px</c></remarks>
    [JsonPropertyName("maxWidth")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? MaxWidth { get; set; }

    /// <summary>
    /// A pixel offset applied to the popup's location.
    /// </summary>
    [JsonPropertyName("offset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Offset { get; set; }

    /// <summary>
    /// Indicates the part of the popup that should be positioned closest to the coordinate set via <c>SetLngLat</c>.
    /// Options: <c>center</c>, <c>top</c>, <c>bottom</c>, <c>left</c>, <c>right</c>, <c>top-left</c>, <c>top-right</c>, <c>bottom-left</c>, <c>bottom-right</c>.
    /// If unset, the anchor will be dynamically set with a preference for <c>bottom</c>.
    /// </summary>
    [JsonPropertyName("anchor")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PositionAnchor? Anchor { get; set; }

    /// <summary>
    /// If <c>true</c>, rounding is disabled for placement of the popup, allowing for subpixel positioning and smoother movement when the popup is translated.
    /// </summary>
    /// <remarks>Default value: <c>false</c></remarks>
    [JsonPropertyName("subpixelPositioning")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? SubpixelPositioning { get; set; }
}
