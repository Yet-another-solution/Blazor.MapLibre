using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Models.Padding;
using OneOf;

namespace Community.Blazor.MapLibre.Models.Camera;

public class EaseToOptions : IAnimationOptions, ICameraOptions
{

    /// <summary>
    /// If `zoom` is specified, `around` determines the point around which the zoom is centered.
    /// </summary>
    [JsonPropertyName("around")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LngLat? Around { get; set; }
    
    [JsonPropertyName("delayEndEvents")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? DelayEndEvents { get; set; }
    
    [JsonPropertyName("easeId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? EaseId { get; set; }

    [JsonPropertyName("noMoveStart")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? NoMoveStart { get; set; }
    
    [JsonPropertyName("padding")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public OneOf<double, PaddingOptions>? Padding { get; set; }

    #region IAnimationOptions

    /// <inheritdoc />
    [JsonPropertyName("animate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Animate { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("duration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Duration { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("easing")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Func<double, double>? Easing { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("essential")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Essential { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("freezeElevation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FreezeElevation { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("offset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PointLike? Offset { get; set; }

    #endregion

    #region ICameraOptions

    /// <inheritdoc />
    [JsonPropertyName("pitch")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Pitch { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("roll")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Roll { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("elevation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Elevation { get; set; }

    #region ICenterZoomBearing

    /// <inheritdoc />
    public LngLat? Center { get; set; }

    /// <inheritdoc />
    public double? Zoom { get; set; }

    /// <inheritdoc />
    public double? Bearing { get; set; }

    #endregion

    #endregion
}
