using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Layers;

public class FillExtrusionLayer : Layer<FillExtrusionLayerLayout, FillExtrusionLayerPaint>
{
    /// <summary>
    ///  <inheritdoc/>
    /// </summary>
    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public override LayerType Type => LayerType.FillExtrusion;

    /// <summary>
    /// Gets or sets the name of the source to be used for this layer.
    /// </summary>
    [JsonPropertyName("source")]
    public required string Source { get; set; }
}

public class FillExtrusionLayerLayout;
public class FillExtrusionLayerPaint;
