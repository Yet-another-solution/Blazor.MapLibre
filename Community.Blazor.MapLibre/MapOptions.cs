using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Models;

namespace Community.Blazor.MapLibre;

public class MapOptions
{
    /// <summary>
    /// If set, an AttributionControl will be added to the map with the provided options.
    /// Pass <c>false</c> to disable the attribution control.
    /// <list type="table">
    ///   <item>
    ///     <term>compact</term>
    ///     <description>Whether the control is compact (typically used on mobile). Default is <c>true</c>.</description>
    ///   </item>
    ///   <item>
    ///     <term>customAttribution</term>
    ///     <description>A custom attribution string (e.g., "MapLibre ...").</description>
    ///   </item>
    /// </list>
    /// </summary>
    [JsonPropertyName("attributionControl")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? AttributionControl { get; set; }

    /// <summary>
    /// The initial bearing (rotation) of the map, measured in degrees counter-clockwise from north.
    /// If not specified, the value is taken from the map's style object, or defaults to 0.
    /// </summary>
    [JsonPropertyName("bearing")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Bearing { get; set; }

    /// <summary>
    /// The threshold, in degrees, at which the map's bearing will snap to north.
    /// For example, a value of 7 means if the user rotates the map within 7 degrees of north,
    /// it will automatically snap to exact north. Default is 7.
    /// </summary>
    [JsonPropertyName("bearingSnap")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? BearingSnap { get; set; }

    /// <summary>
    /// The initial bounds of the map. Overrides center and zoom constructor options if provided.
    /// </summary>
    [JsonPropertyName("bounds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LngLatBounds? Bounds { get; set; }

    /// <summary>
    /// Enables or disables the \"box zoom\" interaction.
    /// When enabled, users can draw a box to zoom in on a specific area of the map. Default is true.
    /// </summary>
    [JsonPropertyName("boxZoom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? BoxZoom { get; set; }

    /// <summary>
    /// Determines whether to cancel or retain pending tile requests when zooming.
    /// <list type="table">
    ///   <item>
    ///     <term>true</term>
    ///     <description>Unloaded tiles from previous zoom levels are canceled when zooming in. May reduce resource usage but cause abrupt map detail changes.</description>
    ///   </item>
    ///   <item>
    ///     <term>false</term>
    ///     <description>Tiles from previous zoom levels will continue loading, resulting in a smoother zooming experience but more resource usage.</description>
    ///   </item>
    /// </list>
    /// Default is true.
    /// </summary>
    [JsonPropertyName("cancelPendingTileRequestsWhileZooming")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CancelPendingTileRequestsWhileZooming { get; set; }

    /// <summary>
    /// A set of WebGL context attributes applied to the map’s WebGL context.
    /// See: https://developer.mozilla.org/en-US/docs/Web/API/HTMLCanvasElement/getContext
    /// <list type="table">
    ///   <item>
    ///     <term>contextType</term>
    ///     <description>webgl2 or webgl; MapLibre tries to choose the most suitable if not set.</description>
    ///   </item>
    ///   <item>
    ///     <term>antialias</term>
    ///     <description>Whether or not to perform anti-aliasing. Default is false.</description>
    ///   </item>
    ///   <item>
    ///     <term>powerPreference</term>
    ///     <description>GPU performance preference. Default is 'high-performance'.</description>
    ///   </item>
    ///   <item>
    ///     <term>preserveDrawingBuffer</term>
    ///     <description>Whether to preserve the buffer contents after rendering. Default is false.</description>
    ///   </item>
    ///   <item>
    ///     <term>failIfMajorPerformanceCaveat</term>
    ///     <description>Whether to fail if the system performance is significantly degraded. Default is false.</description>
    ///   </item>
    ///   <item>
    ///     <term>desynchronized</term>
    ///     <description>Whether to enable low-latency rendering mode. Default is false.</description>
    ///   </item>
    /// </list>
    /// </summary>
    [JsonPropertyName("canvasContextAttributes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public WebGLContextAttributes? CanvasContextAttributes { get; set; }

    /// <summary>
    /// The initial centerpoint of the map in [longitude, latitude] order. Defaults to [0, 0].
    /// If not specified, the map will use the style’s center or fall back to [0, 0].
    /// </summary>
    [JsonPropertyName("center")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LngLat? Center { get; set; }

    /// <summary>
    /// If true, the center point’s elevation is clamped to terrain height or 0 if terrain is not enabled.
    /// If false, the elevation remains fixed (sea level) and won’t automatically update.
    /// Needed when pitch &gt; 90 degrees. Default is true.
    /// </summary>
    [JsonPropertyName("centerClampedToGround")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CenterClampedToGround { get; set; }

    /// <summary>
    /// The max number of pixels the pointer may move between mousedown and mouseup for the event to be considered a click.
    /// Default is 3.
    /// </summary>
    [JsonPropertyName("clickTolerance")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? ClickTolerance { get; set; }

    /// <summary>
    /// If true, Resource Timing API information will be collected for requests made by GeoJSON and Vector Tile web workers.
    /// This data is returned in a <c>resourceTiming</c> property of relevant data events. Default is false.
    /// </summary>
    [JsonPropertyName("collectResourceTiming")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CollectResourceTiming { get; set; }

    /// <summary>
    /// The HTML element or its string ID in which the map will be rendered.
    /// The specified element must have no children.
    /// </summary>
    [JsonPropertyName("container")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Container { get; set; }

    /// <summary>
    /// Enables cooperative gesture handling, limiting map interaction unless specific gestures are used.
    /// <list type="table">
    ///   <item>
    ///     <term>Desktop</term>
    ///     <description>Interaction requires holding Command/Ctrl key.</description>
    ///   </item>
    ///   <item>
    ///     <term>Mobile</term>
    ///     <description>Interaction requires two fingers.</description>
    ///   </item>
    ///   <item>
    ///     <term>Pitch</term>
    ///     <description>Requires three-finger gesture.</description>
    ///   </item>
    /// </list>
    /// Default is false.
    /// </summary>
    [JsonPropertyName("cooperativeGestures")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? CooperativeGestures { get; set; }

    /// <summary>
    /// If true, symbols from multiple sources can collide with each other during collision detection.
    /// If false, collision detection is run separately per source. Default is true.
    /// </summary>
    [JsonPropertyName("crossSourceCollisions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CrossSourceCollisions { get; set; }

    /// <summary>
    /// Enables or disables the \"double click to zoom\" interaction.
    /// Default is true.
    /// </summary>
    [JsonPropertyName("doubleClickZoom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DoubleClickZoom { get; set; }

    /// <summary>
    /// Enables or disables the \"drag to pan\" interaction. Default is true.
    /// May also be configured via <see cref="DragPanOptions"/> for advanced behavior.
    /// </summary>
    [JsonPropertyName("dragPan")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? DragPan { get; set; }

    /// <summary>
    /// Enables or disables the \"drag to rotate\" interaction.
    /// Default is true.
    /// </summary>
    [JsonPropertyName("dragRotate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DragRotate { get; set; }

    /// <summary>
    /// Sets the elevation (in meters above sea level) of the initial centerpoint of the map.
    /// Defaults to 0 if not specified.
    /// </summary>
    [JsonPropertyName("elevation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Elevation { get; set; }

    /// <summary>
    /// Duration in milliseconds of the fade-in/fade-out animation for label collisions after the initial load.
    /// This affects all symbol layers and not runtime styling or raster tile transitions. Default is 300.
    /// </summary>
    [JsonPropertyName("fadeDuration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? FadeDuration { get; set; }

    /// <summary>
    /// Options used when fitting the initial bounds provided to the map.
    /// </summary>
    [JsonPropertyName("fitBoundsOptions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? FitBoundsOptions { get; set; }

    /// <summary>
    /// Syncs the map’s position with the URL hash fragment.
    /// <list type="table">
    ///   <item>
    ///     <term>true</term>
    ///     <description>The full map state (zoom, center, bearing, pitch) is stored in the URL hash.</description>
    ///   </item>
    ///   <item>
    ///     <term>string</term>
    ///     <description>Use a custom parameter name (e.g., \"map\") for storing map state in the hash.</description>
    ///   </item>
    ///   <item>
    ///     <term>false</term>
    ///     <description>Disables URL hash syncing (default).</description>
    ///   </item>
    /// </list>
    /// </summary>
    [JsonPropertyName("hash")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Hash { get; set; }

    /// <summary>
    /// If false, disables all mouse, touch, and keyboard interactions with the map.
    /// Default is true.
    /// </summary>
    [JsonPropertyName("interactive")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Interactive { get; set; }

    /// <summary>
    /// Enables keyboard shortcuts for map interaction.
    /// Default is true.
    /// </summary>
    [JsonPropertyName("keyboard")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Keyboard { get; set; }

    /// <summary>
    /// A localization override object mapping string IDs to translated UI strings,
    /// used to patch or replace the default UI localization table.
    /// </summary>
    [JsonPropertyName("locale")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, string>? Locale { get; set; }

    /// <summary>
    /// Defines a CSS font-family to locally generate Chinese, Japanese, and Korean characters.
    /// Set to <c>false</c> to disable local override and use style-defined fonts.
    /// Default is 'sans-serif'.
    /// </summary>
    [JsonPropertyName("localIdeographFontFamily")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? LocalIdeographFontFamily { get; set; }

    /// <summary>
    /// Specifies the position of the MapLibre logo on the map.
    /// Valid options: top-left, top-right, bottom-left, bottom-right.
    /// Default is 'bottom-left'.
    /// </summary>
    [JsonPropertyName("logoPosition")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LogoPosition { get; set; }

    /// <summary>
    /// If true, the MapLibre logo will be displayed.
    /// </summary>
    [JsonPropertyName("maplibreLogo")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? MaplibreLogo { get; set; }

    /// <summary>
    /// Sets the maximum geographical bounds of the map. The map will be constrained to these bounds.
    /// </summary>
    [JsonPropertyName("maxBounds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LngLatBounds? MaxBounds { get; set; }

    /// <summary>
    /// Specifies the maximum canvas size as [width, height]. Avoid exceeding WebGL MAX_TEXTURE_SIZE.
    /// Default is [4096, 4096].
    /// </summary>
    [JsonPropertyName("maxCanvasSize")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int[]? MaxCanvasSize { get; set; }

    /// <summary>
    /// The maximum pitch of the map in degrees (0 to 180). Default is 60.
    /// </summary>
    [JsonPropertyName("maxPitch")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? MaxPitch { get; set; }

    /// <summary>
    /// The maximum number of tiles stored in the tile cache for a source. If null, the cache is dynamically sized.
    /// </summary>
    [JsonPropertyName("maxTileCacheSize")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxTileCacheSize { get; set; }

    /// <summary>
    /// The maximum number of zoom levels for which tiles are stored per source.
    /// The cache size is calculated as maxTileCacheZoomLevels × tile count in the current viewport.
    /// Default is 5.
    /// </summary>
    [JsonPropertyName("maxTileCacheZoomLevels")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxTileCacheZoomLevels { get; set; }

    /// <summary>
    /// The maximum zoom level of the map (0–24). Default is 22.
    /// </summary>
    [JsonPropertyName("maxZoom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? MaxZoom { get; set; }

    /// <summary>
    /// The minimum pitch (tilt) of the map in degrees (0–180). Default is 0.
    /// </summary>
    [JsonPropertyName("minPitch")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? MinPitch { get; set; }

    /// <summary>
    /// The minimum zoom level of the map (0–24). Default is 0.
    /// </summary>
    [JsonPropertyName("minZoom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? MinZoom { get; set; }

    /// <summary>
    /// The initial pitch (tilt) of the map, in degrees away from the screen (0–85). Default is 0.
    /// If not set, it is inherited from the style object. Experimental above 60 degrees.
    /// </summary>
    [JsonPropertyName("pitch")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Pitch { get; set; }

    /// <summary>
    /// Enables or disables pitch (tilt) control via the \"drag to rotate\" interaction. Default is true.
    /// </summary>
    [JsonPropertyName("pitchWithRotate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? PitchWithRotate { get; set; }

    /// <summary>
    /// The pixel ratio used to scale the canvas resolution.
    /// Defaults to <c>devicePixelRatio</c> if not specified.
    /// </summary>
    [JsonPropertyName("pixelRatio")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? PixelRatio { get; set; }

    /// <summary>
    /// If false, expired tiles will not be re-requested according to HTTP cache headers. Default is true.
    /// </summary>
    [JsonPropertyName("refreshExpiredTiles")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? RefreshExpiredTiles { get; set; }

    /// <summary>
    /// If true, the map will render multiple copies of the world side by side beyond -180/180 longitude.
    /// If false, blank space will appear and features may be cut across the dateline. Default is true.
    /// </summary>
    [JsonPropertyName("renderWorldCopies")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? RenderWorldCopies { get; set; }

    /// <summary>
    /// The initial roll angle of the map in degrees, measured counter-clockwise about the camera boresight. Default is 0.
    /// </summary>
    [JsonPropertyName("roll")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Roll { get; set; }

    /// <summary>
    /// If false, disables roll control via \"drag to rotate\" interaction. Default is false.
    /// </summary>
    [JsonPropertyName("rollEnabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? RollEnabled { get; set; }

    /// <summary>
    /// Enables \"scroll to zoom\" interaction, or provides options for zooming behavior.
    /// Default is true.
    /// </summary>
    [JsonPropertyName("scrollZoom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? ScrollZoom { get; set; }

    /// <summary>
    /// The MapLibre style, either as a JSON URL or a full style object.
    /// When not specified, it will use the same style as on the demo maps.
    /// This can be overwritten with <see cref="MapLibre.SetStyle"/> before rendering.
    /// </summary>
    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Style { get; set; } = "https://demotiles.maplibre.org/style.json";

    /// <summary>
    /// Enables \"drag to pitch\" interaction, or provides options for pitch behavior.
    /// Default is true.
    /// </summary>
    [JsonPropertyName("touchPitch")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? TouchPitch { get; set; }

    /// <summary>
    /// Enables \"pinch to rotate and zoom\" interaction, or provides options for gesture behavior.
    /// Default is true.
    /// </summary>
    [JsonPropertyName("touchZoomRotate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? TouchZoomRotate { get; set; }

    /// <summary>
    /// If true, the map will automatically resize when the window resizes. Default is true.
    /// </summary>
    [JsonPropertyName("trackResize")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? TrackResize { get; set; }

    /// <summary>
    /// A callback executed before camera changes due to user input or animations.
    /// Can modify center, zoom, pitch, and bearing before they're applied.
    /// </summary>
    [JsonPropertyName("transformCameraUpdate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? TransformCameraUpdate { get; set; }

    /// <summary>
    /// A callback executed before external URL requests are made. Allows modifying URL, headers, or credentials.
    /// </summary>
    [JsonPropertyName("transformRequest")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? TransformRequest { get; set; }

    /// <summary>
    /// If false, style validation is skipped. Useful for production environments. Default is true.
    /// </summary>
    [JsonPropertyName("validateStyle")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ValidateStyle { get; set; }

    /// <summary>
    /// The initial zoom level of the map. If not specified, it is inherited from the style or defaults to 0.
    /// </summary>
    [JsonPropertyName("zoom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Zoom { get; set; }
  
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
