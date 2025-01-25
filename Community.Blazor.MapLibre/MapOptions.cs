using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Models;

namespace Community.Blazor.MapLibre;

/// <summary>
/// Represents the options for initializing a MapLibre map.
/// </summary>
public class MapOptions
{
    /// <summary>
    /// If `true`, the map's position (zoom, center latitude, center longitude, bearing, and pitch) will be synced with the hash fragment of the page's URL.
    /// Example: http://path/to/my/page.html#2.59/39.26/53.07/-24.1/60.
    /// An additional string may optionally indicate a parameter-styled hash.
    /// @defaultValue false
    /// </summary>
    /// <remarks>Can be bool or string</remarks>
    public object Hash { get; set; } = false;

    /// <summary>
    /// If `false`, no mouse, touch, or keyboard listeners will be attached to the map, so it will not respond to interaction.
    /// @defaultValue true
    /// </summary>
    public bool? Interactive { get; set; } = true;

    /// <summary>
    /// The HTML element in which MapLibre GL JS will render the map, or the element's string `id`. The specified element must have no children.
    /// </summary>
    /// <remarks>Can be string (ID) or HTMLElement reference</remarks>
    public object Container { get; set; } = "";

    /// <summary>
    /// The threshold, measured in degrees, that determines when the map's bearing will snap to north.
    /// Example: with a `bearingSnap` of 7, if the user rotates the map within 7 degrees of north, it will snap to exact north.
    /// @defaultValue 7
    /// </summary>
    public double? BearingSnap { get; set; } = 7;

    /// <summary>
    /// If set, an {@link AttributionControl} will be added to the map with the provided options. To disable the attribution control, pass `false`.
    /// Note: showing the MapLibre logo is not required for using MapLibre.
    /// @defaultValue compact: true, customAttribution: "MapLibre ..."
    /// </summary>
    /// <remarks>Can be false or AttributionControlOptions</remarks>
    public object? AttributionControl { get; set; }

    /// <summary>
    /// If `true`, the MapLibre logo will be shown.
    /// </summary>
    public bool? MapLibreLogo { get; set; }

    /// <summary>
    /// A string representing the position of the MapLibre wordmark on the map. Valid options are `top-left`, `top-right`, `bottom-left`, or `bottom-right`.
    /// @defaultValue "bottom-left"
    /// </summary>
    public string LogoPosition { get; set; } = "bottom-left";

    /// <summary>
    /// Set of WebGLContextAttributes that are applied to the WebGL context of the map. 
    /// See https://developer.mozilla.org/en-US/docs/Web/API/HTMLCanvasElement/getContext for more details.
    /// `contextType` can be set to `webgl2` or `webgl` to force a WebGL version.
    /// @defaultValue antialias: false, powerPreference: "high-performance", preserveDrawingBuffer: false, failIfMajorPerformanceCaveat: false, desynchronized: false, contextType: "webgl2withfallback"
    /// </summary>
    public WebGLContextAttributes? CanvasContextAttributes { get; set; }

    /// <summary>
    /// If `false`, the map won't attempt to re-request tiles once they expire per their HTTP `cacheControl`/`expires` headers.
    /// @defaultValue true
    /// </summary>
    public bool? RefreshExpiredTiles { get; set; } = true;

    /// <summary>
    /// If set, the map will be constrained to the given bounds.
    /// </summary>
    public LngLatBounds? MaxBounds { get; set; }

    /// <summary>
    /// If `true`, the "scroll to zoom" interaction is enabled.
    /// @defaultValue true
    /// </summary>
    /// <remarks>Can be bool or AroundCenterOptions</remarks>
    public object ScrollZoom { get; set; } = true; 

    /// <summary>
    /// The minimum zoom level of the map (0-24).
    /// @defaultValue 0
    /// </summary>
    public double? MinZoom { get; set; }

    /// <summary>
    /// The maximum zoom level of the map (0-24).
    /// @defaultValue 22
    /// </summary>
    public double? MaxZoom { get; set; } = 22;

    /// <summary>
    /// The minimum pitch of the map (0-180).
    /// @defaultValue 0
    /// </summary>
    public double? MinPitch { get; set; }

    /// <summary>
    /// The maximum pitch of the map (0-180).
    /// @defaultValue 60
    /// </summary>
    public double? MaxPitch { get; set; }

    /// <summary>
    /// If `true`, the "box zoom" interaction is enabled.
    /// @defaultValue true
    /// </summary>
    public bool? BoxZoom { get; set; } = true;

    /// <summary>
    /// If `true`, the "drag to rotate" interaction is enabled.
    /// @defaultValue true
    /// </summary>
    public bool? DragRotate { get; set; } = true;

    /// <summary>
    /// If `true`, the "drag to pan" interaction is enabled.
    /// @defaultValue true
    /// </summary>
    /// <remarks>Can be bool or DragPanOptions</remarks>
    public object DragPan { get; set; } = true; 

    /// <summary>
    /// If `true`, keyboard shortcuts are enabled.
    /// @defaultValue true
    /// </summary>
    public bool? Keyboard { get; set; } = true;

    /// <summary>
    /// If `true`, the "double click to zoom" interaction is enabled.
    /// @defaultValue true
    /// </summary>
    public bool? DoubleClickZoom { get; set; } = true;

    /// <summary>
    /// If `true`, the "pinch to rotate and zoom" interaction is enabled.
    /// @defaultValue true
    /// </summary>
    /// <remarks>Can be bool or AroundCenterOptions</remarks>
    public object TouchZoomRotate { get; set; } = true; 

    /// <summary>
    /// The initial geographical centerpoint of the map. If `center` is not specified, it defaults to [0, 0].
    /// </summary>
    public LngLat Center { get; set; } = new LngLat { Longitude = 0, Latitude = 0 };

    /// <summary>
    /// The initial zoom level of the map. Defaults to 0.
    /// @defaultValue 0
    /// </summary>
    public double? Zoom { get; set; } = 0;

    /// <summary>
    /// The map's MapLibre style. This must be a JSON object or a URL to a JSON object conforming to the MapLibre Style Specification.
    /// </summary>
    /// <remarks>Can be StyleSpecification or string (URL)</remarks>
    public object Style { get; set; } = "https://demotiles.maplibre.org/style.json";
}

