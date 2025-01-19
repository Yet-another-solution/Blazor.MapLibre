using System.Collections.Concurrent;
using System.Text.Json;
using Community.Blazor.MapLibre.Models;
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


    #endregion
}