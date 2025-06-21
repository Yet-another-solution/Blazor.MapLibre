using Microsoft.JSInterop;

namespace Community.Blazor.MapLibre;

public interface IMapLibrePlugin : IAsyncDisposable
{
    /// <summary>
    /// Initializes the plugin with the Map Libre map and JavaScript runtime.
    /// Invoked by the <see cref="MapLibre"/> component when the map is ready.
    /// </summary>
    /// <param name="map">A reference to the Map Libre map object.</param>
    /// <param name="runtime">A reference to the JavaScript runtime.</param>
    Task Initialize(IJSObjectReference map, IJSRuntime runtime);
}