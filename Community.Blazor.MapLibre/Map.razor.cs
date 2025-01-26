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

    #endregion
}