using Community.Blazor.MapLibre.Models.Feature;
using Community.Blazor.MapLibre.Models.Feature.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Community.Blazor.MapLibre.Examples.WebAssembly.Plugins;

public class MapboxGlDrawPlugin : IMapLibrePlugin
{
    
    /// <summary>
    /// A reference to the <see cref="IJSObjectReference"/> JavaScript object for the MapLibre map.
    /// </summary>
    /// <remarks>
    /// Used to access the map instance in JavaScript. This is set when the plugin is initialized and allows interaction with the map.
    /// </remarks>
    private IJSObjectReference MapObject { get; set; } = null!;

    /// <summary>
    /// A reference to the <see cref="DotNetObjectReference"/> plugin object.
    /// </summary>
    /// <remarks>
    /// Passed to the JavaScript modules <c>initialize</c> function to allow callbacks to the plugins .NET methods.
    /// Set via <see cref="Initialize"/> when plugins are loaded by the <see cref="MapLibre"/>.
    /// </remarks>
    private DotNetObjectReference<MapboxGlDrawPlugin>? PluginDotNetReference { get; set; }

    /// <summary>
    /// A reference to the <see cref="IJSObjectReference"/> JavaScript module instance for this plugin.
    /// </summary>
    /// <remarks>
    /// Only has access to the JavaScript module and functions within for the plugin itself.
    /// Set via <see cref="Initialize"/> when plugins are loaded by the <see cref="MapLibre"/>.
    /// </remarks>
    private IJSObjectReference PluginJsModule { get; set; } = null!;
    
    /// <summary>
    /// Triggered when a draw-related update occurs on the map.
    /// Allows the user to respond to changes or updates related to drawing features on the map,
    /// such as modifications to drawn shapes or geographic feature data updates.
    /// </summary>
    public Func<(FeatureCollection featureData, string mapStatus), Task>? OnDrawUpdate { get; set; }
    
    public async Task Initialize(IJSObjectReference map, IJSRuntime runtime)
    {
        MapObject = map;
        
        // Create a DotNetObjectReference for this plugin instance to allow JavaScript to call back into .NET methods.
        PluginDotNetReference = DotNetObjectReference.Create(this);
        
        // Import the JavaScript module for the Mapbox GL Draw plugin
        PluginJsModule = await runtime.InvokeAsync<IJSObjectReference>("import", "/_content/MapboxGlDrawPlugin/MapboxGlDrawPlugin.js");
        
        // Import dependencies for the plugin
        await runtime.InvokeAsync<IJSObjectReference>("import", "/_content/MapboxGlDrawPlugin/turf.min.js");
        await runtime.InvokeAsync<IJSObjectReference>("import", "/_content/MapboxGlDrawPlugin/mapbox-gl-draw.js");
        
        await PluginJsModule.InvokeVoidAsync("initialize", MapObject, PluginDotNetReference);
    }
    
    [JSInvokable]
    public async Task OnDrawUpdateCallback(JsFeatureCollection jsFeatureCollection, string mapStatus)
    {
        var featureCollection = jsFeatureCollection.ToFeatureCollection();
        if (OnDrawUpdate != null)
        {
            await OnDrawUpdate((featureCollection, mapStatus));
        }
    }
    
    /// <summary>
    /// Adds a control to the map instance based on the specified control type and options.
    /// </summary>
    /// <param name="drawControl">The type of control to be added to the map.</param>
    /// <returns>A task that represents the asynchronous operation of adding the control.</returns>
    public async ValueTask AddControl(object drawControl) =>
        await PluginJsModule.InvokeVoidAsync("addControl", drawControl);

    /// <summary>
    /// Adds a feature to the map's draw control.
    /// </summary>
    /// <param name="feature">The feature to be added to the draw control.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async ValueTask AddFeature(FeatureFeature feature) =>
        await PluginJsModule.InvokeVoidAsync("addFeature", feature);
    
    public async ValueTask DisposeAsync()
    {
        try
        {
            await PluginJsModule.DisposeAsync();
        }
        catch (JSDisconnectedException) { }
        catch (ObjectDisposedException) { }

        PluginDotNetReference?.Dispose();
        PluginDotNetReference = null;
    }
}