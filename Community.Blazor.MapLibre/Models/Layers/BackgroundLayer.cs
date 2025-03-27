using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Layers;

public class BackgroundLayer : Layer<BackgroundLayerLayout, BackgroundLayerPaint>
{
    /// <summary>
    ///  <inheritdoc/>
    /// </summary>
    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public override LayerType Type => LayerType.Background;
}

public class BackgroundLayerLayout;
public class BackgroundLayerPaint;
