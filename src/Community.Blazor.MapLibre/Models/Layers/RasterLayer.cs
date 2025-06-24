using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Layers;

public class RasterLayer : Layer<RasterLayerLayout, RasterLayerPaint>
{
    /// <summary>
    ///  <inheritdoc/>
    /// </summary>
    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public override LayerType Type => LayerType.Raster;

    /// <summary>
    /// Gets or sets the name of the source to be used for this layer.
    /// </summary>
    [JsonPropertyName("source")]
    public required string Source { get; set; }
}

public class RasterLayerLayout;
public class RasterLayerPaint;
