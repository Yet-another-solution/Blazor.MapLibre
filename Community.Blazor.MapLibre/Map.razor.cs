using System.Collections.Concurrent;
using System.Text.Json;
using Community.Blazor.MapLibre.Models;
using Community.Blazor.MapLibre.Models.Camera;
using Community.Blazor.MapLibre.Models.Control;
using Community.Blazor.MapLibre.Models.Image;
using Community.Blazor.MapLibre.Models.Source;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Community.Blazor.MapLibre;

public partial class Map : ComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Provides access to the JavaScript runtime environment for executing interop calls.
    /// Used to interact with JavaScript modules and invoke JavaScript functions.
    /// </summary>
    [Inject]
    private IJSRuntime JsRuntime { get; set; } = null!;

    /// <summary>
    /// The HTML element in which MapLibre GL JS will render the map, or the element's string id.
    /// The specified element must have no children.
    /// </summary>
    [Parameter]
    public string MapId { get; set; } = $"map-{Guid.NewGuid()}";

    /// <summary>
    /// Specifies the width of the map component. Can be set using valid CSS width values (e.g., "100%", "500px").
    /// Defaults to "100%".
    /// </summary>
    [Parameter]
    public string Width { get; set; } = "100%";

    /// <summary>
    /// Specifies the height of the map component.
    /// Accepts values in CSS units (e.g., "500px", "100%") to determine the vertical size of the map.
    /// </summary>
    [Parameter]
    public string Height { get; set; } = "500px";

    /// <summary>
    /// Callback event that is triggered when the map completes loading.
    /// Allows users to execute custom logic upon the successful initialization of the map.
    /// </summary>
    [Parameter]
    public EventCallback<EventArgs> OnLoad { get; set; }

    /// <summary>
    /// Represents the configuration options used to initialize a MapLibre map.
    /// These options allow customization of various map properties such as style, zoom, center, and interactions.
    /// </summary>
    [Parameter]
    public MapOptions Options { get; set; } = new();

    /// <summary>
    /// Represents the JavaScript module reference used to interact with the MapLibre map instance in the Blazor component.
    /// This is dynamically loaded and utilized to invoke JavaScript functions for map initialization and operations.
    /// </summary>
    private IJSObjectReference _jsModule = null!;

    /// <summary>
    /// Manages a thread-safe dictionary for storing references to .NET object instances
    /// used in JavaScript interop callbacks. Each reference is identified by a unique Guid.
    /// </summary>
    private readonly ConcurrentDictionary<Guid, DotNetObjectReference<CallbackHandler>> _references = new ();

    /// <summary>
    /// Encapsulates a reference to the current .NET instance of the Map component, enabling JavaScript interop calls
    /// to invoke methods on the .NET object.
    /// Used to facilitate communication between JavaScript and the .NET component.
    /// </summary>
    private DotNetObjectReference<Map> _dotNetObjectReference = null!;

    /// <summary>
    /// Optional CSS class names. If given, these will be included in the class attribute of the component.
    /// </summary>
    [Parameter]
    public virtual string? Class { get; set; } = null;

    /// <summary>
    /// Invokes the OnLoad event callback when the map component has fully loaded.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [JSInvokable]
    public async Task OnLoadCallback()
    {
        await OnLoad.InvokeAsync(EventArgs.Empty);
    }

    #region Setup

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JsRuntime.InvokeAsync<IJSObjectReference>("import",
                "https://unpkg.com/maplibre-gl@^5.0.0/dist/maplibre-gl.js");
            // Import your JavaScript module
            _jsModule = await JsRuntime.InvokeAsync<IJSObjectReference>("import",
                "./_content/Community.Blazor.MapLibre/Map.razor.js");

            _dotNetObjectReference = DotNetObjectReference.Create(this);
            // Just making sure the Container is being seeded on Create
            Options.Container = MapId;
            // Initialize the MapLibre map
            await _jsModule.InvokeVoidAsync("MapInterop.initializeMap", Options, _dotNetObjectReference);
        }
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var value in _references.Values)
        {
            value.Dispose();
        }

        await _jsModule.DisposeAsync();
    }

    #endregion

    #region Events

    /// <summary>
    /// Registers an event listener for a specified event on the map, optionally scoped to a specific layer.
    /// </summary>
    /// <typeparam name="T">The type of the event payload.</typeparam>
    /// <param name="eventName">The name of the event to listen for (e.g., "click", "mousemove").</param>
    /// <param name="handler">The callback action to execute when the event occurs.</param>
    /// <param name="layer">The optional layer ID where the event listener should be applied.</param>
    /// <returns>A <see cref="Listener"/> instance that allows removal of the registered listener.</returns>
    public async Task<Listener> AddListener<T>(string eventName, Action<T> handler, object? layer = null)
    {
        var callback = new CallbackHandler(_jsModule, eventName, handler, typeof(T));
        var reference = DotNetObjectReference.Create(callback);
        _references.TryAdd(Guid.NewGuid(), reference);
        
        await _jsModule.InvokeVoidAsync("MapInterop.on", MapId, eventName, reference, layer);

        return new Listener(callback);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Adds a control to the map instance based on the specified control type and options.
    /// </summary>
    /// <param name="controlType">The type of control to be added to the map.</param>
    /// <param name="options">Optional settings or parameters specific to the control being added.</param>
    /// <returns>A task that represents the asynchronous operation of adding the control.</returns>
    public async ValueTask AddControl(ControlType controlType, object? options = null) =>
        await _jsModule.InvokeVoidAsync("MapInterop.addControl", MapId, controlType.ToString(), options);

    /// <summary>
    /// Adds an image to the map for use in styling or layer configuration.
    /// </summary>
    /// <param name="id">The unique identifier for the image to be added to the map.</param>
    /// <param name="url">The URL pointing to the image resource to be added.</param>
    /// <param name="options">Optional parameters to configure the image, such as pixel ratio or content layout.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask AddImage(string id, string url, object? options = null) =>
        await _jsModule.InvokeVoidAsync("MapInterop.addImage", MapId, id, url, options);

    /// <summary>
    /// Adds a layer to the MapLibre map with the specified properties and an optional position before another layer.
    /// </summary>
    /// <param name="layer">The layer to be added, defining the rendering and customization options.</param>
    /// <param name="beforeId">An optional layer ID indicating the position before which the new layer should be added. If null, the layer is added to the end.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask AddLayer(Layer layer, string? beforeId = null) =>
        await _jsModule.InvokeVoidAsync("MapInterop.addLayer", MapId, layer, beforeId);

    /// <summary>
    /// Adds a source to the map with the specified identifier and source object.
    /// </summary>
    /// <param name="id">A unique identifier for the source.</param>
    /// <param name="source">The source object to add to the map, implementing the <see cref="ISource"/> interface.</param>
    /// <returns>A task that represents the asynchronous operation of adding the source.</returns>
    public async ValueTask AddSource(string id, ISource source) =>
        await _jsModule.InvokeVoidAsync("MapInterop.addSource", MapId, id, source);

    /// <summary>
    /// Adds a sprite to the map using the specified sprite id, URL, and optional configuration.
    /// </summary>
    /// <param name="id">The unique identifier for the sprite to be added.</param>
    /// <param name="url">The URL of the sprite image to be loaded.</param>
    /// <param name="options">Optional parameters to configure the sprite.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask AddSprite(string id, string url, object? options = null) =>
        await _jsModule.InvokeVoidAsync("MapInterop.addSprite", MapId, id, url, options);

    /// <summary>
    /// Determines whether all map tiles have been fully loaded.
    /// </summary>
    /// <returns>A task that resolves to a boolean indicating whether the tiles are completely loaded.</returns>
    public async ValueTask<bool> AreTilesLoaded() =>
        await _jsModule.InvokeAsync<bool>("MapInterop.areTilesLoaded", MapId);

    /// <summary>
    /// Calculates and returns camera options based on the provided longitude and latitude coordinates,
    /// altitude, and rotation parameters including bearing, pitch, and optional roll.
    /// </summary>
    /// <param name="cameraLngLat">The geographic longitude and latitude coordinates of the camera.</param>
    /// <param name="cameraAltitude">The altitude of the camera in meters.</param>
    /// <param name="bearing">The compass direction that the camera is facing, in degrees.</param>
    /// <param name="pitch">The tilt of the camera, in degrees from the horizontal plane.</param>
    /// <param name="roll">Optional roll angle of the camera, in degrees (rotation along the view vector).</param>
    /// <returns>A <see cref="CameraOptions"/> object containing the calculated camera options, including position, zoom, and rotation.</returns>
    public async ValueTask<CameraOptions> CalculateCameraOptionsFromCameraLngLatAltRotation(LngLat cameraLngLat,
        double cameraAltitude, double bearing, double pitch, double? roll = null) =>
        await _jsModule.InvokeAsync<CameraOptions>("MapInterop.calculateCameraOptionsFromCameraLngLatAltRotation", MapId, cameraLngLat, cameraAltitude, bearing, pitch, roll);

    /// <summary>
    /// Calculates the camera options to transition from one location to another, considering their respective altitudes.
    /// </summary>
    /// <param name="from">The starting geographical coordinates.</param>
    /// <param name="altitudeFrom">The altitude at the starting location.</param>
    /// <param name="to">The destination geographical coordinates.</param>
    /// <param name="altitudeTo">The altitude at the destination location. This parameter is optional.</param>
    /// <returns>A task representing the asynchronous operation that provides the calculated CameraOptions.</returns>
    public async ValueTask<CameraOptions> CalculateCameraOptionsFromTo(LngLat from, double altitudeFrom, LngLat to,
        double? altitudeTo = null) =>
        await _jsModule.InvokeAsync<CameraOptions>("MapInterop.calculateCameraOptionsFromTo", MapId, from, altitudeFrom, to, altitudeTo);

    /// <summary>
    /// Computes the required center, zoom, and bearing to fit the specified bounding box within the viewport.
    /// </summary>
    /// <param name="bounds">The geographical bounding box to be fitted.</param>
    /// <param name="options">Optional parameters to customize the calculation.</param>
    /// <returns>A task that represents the asynchronous operation, containing the resulting center, zoom, and bearing.</returns>
    public async ValueTask<CenterZoomBearing> CameraForBounds(LngLatBounds bounds, object? options = null) =>
        await _jsModule.InvokeAsync<CenterZoomBearing>("MapInterop.cameraForBounds", MapId, bounds, options);

    /// <summary>
    /// Smoothly transitions the camera's view to the specified target, animating parameters such as
    /// center, zoom, bearing, pitch, roll, and padding. Any unspecified parameters will retain their current values.
    /// </summary>
    /// <remarks>
    /// The transition is animated unless the user has enabled the "reduced motion" accessibility feature 
    /// in their operating system. This can be overridden by including <c>essential: true</c> in the options.
    /// </remarks>
    /// <param name="options">
    /// The options describing the destination and animation behavior. Accepts both camera and animation-related properties.
    /// </param>
    /// <param name="eventData">
    /// Additional data to be included in events triggered during the transition (e.g., move, zoom, rotate events).
    /// </param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask EaseTo(EaseToOptions options, object? eventData = null) =>
        await _jsModule.InvokeVoidAsync("MapInterop.easeTo", MapId, options, eventData);

    /// <summary>
    /// Pans and zooms the map to contain its visible area within the specified geographical bounds. This function will also reset the map's bearing to 0 if bearing is nonzero.
    /// </summary>
    /// <param name="bounds">The geographical bounds to fit within the viewport.</param>
    /// <param name="options">Options to customize the behavior of the fit bounds operation.</param>
    /// <param name="eventData">Additional event data associated with the operation, if any.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask FitBounds(LngLatBounds bounds, FitBoundOptions? options = null, object? eventData = null) =>
        await _jsModule.InvokeVoidAsync("MapInterop.fitBounds", MapId, bounds, options, eventData);

    /// <summary>
    /// Pans, rotates, and zooms the map to fit the bounding box formed by two given screen points 
    /// after rotating the map to the specified bearing. If the current map bearing is passed, the map will
    /// zoom without rotating.
    /// </summary>
    /// <remarks>
    /// Triggers the following events during the animation lifecycle: <c>movestart</c>, <c>move</c>, <c>moveend</c>, 
    /// <c>zoomstart</c>, <c>zoom</c>, <c>zoomend</c>, and <c>rotate</c>.
    /// </remarks>
    /// <param name="p0">The first screen point, specified in pixel coordinates.</param>
    /// <param name="p1">The second screen point, specified in pixel coordinates.</param>
    /// <param name="bearing">The desired final map bearing, in degrees, for the animation.</param>
    /// <param name="options">Optional parameters to customize the animation behavior and padding.</param>
    /// <param name="eventData">Additional data to include with the triggered animation events.</param>
    /// <example>
    /// <code>
    /// var p0 = new PointLike(220, 400);
    /// var p1 = new PointLike(500, 900);
    /// await map.FitScreenCoordinates(p0, p1, map.GetBearing(), new FitBoundOptions
    /// {
    ///     Padding = new PaddingOptions { Top = 10, Bottom = 25, Left = 15, Right = 5 }
    /// });
    /// </code>
    /// </example>
    public async ValueTask FitScreenCoordinates(PointLike p0, PointLike p1, double bearing,
        FitBoundOptions? options = null, object? eventData = null) =>
        await _jsModule.InvokeVoidAsync("MapInterop.fitScreenCoordinates", MapId, p0, p1, bearing, options, eventData);

    /// <summary>
    /// Smoothly transitions the map by animating changes to the center, zoom, bearing, pitch, and roll properties. 
    /// The animation follows a flight-like curve, incorporating zooming and panning to maintain orientation over large distances.
    /// </summary>
    /// <remarks>
    /// Triggers the following events during the animation lifecycle: <c>movestart</c>, <c>move</c>, <c>moveend</c>, 
    /// <c>zoomstart</c>, <c>zoom</c>, <c>zoomend</c>, <c>pitchstart</c>, <c>pitch</c>, <c>pitchend</c>, <c>rollstart</c>, 
    /// <c>roll</c>, <c>rollend</c>, and <c>rotate</c>. The animation will be skipped and instead transition 
    /// immediately if the user’s operating system has the ‘reduced motion’ accessibility feature enabled, unless the 
    /// <paramref name="options"/> object includes <c>essential: true</c>.
    /// </remarks>
    /// <param name="options">Describes the animation destination and transition behavior. Includes camera and animation properties.</param>
    /// <param name="eventData">Additional data to include with triggered animation events.</param>
    /// <example>
    /// <code>
    /// // Fly to a specific location with default duration and easing.
    /// await map.FlyTo(new FlyToOptions { Center = new LngLat(0, 0), Zoom = 9 });
    ///
    /// // Customize the flight animation with specific options.
    /// await map.FlyTo(new FlyToOptions
    /// {
    ///     Center = new LngLat(0, 0),
    ///     Zoom = 9,
    ///     Speed = 0.2,
    ///     Curve = 1,
    ///     Easing = t => t
    /// });
    /// </code>
    /// </example>
    public async ValueTask FlyTo(FlyToOptions options, object? eventData = null) =>
        await _jsModule.InvokeVoidAsync("MapInterop.flyTo", MapId, options, eventData);

    /// <summary>
    /// Gets the bearing of the map's current view direction.
    /// </summary>
    /// <returns>Returns the map's current bearing, a value in degrees.</returns>
    public async ValueTask<double> GetBearing() =>
        await _jsModule.InvokeAsync<double>("MapInterop.getBearing", MapId);

    /// <summary>
    /// Gets the geographical bounds visible in the current viewport.
    /// </summary>
    /// <returns>The <see cref="LngLatBounds"/> object representing the visible geographical bounds.</returns>
    public async ValueTask<LngLatBounds> GetBounds() =>
        await _jsModule.InvokeAsync<LngLatBounds>("MapInterop.getBounds", MapId);

    /// <summary>
    /// Gets the elevation of the camera target with respect to the terrain.
    /// </summary>
    /// <returns>The elevation of the center point in meters.</returns>
    public async ValueTask<double> GetCameraTargetElevation() =>
        await _jsModule.InvokeAsync<double>("MapInterop.getCameraTargetElevation", MapId);

    /// <summary>
    /// Gets a reference to the map's HTML canvas element.
    /// </summary>
    /// <returns>A JSObjectReference representing the canvas element.</returns>
    public async ValueTask<IJSObjectReference> GetCanvas() =>
        await _jsModule.InvokeAsync<IJSObjectReference>("MapInterop.getCanvas", MapId);

    /// <summary>
    /// Gets the container of the map's canvas element.
    /// </summary>
    /// <returns>A JSObjectReference representing the canvas container.</returns>
    public async ValueTask<IJSObjectReference> GetCanvasContainer() =>
        await _jsModule.InvokeAsync<IJSObjectReference>("MapInterop.getCanvasContainer", MapId);

    /// <summary>
    /// Gets the geographical center of the current map view.
    /// </summary>
    /// <returns>A <see cref="LngLat"/> representing the center of the viewport.</returns>
    public async ValueTask<LngLat> GetCenter() =>
        await _jsModule.InvokeAsync<LngLat>("MapInterop.getCenter", MapId);

    /// <summary>
    /// Returns the value of centerClampedToGround.
    /// If true, the elevation of the center point will automatically be set to the terrain elevation (or zero if
    /// terrain is not enabled). If false, the elevation of the center point will default to sea level and will not
    /// automatically update. Defaults to true. Needs to be set to false to keep the camera above ground when pitch > 90 degrees.
    /// </summary>
    /// <returns></returns>
    public async ValueTask<bool> GetCenterClampedToGround() =>
        await _jsModule.InvokeAsync<bool>("MapInterop.getCenterClampedToGround", MapId);

    /// <summary>
    /// Returns the elevation of the map's center point.
    /// </summary>
    /// <returns></returns>
    public async ValueTask<double> GetCenterElevation() =>
        await _jsModule.InvokeAsync<double>("MapInterop.getCenterElevation", MapId);

    /// <summary>
    /// Returns the map's containing HTML element.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, resulting in a JavaScript object reference to the container element.</returns>
    public async ValueTask<IJSObjectReference> GetContainer() =>
        await _jsModule.InvokeAsync<IJSObjectReference>("MapInterop.getContainer", MapId);

    /// <summary>
    /// Gets the state of a feature. A feature's state is a set of user-defined key-value pairs that are assigned to a
    /// feature at runtime. Features are identified by their feature.id attribute, which can be any number or string.
    /// Note: To access the values in a feature's state object for the purposes of styling the feature, use the feature-state expression.
    /// </summary>
    /// <param name="feature">The feature whose state is to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation, with the result containing the state of the feature as an object.</returns>
    public async ValueTask<object> GetFeatureState(object feature) =>
        await _jsModule.InvokeAsync<object>("MapInterop.getFeatureState", MapId, feature);

    /// <summary>
    /// Returns the filter applied to the specified style layer.
    /// </summary>
    /// <param name="layerId"></param>
    /// <returns></returns>
    public async ValueTask<string> GetFilter(string layerId) =>
        await _jsModule.InvokeAsync<string>("MapInterop.getFilter", MapId, layerId);

    /// <summary>
    /// Returns the value of the style's glyphs URL
    /// </summary>
    /// <returns></returns>
    public async ValueTask<string> GetGlyphs() =>
        await _jsModule.InvokeAsync<string>("MapInterop.getGlyphs", MapId);

    /// <summary>
    /// Returns an image, specified by ID, currently available in the map. This includes both images from the style's
    /// original sprite and any images that have been added at runtime using Map#addImage.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async ValueTask<string> GetImage(string id) =>
        await _jsModule.InvokeAsync<string>("MapInterop.getImage", MapId, id);

    /// <summary>
    /// Returns the layer with the specified ID in the map's style.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async ValueTask<object> GetLayer(string id) =>
        await _jsModule.InvokeAsync<object>("MapInterop.getLayer", MapId, id);

    /// <summary>
    /// Return the ids of all layers currently in the style, including custom layers, in order.
    /// </summary>
    /// <returns></returns>
    public async ValueTask<string[]> GetLayersOrder() =>
        await _jsModule.InvokeAsync<string[]>("MapInterop.getLayersOrder", MapId);

    /// <summary>
    /// Returns the value of a layout property in the specified style layer.
    /// </summary>
    /// <param name="layerId"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public async ValueTask<object> GetLayoutProperty(string layerId, string name) =>
        await _jsModule.InvokeAsync<object>("MapInterop.getLayoutProperty", MapId, layerId, name);
    
    /// <summary>
    /// Retrieves the current light settings of the map.
    /// </summary>
    /// <returns>An object representing the map's light settings.</returns>
    public async ValueTask<object> GetLight() =>
        await _jsModule.InvokeAsync<object>("MapInterop.getLight", MapId);

    /// <summary>
    /// Retrieves the maximum geographical bounds the map is constrained to.
    /// </summary>
    /// <returns>An object representing the map's maximum bounds or null if not set.</returns>
    public async ValueTask<object?> GetMaxBounds() =>
        await _jsModule.InvokeAsync<object?>("MapInterop.getMaxBounds", MapId);

    /// <summary>
    /// Retrieves the map's maximum allowable pitch.
    /// </summary>
    /// <returns>The maximum allowable pitch in degrees.</returns>
    public async ValueTask<double> GetMaxPitch() =>
        await _jsModule.InvokeAsync<double>("MapInterop.getMaxPitch", MapId);

    /// <summary>
    /// Retrieves the map's maximum allowable zoom level.
    /// </summary>
    /// <returns>The maximum zoom level allowed by the map.</returns>
    public async ValueTask<double> GetMaxZoom() =>
        await _jsModule.InvokeAsync<double>("MapInterop.getMaxZoom", MapId);

    /// <summary>
    /// Retrieves the map's minimum allowable pitch.
    /// </summary>
    /// <returns>The minimum allowable pitch in degrees.</returns>
    public async ValueTask<double> GetMinPitch() =>
        await _jsModule.InvokeAsync<double>("MapInterop.getMinPitch", MapId);

    /// <summary>
    /// Retrieves the map's minimum allowable zoom level.
    /// </summary>
    /// <returns>The minimum zoom level allowed by the map.</returns>
    public async ValueTask<double> GetMinZoom() =>
        await _jsModule.InvokeAsync<double>("MapInterop.getMinZoom", MapId);

    /// <summary>
    /// Retrieves the current padding applied to the map's viewport.
    /// </summary>
    /// <returns>An object representing padding options applied to the map.</returns>
    public async ValueTask<object> GetPadding() =>
        await _jsModule.InvokeAsync<object>("MapInterop.getPadding", MapId);

    /// <summary>
    /// Retrieves the value of a specific paint property of a specified layer.
    /// </summary>
    /// <param name="layerId">The ID of the layer to get the paint property from.</param>
    /// <param name="name">The name of the paint property.</param>
    /// <returns>The value of the specified paint property.</returns>
    public async ValueTask<object?> GetPaintProperty(string layerId, string name) =>
        await _jsModule.InvokeAsync<object?>("MapInterop.getPaintProperty", MapId, layerId, name);

    /// <summary>
    /// Retrieves the current pitch (tilt) of the map in degrees.
    /// </summary>
    /// <returns>The map's current pitch value.</returns>
    public async ValueTask<double> GetPitch() =>
        await _jsModule.InvokeAsync<double>("MapInterop.getPitch", MapId);

    /// <summary>
    /// Retrieves the map's pixel ratio.
    /// </summary>
    /// <returns>The pixel ratio of the map.</returns>
    public async ValueTask<double> GetPixelRatio() =>
        await _jsModule.InvokeAsync<double>("MapInterop.getPixelRatio", MapId);

    /// <summary>
    /// Retrieves the projection specification of the map.
    /// </summary>
    /// <returns>An object representing the map's projection specification.</returns>
    public async ValueTask<object> GetProjection() =>
        await _jsModule.InvokeAsync<object>("MapInterop.getProjection", MapId);

    /// <summary>
    /// Returns the state of whether multiple world copies are rendered or not.
    /// </summary>
    /// <returns>True if multiple world copies are rendered; otherwise, false.</returns>
    public async ValueTask<bool> GetRenderWorldCopies() =>
        await _jsModule.InvokeAsync<bool>("MapInterop.getRenderWorldCopies", MapId);

    /// <summary>
    /// Retrieves the current roll angle of the map in degrees.
    /// </summary>
    /// <returns>The current roll value of the map.</returns>
    public async ValueTask<double> GetRoll() =>
        await _jsModule.InvokeAsync<double>("MapInterop.getRoll", MapId);

    /// <summary>
    /// Retrieves the sky properties applied to the map style.
    /// </summary>
    /// <returns>An object representing the sky properties of the map.</returns>
    public async ValueTask<object?> GetSky() =>
        await _jsModule.InvokeAsync<object?>("MapInterop.getSky", MapId);

    /// <summary>
    /// Retrieves a source from the map's style by its ID.
    /// </summary>
    /// <param name="id">The ID of the source to retrieve.</param>
    /// <returns>The source object if found, or null if not found.</returns>
    public async ValueTask<object?> GetSource(string id) =>
        await _jsModule.InvokeAsync<object?>("MapInterop.getSource", MapId, id);

    /// <summary>
    /// Retrieves the style's sprite as a list of objects.
    /// </summary>
    /// <returns>A list of objects representing the style's sprite.</returns>
    public async ValueTask<object[]> GetSprite() =>
        await _jsModule.InvokeAsync<object[]>("MapInterop.getSprite", MapId);
    
    /// <summary>
    /// Retrieves the map's style specification.
    /// </summary>
    /// <returns>An object representing the style specification of the map.</returns>
    public async ValueTask<object> GetStyle() =>
        await _jsModule.InvokeAsync<object>("MapInterop.getStyle", MapId);

    /// <summary>
    /// Retrieves the terrain options if terrain is loaded.
    /// </summary>
    /// <returns>An object representing terrain options, or null if not loaded.</returns>
    public async ValueTask<object?> GetTerrain() =>
        await _jsModule.InvokeAsync<object?>("MapInterop.getTerrain", MapId);

    /// <summary>
    /// Retrieves the map's current vertical field of view in degrees.
    /// </summary>
    /// <returns>The map's vertical field of view in degrees.</returns>
    public async ValueTask<double> GetVerticalFieldOfView() =>
        await _jsModule.InvokeAsync<double>("MapInterop.getVerticalFieldOfView", MapId);

    /// <summary>
    /// Retrieves the map's current zoom level.
    /// </summary>
    /// <returns>The current zoom level of the map.</returns>
    public async ValueTask<double> GetZoom() =>
        await _jsModule.InvokeAsync<double>("MapInterop.getZoom", MapId);

    /// <summary>
    /// Checks if a specific control exists on the map.
    /// </summary>
    /// <param name="control">The control instance to check for.</param>
    /// <returns>True if the control exists on the map; otherwise, false.</returns>
    public async ValueTask<bool> HasControl(object control) =>
        await _jsModule.InvokeAsync<bool>("MapInterop.hasControl", MapId, control);

    /// <summary>
    /// Checks whether a specific image ID exists in the map's style.
    /// </summary>
    /// <param name="id">The image ID to check.</param>
    /// <returns>True if the image exists; otherwise, false.</returns>
    public async ValueTask<bool> HasImage(string id) =>
        await _jsModule.InvokeAsync<bool>("MapInterop.hasImage", MapId, id);

    /// <summary>
    /// Determines if the map is currently moving.
    /// </summary>
    /// <returns>True if the map is moving; otherwise, false.</returns>
    public async ValueTask<bool> IsMoving() =>
        await _jsModule.InvokeAsync<bool>("MapInterop.isMoving", MapId);

    /// <summary>
    /// Determines if the map is currently rotating.
    /// </summary>
    /// <returns>True if the map is rotating; otherwise, false.</returns>
    public async ValueTask<bool> IsRotating() =>
        await _jsModule.InvokeAsync<bool>("MapInterop.isRotating", MapId);

    /// <summary>
    /// Determines if a source with the given ID is loaded in the map.
    /// </summary>
    /// <param name="id">The ID of the source to check.</param>
    /// <returns>True if the source is loaded; otherwise, false.</returns>
    public async ValueTask<bool> IsSourceLoaded(string id) =>
        await _jsModule.InvokeAsync<bool>("MapInterop.isSourceLoaded", MapId, id);

    /// <summary>
    /// Determines if the map's style is fully loaded.
    /// </summary>
    /// <returns>True if the style is fully loaded; otherwise, false.</returns>
    public async ValueTask<bool> IsStyleLoaded() =>
        await _jsModule.InvokeAsync<bool>("MapInterop.isStyleLoaded", MapId);

    /// <summary>
    /// Determines if the map is currently zooming.
    /// </summary>
    /// <returns>True if the map is zooming; otherwise, false.</returns>
    public async ValueTask<bool> IsZooming() =>
        await _jsModule.InvokeAsync<bool>("MapInterop.isZooming", MapId);

    /// <summary>
    /// Updates the map view by changing the center, zoom, bearing, pitch, or roll without animation.
    /// </summary>
    /// <param name="options">The new view options.</param>
    /// <param name="eventData">Optional event data.</param>
    public async ValueTask JumpTo(object options, object? eventData = null) =>
        await _jsModule.InvokeVoidAsync("MapInterop.jumpTo", MapId, options, eventData);

    /// <summary>
    /// Determines if there are any registered listeners for a given event type on the map.
    /// </summary>
    /// <param name="type">The event type to check.</param>
    /// <returns>True if a listener exists for the given event type; otherwise, false.</returns>
    public async ValueTask<bool> Listens(string type) =>
        await _jsModule.InvokeAsync<bool>("MapInterop.listens", MapId, type);

    /// <summary>
    /// Lists all image IDs available in the map's style.
    /// </summary>
    /// <returns>An array of image IDs available in the style.</returns>
    public async ValueTask<string[]> ListImages() =>
        await _jsModule.InvokeAsync<string[]>("MapInterop.listImages", MapId);

    /// <summary>
    /// Checks if the map is fully loaded.
    /// </summary>
    /// <returns>True if the map is fully loaded; otherwise, false.</returns>
    public async ValueTask<bool> Loaded() =>
        await _jsModule.InvokeAsync<bool>("MapInterop.loaded", MapId);

    /// <summary>
    /// Loads an image from an external URL and returns it.
    /// </summary>
    /// <param name="url">The URL of the image to load.</param>
    /// <returns>An object containing the loaded image.</returns>
    public async ValueTask<object> LoadImage(string url) =>
        await _jsModule.InvokeAsync<object>("MapInterop.loadImage", MapId, url);

    /// <summary>
    /// Moves a layer to a different z-position in the style.
    /// </summary>
    /// <param name="id">The ID of the layer to move.</param>
    /// <param name="beforeId">The ID of the target layer to place the layer before.</param>
    public async ValueTask MoveLayer(string id, string beforeId) =>
        await _jsModule.InvokeVoidAsync("MapInterop.moveLayer", MapId, id, beforeId);

    /// <summary>
    /// Pans the map by a specified offset.
    /// </summary>
    /// <param name="offset">The offset by which to pan the map, in pixels.</param>
    /// <param name="options">Additional pan options (e.g., animation parameters).</param>
    /// <param name="eventData">Optional event data associated with the operation.</param>
    public async ValueTask PanBy(object offset, object? options = null, object? eventData = null) =>
        await _jsModule.InvokeVoidAsync("MapInterop.panBy", MapId, offset, options, eventData);

    /// <summary>
    /// Pans the map to the given geographical location.
    /// </summary>
    /// <param name="lngLat">The target longitude and latitude to pan to.</param>
    /// <param name="options">Additional options (e.g., duration).</param>
    /// <param name="eventData">Optional event data.</param>
    public async ValueTask PanTo(object lngLat, object? options = null, object? eventData = null) =>
        await _jsModule.InvokeVoidAsync("MapInterop.panTo", MapId, lngLat, options, eventData);

    /// <summary>
    /// Projects geographical coordinates to pixel coordinates in the current map view.
    /// </summary>
    /// <param name="lngLat">The geographical coordinates to project.</param>
    /// <returns>The projected point as pixel coordinates.</returns>
    public async ValueTask<object> Project(object lngLat) =>
        await _jsModule.InvokeAsync<object>("MapInterop.project", MapId, lngLat);

    /// <summary>
    /// Queries the map for rendered features within a specified geometry or options.
    /// </summary>
    /// <param name="query">The query geometry or options.</param>
    /// <param name="options">Additional query options (e.g., layer IDs).</param>
    /// <returns>An array of features matching the query.</returns>
    public async ValueTask<object[]> QueryRenderedFeatures(object query, object? options = null) =>
        await _jsModule.InvokeAsync<object[]>("MapInterop.queryRenderedFeatures", MapId, query, options);

    /// <summary>
    /// Queries features from a source.
    /// </summary>
    /// <param name="sourceId">The ID of the source.</param>
    /// <param name="parameters">Query parameters as an object.</param>
    /// <returns>An array of query results.</returns>
    public async ValueTask<object[]> QuerySourceFeatures(string sourceId, object parameters) =>
        await _jsModule.InvokeAsync<object[]>("MapInterop.querySourceFeatures", MapId, sourceId, parameters);

    /// <summary>
    /// Queries terrain elevation at the given location.
    /// </summary>
    /// <param name="lngLat">An array with longitude and latitude coordinates.</param>
    /// <returns>The elevation in meters at the given location.</returns>
    public async ValueTask<double> QueryTerrainElevation(object lngLat) =>
        await _jsModule.InvokeAsync<double>("MapInterop.queryTerrainElevation", MapId, lngLat);

    /// <summary>
    /// Forces a redraw of the map.
    /// </summary>
    public async ValueTask Redraw() =>
        await _jsModule.InvokeVoidAsync("MapInterop.redraw", MapId);

    /// <summary>
    /// Cleans up internal resources associated with the map and removes it.
    /// </summary>
    public async ValueTask Remove()
    {
        await _jsModule.InvokeVoidAsync("MapInterop.remove", MapId);
    }

    /// <summary>
    /// Removes a control from the map.
    /// </summary>
    /// <param name="control">The control to remove.</param>
    public async ValueTask RemoveControl(object control) =>
        await _jsModule.InvokeVoidAsync("MapInterop.removeControl", MapId, control);

    /// <summary>
    /// Removes feature states from the map.
    /// </summary>
    /// <param name="target">The feature or source to remove states from.</param>
    /// <param name="key">The optional key of the state to remove.</param>
    public async ValueTask RemoveFeatureState(object target, string? key = null) =>
        await _jsModule.InvokeVoidAsync("MapInterop.removeFeatureState", MapId, target, key);

    /// <summary>
    /// Removes an image from the map's style by ID.
    /// </summary>
    /// <param name="id">The ID of the image to remove.</param>
    public async ValueTask RemoveImage(string id) =>
        await _jsModule.InvokeVoidAsync("MapInterop.removeImage", MapId, id);

    /// <summary>
    /// Removes a layer from the map by its ID.
    /// </summary>
    /// <param name="id">The ID of the layer to remove.</param>
    public async ValueTask RemoveLayer(string id) =>
        await _jsModule.InvokeVoidAsync("MapInterop.removeLayer", MapId, id);

    /// <summary>
    /// Removes a source from the map's style.
    /// </summary>
    /// <param name="id">The ID of the source to remove.</param>
    public async ValueTask RemoveSource(string id) =>
        await _jsModule.InvokeVoidAsync("MapInterop.removeSource", MapId, id);

    /// <summary>
    /// Removes the sprite from the map's style by ID.
    /// </summary>
    /// <param name="id">The ID of the sprite to remove.</param>
    public async ValueTask RemoveSprite(string id) =>
        await _jsModule.InvokeVoidAsync("MapInterop.removeSprite", MapId, id);

    /// <summary>
    /// Rotates the map to reset north to be up.
    /// </summary>
    /// <param name="options">Animation options.</param>
    /// <param name="eventData">Optional event data.</param>
    public async ValueTask ResetNorth(object? options = null, object? eventData = null) =>
        await _jsModule.InvokeVoidAsync("MapInterop.resetNorth", MapId, options, eventData);

    /// <summary>
    /// Resets the map’s north and pitch angles with an animated transition.
    /// </summary>
    /// <param name="options">Animation options.</param>
    /// <param name="eventData">Optional event data.</param>
    public async ValueTask ResetNorthPitch(object? options = null, object? eventData = null) =>
        await _jsModule.InvokeVoidAsync("MapInterop.resetNorthPitch", MapId, options, eventData);

    /// <summary>
    /// Resizes the map to fit its container dimensions.
    /// </summary>
    /// <param name="eventData">Optional event data.</param>
    /// <param name="constrainTransform">Whether to constrain the transform.</param>
    public async ValueTask Resize(object? eventData = null, bool constrainTransform = true) =>
        await _jsModule.InvokeVoidAsync("MapInterop.resize", MapId, eventData, constrainTransform);

    /// <summary>
    /// Rotates the map to the specified bearing.
    /// </summary>
    /// <param name="bearing">The target bearing.</param>
    /// <param name="options">Optional animation options.</param>
    /// <param name="eventData">Optional event data.</param>
    public async ValueTask RotateTo(double bearing, object? options = null, object? eventData = null) =>
        await _jsModule.InvokeVoidAsync("MapInterop.rotateTo", MapId, bearing, options, eventData);

    /// <summary>
    /// Sets the map's bearing (rotation).
    /// </summary>
    /// <param name="bearing">The bearing in degrees.</param>
    /// <param name="eventData">Optional event data.</param>
    public async ValueTask SetBearing(double bearing, object? eventData = null) =>
        await _jsModule.InvokeVoidAsync("MapInterop.setBearing", MapId, bearing, eventData);

    /// <summary>
    /// Sets the map's geographical center.
    /// </summary>
    /// <param name="center">The geographical center coordinates [longitude, latitude].</param>
    /// <param name="eventData">Optional event data.</param>
    public async ValueTask SetCenter(object center, object? eventData = null) =>
        await _jsModule.InvokeVoidAsync("MapInterop.setCenter", MapId, center, eventData);

    /// <summary>
    /// Sets whether the map's center is clamped to the ground.
    /// </summary>
    /// <param name="centerClampedToGround">Whether to clamp the map's center to the ground.</param>
    public async ValueTask SetCenterClampedToGround(bool centerClampedToGround) =>
        await _jsModule.InvokeVoidAsync("MapInterop.setCenterClampedToGround", MapId, centerClampedToGround);

    /// <summary>
    /// Sets the elevation of the map's center point.
    /// </summary>
    /// <param name="elevation">The elevation in meters.</param>
    /// <param name="eventData">Optional event data.</param>
    public async ValueTask SetCenterElevation(double elevation, object? eventData = null) =>
        await _jsModule.InvokeVoidAsync("MapInterop.setCenterElevation", MapId, elevation, eventData);

    /// <summary>
    /// Updates the state of a specific feature on the map.
    /// </summary>
    /// <param name="feature">The feature identifier object.</param>
    /// <param name="state">The state properties to apply to the feature.</param>
    public async ValueTask SetFeatureState(object feature, object state) =>
        await _jsModule.InvokeVoidAsync("MapInterop.setFeatureState", MapId, feature, state);

    /// <summary>
    /// Sets a zoom level for the map.
    /// </summary>
    /// <param name="zoom">The desired zoom level (0–20).</param>
    /// <param name="eventData">Optional event data.</param>
    public async ValueTask SetZoom(double zoom, object? eventData = null) =>
        await _jsModule.InvokeVoidAsync("MapInterop.setZoom", MapId, zoom, eventData);

    /// <summary>
    /// Adjusts the map's style to a new configuration or URL.
    /// </summary>
    /// <param name="style">The style configuration object or URL.</param>
    /// <param name="options">Optional parameters for the style application.</param>
    public async ValueTask SetStyle(object style, object? options = null) =>
        await _jsModule.InvokeVoidAsync("MapInterop.setStyle", MapId, style, options);

    /// <summary>
    /// Stops any animated transition currently underway on the map.
    /// </summary>
    public async ValueTask Stop() =>
        await _jsModule.InvokeVoidAsync("MapInterop.stop", MapId);

    /// <summary>
    /// Converts pixel coordinates to geographical coordinates.
    /// </summary>
    /// <param name="point">The pixel coordinates [x, y].</param>
    /// <returns>Geographical coordinates [longitude, latitude].</returns>
    public async ValueTask<object> Unproject(object point) =>
        await _jsModule.InvokeAsync<object>("MapInterop.unproject", MapId, point);

    /// <summary>
    /// Updates an existing image in the map's sprite.
    /// </summary>
    /// <param name="id">The image ID.</param>
    /// <param name="image">The new image data to update.</param>
    public async ValueTask UpdateImage(string id, object image)
    {
        await _jsModule.InvokeVoidAsync("MapInterop.updateImage", MapId, id, image);
    }

    /// <summary>
    /// Increases the map's zoom level by 1.
    /// </summary>
    /// <param name="options">Animation options object (optional).</param>
    /// <param name="eventData">Additional event data (optional).</param>
    public async ValueTask ZoomIn(object? options = null, object? eventData = null)
    {
        await _jsModule.InvokeVoidAsync("MapInterop.zoomIn", MapId, options, eventData);
    }

    /// <summary>
    /// Decreases the map's zoom level by 1.
    /// </summary>
    /// <param name="options">Animation options object (optional).</param>
    /// <param name="eventData">Additional event data (optional).</param>
    public async ValueTask ZoomOut(object? options = null, object? eventData = null)
    {
        await _jsModule.InvokeVoidAsync("MapInterop.zoomOut", MapId, options, eventData);
    }

    /// <summary>
    /// Zooms the map to a specific zoom level with animation.
    /// </summary>
    /// <param name="zoom">The target zoom level.</param>
    /// <param name="options">Animation options for duration, easing, etc. (optional).</param>
    /// <param name="eventData">Additional event data (optional).</param>
    public async ValueTask ZoomTo(double zoom, object? options = null, object? eventData = null)
    {
        await _jsModule.InvokeVoidAsync("MapInterop.zoomTo", MapId, zoom, options, eventData);
    }

    #endregion
}