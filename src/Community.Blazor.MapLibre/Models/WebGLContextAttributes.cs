namespace Community.Blazor.MapLibre.Models;

/// <summary>
/// WebGL context attributes for the map.
/// </summary>
public class WebGLContextAttributes
{
    public bool? Antialias { get; set; }
    public string PowerPreference { get; set; } = "high-performance";
    public bool? PreserveDrawingBuffer { get; set; }
    public bool? FailIfMajorPerformanceCaveat { get; set; }
    public bool? Desynchronized { get; set; }
    public string ContextType { get; set; } = "webgl2withfallback";
}