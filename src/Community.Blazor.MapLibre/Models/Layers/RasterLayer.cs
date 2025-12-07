using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Layers;

public class RasterLayer : Layer<RasterLayerLayout, RasterLayerPaint>
{
    /// <summary>
    /// Gets or sets the name of the source to be used for this layer.
    /// </summary>
    [JsonPropertyName("source")]
    public required string Source { get; set; }
}

public class RasterLayerLayout;
public class RasterLayerPaint;
