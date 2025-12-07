using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Layers;

public class HillShadeLayer : Layer<HillShadeLayerLayout, HillShadeLayerPaint>
{
    /// <summary>
    /// Gets or sets the name of the source to be used for this layer.
    /// </summary>
    [JsonPropertyName("source")]
    public required string Source { get; set; }
}

public class HillShadeLayerLayout;
public class HillShadeLayerPaint;
