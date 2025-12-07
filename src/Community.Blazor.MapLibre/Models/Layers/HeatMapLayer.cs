using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Layers;

public class HeatMapLayer : Layer<HeatMapLayerLayout, HeatMapLayerPaint>
{
    /// <summary>
    /// Gets or sets the name of the source to be used for this layer.
    /// </summary>
    [JsonPropertyName("source")]
    public required string Source { get; set; }

    /// <summary>
    /// Gets or sets the layer to use from a vector tile source.
    /// </summary>
    /// <remarks>
    /// Required for vector tile sources. Specifies the layer within the vector tiles to use for this layer.
    /// </remarks>
    [JsonPropertyName("source-layer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SourceLayer { get; set; }
}

public class HeatMapLayerLayout;
public class HeatMapLayerPaint;
