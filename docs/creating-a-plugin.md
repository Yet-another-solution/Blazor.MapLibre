# Creating a plugin

`Community.Blazor.MapLibre` is a wrapper library around the core [MapLibre GL JS JavaScript API](https://maplibre.org/maplibre-gl-js/docs/API/). You may want to interop with the MapLibre map in ways that cannot be achieved using the wrapper library, and for this, you can add a plugin.

At a high-level, a plugin can, but not exclusively:

- Maintain a reference to the MapLibre map object created by the core library.
- Import other JavaScript modules, kept separate from the core library.
- Use the MapLibre map object reference in its own JavaScript module(s).
- Expose a type-safe API similar to the core library.

## Steps

1. Create a **Razor Class Library** project.

    ```shell
    dotnet new razorclasslib --name MyMapLibreRotationPlugin
    ```

    _The existing files in the project are not relevant here and can be ignored/delete later._

2. Add a reference to the `Community.Blazor.MapLibre` NuGet package.

    _From the directory containing the project (`.csproj` file)._

    ```shell
    dotnet add package Community.Blazor.MapLibre
    ```

3. Create a plugin JavaScript module.

    A core rational for creating a plugin is to interop with the MapLibre object that was initialised in the core libraries JavaScript module. This map object can be passed into our JavaScript module if we design it to receive it.

    Create a file named `MyMapLibreRotationPlugin.js` in the `/wwwroot` folder as below.

    ```javascript
    let mapObject = {};
    let pluginDotnetReference = {};
    
    export function initialize(map, dotnetReference) {
        mapObject = map;
        pluginDotnetReference = dotnetReference;
        console.log("Plugin initialized");
    }
    
    // See https://maplibre.org/maplibre-gl-js/docs/examples/animate-camera-around-point/
    export function rotate(duration) {
        const rotate = (startTime) => {
            const elapsed = Date.now() - startTime;
            const progress = elapsed / duration;
            const degrees = progress * 360;
            mapObject.rotateTo(degrees % 360, { duration: 0 });
            if (elapsed < duration) {
                requestAnimationFrame(() => rotate(startTime));
            }
        };
        rotate(Date.now());
    }
    ```

    Accepting the `dotnetReference` into the JavaScript module enables call back to the plugin C# class. It's not used in this example, but the implementation does not differ from the official guidance. See [Call .NET methods from JavaScript functions in ASP.NET Core Blazor](https://learn.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/call-dotnet-from-javascript?view=aspnetcore-9.0). It could be used to notify the plugin C# class that the JavaScript module has initialized, or the rotation has complected for example.

4. Create a plugin class.

    Plugins must implement the `IMapLibrePlugin` interface, and should load in any JavaScript modules in the `Initialize` method, and perform any other setup needed.

    ```csharp
    public class MyMapLibreRotationPlugin : IMapLibrePlugin
    {
        private DotNetObjectReference<MyMapLibreRotationPlugin> PluginDotNetReference { get; set; } = null!;
        private IJSObjectReference PluginJsModule { get; set; } = null!;
        
        public async Task Initialize(IJSObjectReference map, IJSRuntime runtime)
        {
            PluginDotNetReference = DotNetObjectReference.Create(this);
            PluginJsModule = await runtime
                .InvokeAsync<IJSObjectReference>("import", "/_content/MyMapLibreRotationPlugin/MyMapLibreRotationPlugin.js");
            
            await PluginJsModule.InvokeVoidAsync("initialize", map, PluginDotNetReference);
        }
        
        public async ValueTask Rotate(int duration) => 
            await PluginJsModule.InvokeVoidAsync("rotate", duration);
        
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
    ```

   - Since the JavaScript module is being loaded from a Razor Class Library, the base path of the module should be `/_content/{NAMESPACE}/{MODULE_FILE_NAME}.js`.
   - The plugin is not responsible for disposing the `MapObject` since a that reference was provided to it by the core `MapLibre` component during initialization.

5. Add a reference to your plugin **Razor Class Library** project.

    ```shell
    dotnet reference add MyMapLibreRotationPlugin.csproj --project MyBlazorProject.csproj
    ```

6. Register your plugin with the MapLibre component.

    In the `OnAfterRenderAsync` method of the page/component that contains the `MapLibre` component.

    ```csharp
    private MapLibre? _map { get; set; }
    private MyMapLibreRotationPlugin _myMapLibreRotationPlugin = new();
        
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _map.RegisterPlugin(_myMapLibreRotationPlugin);
        }
    }
    ```

7. Use the capabilities exposed by your plugin.

    For example, in the `OnMapLoad` call back for the `MapLibre` component, you can use your plugin to invoke the capabilities it exposes.

    ```csharp
    private async Task OnMapLoad(EventArgs args)
    {
        await _myMapLibreRotationPlugin.Rotate(2000);
    }
    ```
