# Blazor MapLibre

## About The Project

This project should help people working on Blazor projects to use maps more easily.

## Getting Started

### Prerequisites

This project is created on .NET 8/9 so you need to use this or never version to run it.

### Installation

Install the package:

```bash
dotnet add package Community.Blazor.MapLibre
```

Add this to head of your file to load the css for the maps:

```html
<link href="_content/Community.Blazor.MapLibre/maplibre-5.12.0.min.css" rel="stylesheet" />
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
