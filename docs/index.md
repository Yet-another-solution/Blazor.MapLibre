# Blazor MapLibre

## About The Project

This project should help people working on Blazor projects to use maps more easily.

## Getting Started

### Prerequisites

This project is created on .NET 8/9 so you need to use this or never version to run it.

#### Browser requirements (WebGL)

Version 2.0.0 and newer bundle MapLibre GL JS 6.x, which **requires WebGL2**. WebGL1 is no longer supported, so the map will not render in a browser or on a device that only offers WebGL1.

If you need WebGL1 support, stay on version 1.4.0, the last release that bundles MapLibre GL JS 5.x (which supports both WebGL1 and WebGL2):

```bash
dotnet add package Community.Blazor.MapLibre --version 1.4.0
```

With 1.4.0 the stylesheet link is `_content/Community.Blazor.MapLibre/maplibre-5.12.0.min.css`.

### Installation

Install the package:

```bash
dotnet add package Community.Blazor.MapLibre
```

Add this to head of your file to load the css for the maps:

```html
<link href="_content/Community.Blazor.MapLibre/maplibre-6.13.0.min.css" rel="stylesheet" />
```

## Usage

After the package is installed you can use it with simple:

```csharp
<MapLibre />
```

You can customize the map more with options using `MapOptions.cs`:

```csharp
<MapLibre Options="_mapOptions"></MapLibre>

@code
{
    private readonly MapOptions _mapOptions = new MapOptions();
}
```
