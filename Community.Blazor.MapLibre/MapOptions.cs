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
    public double? MinPitch { get; set; } = 0;

    /// <summary>
    /// The maximum pitch of the map (0-180).
    /// @defaultValue 60
    /// </summary>
    public double? MaxPitch { get; set; } = 60;

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
    /// If `true`, the "drag to pitch" interaction is enabled. An `Object` value is passed as options to {@link TwoFingersTouchPitchHandler#enable}.
    /// @defaultValue true
    /// </summary>
    public object TouchPitch { get; set; } = true;

    /// <summary>
    /// If `true` or set to an options object, the map is only accessible on desktop while holding Command/Ctrl and only accessible on mobile with two fingers. Interacting with the map using normal gestures will trigger an informational screen. With this option enabled, "drag to pitch" requires a three-finger gesture. Cooperative gestures are disabled when a map enters fullscreen using {@link FullscreenControl}.
    /// @defaultValue false
    /// </summary>
    public object CooperativeGestures { get; set; } = false;

    /// <summary>
    /// If `true`, the map will automatically resize when the browser window resizes.
    /// @defaultValue true
    /// </summary>
    public bool TrackResize { get; set; } = true;

    /// <summary>
    /// The initial geographical centerpoint of the map. If `center` is not specified, it defaults to [0, 0].
    /// </summary>
    public LngLat Center { get; set; } = new LngLat { Longitude = 0, Latitude = 0 };

    /// <summary>
    /// The elevation of the initial geographical centerpoint of the map, in meters above sea level. If `elevation` is not specified in the constructor options, it will default to `0`.
    /// @defaultValue 0
    /// </summary>
    public double Elevation { get; set; } = 0;

    /// <summary>
    /// The initial zoom level of the map. Defaults to 0.
    /// @defaultValue 0
    /// </summary>
    public double Zoom { get; set; } = 0;

    /// <summary>
    /// The initial bearing (rotation) of the map, measured in degrees counter-clockwise from north. If `bearing` is not specified in the constructor options, MapLibre GL JS will look for it in the map's style object. If it is not specified in the style, either, it will default to `0`.
    /// @defaultValue 0
    /// </summary>
    public double Bearing { get; set; } = 0;

    /// <summary>
    /// The initial pitch (tilt) of the map, measured in degrees away from the plane of the screen (0-85). If `pitch` is not specified in the constructor options, MapLibre GL JS will look for it in the map's style object. If it is not specified in the style, either, it will default to `0`. Values greater than 60 degrees are experimental and may result in rendering issues. If you encounter any, please raise an issue with details in the MapLibre project.
    /// @defaultValue 0
    /// </summary>
    public double Pitch { get; set; } = 0;

    /// <summary>
    /// The initial roll angle of the map, measured in degrees counter-clockwise about the camera boresight. If `roll` is not specified in the constructor options, MapLibre GL JS will look for it in the map's style object. If it is not specified in the style, either, it will default to `0`.
    /// @defaultValue 0
    /// </summary>
    public double Roll { get; set; } = 0;

    /// <summary>
    /// If `true`, multiple copies of the world will be rendered side by side beyond -180 and 180 degrees longitude. If set to `false`:
    ///
    /// - When the map is zoomed out far enough that a single representation of the world does not fill the map's entire
    ///   container, there will be blank space beyond 180 and -180 degrees longitude.
    /// - Features that cross 180 and -180 degrees longitude will be cut in two (with one portion on the right edge of the
    ///   map and the other on the left edge of the map) at every zoom level.
    /// @defaultValue true
    /// </summary>
    public bool RenderWorldCopies { get; set; } = true;

    /// <summary>
    /// The maximum number of tiles stored in the tile cache for a given source. If omitted, the cache will be dynamically sized based on the current viewport which can be set using `maxTileCacheZoomLevels` constructor options.
    /// @defaultValue null
    /// </summary>
    public double? MaxTileCacheSize { get; set; } = null;

    /// <summary>
    /// The maximum number of zoom levels for which to store tiles for a given source. Tile cache dynamic size is calculated by multiplying `maxTileCacheZoomLevels` with the approximate number of tiles in the viewport for a given source.
    /// @defaultValue 5
    /// </summary>
    public double MaxTileCacheZoomLevels { get; set; } = 5;

    /// <summary>
    /// A callback run before the Map makes a request for an external URL. The callback can be used to modify the url, set headers, or set the credentials property for cross-origin requests.
    /// Expected to return an object with a `url` property and optionally `headers` and `credentials` properties.
    /// @defaultValue null
    /// </summary>
    public object? TransformRequest { get; set; } = null;

    /// <summary>
    /// A callback run before the map's camera is moved due to user input or animation. The callback can be used to modify the new center, zoom, pitch and bearing.
    /// Expected to return an object containing center, zoom, pitch or bearing values to overwrite.
    /// @defaultValue null
    /// </summary>
    public object? TransformCameraUpdate { get; set; } = null;

    /// <summary>
    /// A patch to apply to the default localization table for UI strings, e.g. control tooltips. The `locale` object maps namespaced UI string IDs to translated strings in the target language; see `src/ui/default_locale.js` for an example with all supported string IDs. The object may specify all UI strings (thereby adding support for a new translation) or only a subset of strings (thereby patching the default translation table).
    /// @defaultValue null
    /// </summary>
    public object? Locale { get; set; } = null;

    /// <summary>
    /// Controls the duration of the fade-in/fade-out animation for label collisions after initial map load, in milliseconds. This setting affects all symbol layers. This setting does not affect the duration of runtime styling transitions or raster tile cross-fading.
    /// @defaultValue 300
    /// </summary>
    public double FadeDuration { get; set; } = 300;

    /// <summary>
    /// If `true`, symbols from multiple sources can collide with each other during collision detection. If `false`, collision detection is run separately for the symbols in each source.
    /// @defaultValue true
    /// </summary>
    public bool CrossSourceCollisions { get; set; } = true;

    /// <summary>
    /// If `true`, Resource Timing API information will be collected for requests made by GeoJSON and Vector Tile web workers (this information is normally inaccessible from the main Javascript thread). Information will be returned in a `resourceTiming` property of relevant `data` events.
    /// @defaultValue false
    /// </summary>
    public bool CollectResourceTiming { get; set; } = false;

    /// <summary>
    /// The max number of pixels a user can shift the mouse pointer during a click for it to be considered a valid click (as opposed to a mouse drag).
    /// @defaultValue 3
    /// </summary>
    public double ClickTolerance { get; set; } = 3;

    /// <summary>
    /// The initial bounds of the map. If `bounds` is specified, it overrides `center` and `zoom` constructor options.
    /// </summary>
    public LngLatBounds? Bounds { get; set; }

    /// <summary>
    /// A {@link FitBoundsOptions} options object to use _only_ when fitting the initial `bounds` provided above.
    /// </summary>
    public object? FitBoundsOptions { get; set; }

    /// <summary>
    /// Defines a CSS
    /// font-family for locally overriding generation of Chinese, Japanese, and Korean characters.
    /// For these characters, font settings from the map's style will be ignored, except for font-weight keywords (light/regular/medium/bold).
    /// Set to `false`, to enable font settings from the map's style for these glyph ranges.
    /// The purpose of this option is to avoid bandwidth-intensive glyph server requests. (See [Use locally generated ideographs](https://maplibre.org/maplibre-gl-js/docs/examples/local-ideographs).)
    /// @defaultValue 'sans-serif'
    /// </summary>
    public object LocalIdeographFontFamily { get; set; } = "sans-serif";

    /// <summary>
    /// The map's MapLibre style. This must be a JSON object or a URL to a JSON object conforming to the MapLibre Style Specification.
    /// </summary>
    /// <remarks>Can be StyleSpecification or string (URL)</remarks>
    public object Style { get; set; } = "https://demotiles.maplibre.org/style.json";

    /// <summary>
    /// If `false`, the map's pitch (tilt) control with "drag to rotate" interaction will be disabled.
    /// @defaultValue true
    /// </summary>
    public bool PitchWithRotate { get; set; } = true;

    /// <summary>
    /// If `false`, the map's roll control with "drag to rotate" interaction will be disabled.
    /// @defaultValue false
    /// </summary>
    public bool RollEnabled { get; set; } = false;

    /// <summary>
    /// The pixel ratio.
    /// The canvas' `width` attribute will be `container.clientWidth * pixelRatio` and its `height` attribute will be `container.clientHeight * pixelRatio`. Defaults to `devicePixelRatio` if not specified.
    /// </summary>
    public double? PixelRatio { get; set; }

    /// <summary>
    /// If false, style validation will be skipped. Useful in production environment.
    /// @defaultValue true
    /// </summary>
    public bool ValidateStyle { get; set; } = true;

    /// <summary>
    /// The canvas' maximum `width` and `height` size.
    /// The values are provided as an array where the first element is the max width and the second element is the max height.
    /// Ensure this is not set above WebGL `MAX_TEXTURE_SIZE`.
    /// </summary>
    /// <remarks>
    /// Default value: [4096, 4096].
    /// </remarks>
    public int[] MaxCanvasSize { get; set; } = [4096, 4096];

    /// <summary>
    /// Determines whether to cancel, or retain, tiles from the current viewport which are still loading but which belong to a farther (smaller) zoom level than the current one.
    /// * If `true`, when zooming in, tiles which didn't manage to load for previous zoom levels will become canceled. This might save some computing resources for slower devices, but the map details might appear more abruptly at the end of the zoom.
    /// * If `false`, when zooming in, the previous zoom level(s) tiles will progressively appear, giving a smoother map details experience. However, more tiles will be rendered in a short period of time.
    /// @defaultValue true
    /// </summary>
    public bool CancelPendingTileRequestsWhileZooming { get; set; } = true;

    /// <summary>
    /// If true, the elevation of the center point will automatically be set to the terrain elevation
    /// (or zero if terrain is not enabled). If false, the elevation of the center point will default
    /// to sea level and will not automatically update. Defaults to true. Needs to be set to false to
    /// keep the camera above ground when pitch \> 90 degrees.
    /// </summary>
    public bool? CenterClampedToGround { get; set; }
    
    /// <summary>
    /// <para>
    /// Determines whether the library should automatically handle geographic data that
    /// crosses the antimeridian (180° E or 180° W).
    /// </para>
    ///
    /// <para>
    /// According to <see href="https://tools.ietf.org/html/rfc7946#section-3.1.9">
    /// RFC 7946 Section 3.1.9</see>, such objects SHOULD be split into two or more
    /// objects that do not cross the antimeridian, while preserving equivalence.
    /// </para>
    ///
    /// <para>
    /// When set to <c>true</c>, all GeoJSON features crossing the antimeridian will be
    /// automatically split into two parts. The data will be processed using
    /// <see href="https://gitlab.com/avandesa/geojson-antimeridian-cut">geojson-antimeridian-cut</see>.
    /// </para>
    /// </summary>
    [JsonPropertyName("cutAtAntimeridian")]
    public bool? CutAtAntimeridian { get; set; }
}

