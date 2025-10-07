using Community.Blazor.MapLibre.Examples;
using Community.Blazor.MapLibre.Examples.Examples;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register custom elements for the MapLibre examples
builder.RootComponents.RegisterCustomElement<AddMarker>("add-marker");
builder.RootComponents.RegisterCustomElement<AddListener>("add-listener");
builder.RootComponents.RegisterCustomElement<AddPopup>("add-popup");
builder.RootComponents.RegisterCustomElement<FitBounds>("fit-bounds");
builder.RootComponents.RegisterCustomElement<LoadGeoJson>("load-geojson");
builder.RootComponents.RegisterCustomElement<MapboxGlDraw>("mapbox-gl-draw");
builder.RootComponents.RegisterCustomElement<MultipleMaps>("multiple-maps");
builder.RootComponents.RegisterCustomElement<RenderGlobe>("render-globe");

await builder.Build().RunAsync();
