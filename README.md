<a id="readme-top"></a>

<div align="center">
  <a href="https://github.com/Yet-another-solution/Blazor.MapLibre">
    <img src="https://maplibre.org/_astro/maplibre-logo.wyLiUNdu_Zcg5mX.svg" alt="Blazor.MapLibre logo" width="300" height="100">
  </a>

  <h3 align="center">Blazor.MapLibre</h3>

  <p align="center">A C# wrapper around MapLibre GL JS for Blazor</p>

[![build](https://img.shields.io/github/actions/workflow/status/Yet-another-solution/Blazor.MapLibre/build-deploy-publish.yml?style=for-the-badge&label=build)](https://github.com/Yet-another-solution/Blazor.MapLibre/actions/workflows/build-deploy-publish.yml) [![NuGet](https://img.shields.io/nuget/v/Community.Blazor.MapLibre?style=for-the-badge)](https://www.nuget.org/packages/Community.Blazor.MapLibre) [![Last commit](https://img.shields.io/github/last-commit/Yet-another-solution/Blazor.MapLibre?style=for-the-badge)](https://github.com/Yet-another-solution/Blazor.MapLibre/commits)<br>
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white) ![Blazor](https://img.shields.io/badge/Blazor-512BD4?style=for-the-badge&logo=blazor&logoColor=white) ![MapLibre](https://img.shields.io/badge/MapLibre-396CB2?style=for-the-badge&logo=maplibre&logoColor=white) ![TypeScript](https://img.shields.io/badge/TypeScript-3178C6?style=for-the-badge&logo=typescript&logoColor=white)

  <p align="center">
    <a href="https://yet-another-solution.github.io/Blazor.MapLibre/"><strong>View demo</strong></a>
    &middot;
    <a href="https://github.com/Yet-another-solution/Blazor.MapLibre/issues/new?labels=bug">Report Bug</a>
    &middot;
    <a href="https://github.com/Yet-another-solution/Blazor.MapLibre/issues/new?labels=enhancement">Request Feature</a>
  </p>
</div>

<hr>

<table>
  <tr>
    <td nowrap><img src="docs/images/icons/layers.svg" width="16" height="16" alt="">&nbsp;<strong>What</strong></td>
    <td>Interactive maps in Blazor with typed C#: markers, popups, layers, styles</td>
  </tr>
  <tr>
    <td nowrap><img src="docs/images/icons/monitor.svg" width="16" height="16" alt="">&nbsp;<strong>Works&nbsp;with</strong></td>
    <td>Blazor WebAssembly and Blazor Server</td>
  </tr>
  <tr>
    <td nowrap><img src="docs/images/icons/terminal.svg" width="16" height="16" alt="">&nbsp;<strong>Install</strong></td>
    <td><code>dotnet add package Community.Blazor.MapLibre</code></td>
  </tr>
  <tr>
    <td nowrap><img src="docs/images/icons/progress.svg" width="16" height="16" alt="">&nbsp;<strong>Status</strong></td>
    <td>Published on NuGet, see <a href="#roadmap">Roadmap</a><img align="right" src="docs/images/icons/spacer.svg" width="2000" height="1" alt=""></td>
  </tr>
</table>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#disclaimer">Disclaimer</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

This project should help people working on Blazor projects to use maps more easily.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Getting Started

### Prerequisites

This project is created on .NET 8 so you need to use this or never version to run it.

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

Add this to head of your file to load the css for the maps.
```html
<link href="_content/Community.Blazor.MapLibre/maplibre-6.13.0.min.css" rel="stylesheet" />
```

> **Upgrading from a release that bundled MapLibre GL JS 5.x?** The library now ships MapLibre GL JS 6.13.0:
> - Update the stylesheet link above to `maplibre-6.13.0.min.css`.
> - WebGL2 is required (WebGL1 support was removed upstream). If you need WebGL1, stay on version 1.4.0.
> - Nested objects/arrays in GeoJSON feature properties now arrive as real JSON objects instead of JSON strings.
> - `window.maplibregl` is still available for plugins and custom JavaScript once the first map has loaded.
>
> See the [MapLibre GL JS v5 to v6 migration guide](https://github.com/maplibre/maplibre-gl-js/blob/main/docs/guides/v5-to-v6-migration-guide.md) for the full list of upstream changes.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- USAGE EXAMPLES -->
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


<!-- ROADMAP -->
## Roadmap
- ✅ Completed  
- ☑️ Partially implemented (missing C# models,...)
- ❌ Not started


| Feature | Status                       |
|---------|------------------------------|
| **Map** | **☑️ Partially implemented** |

<details>
<summary> Map Implementation </summary>

| Feature                                               | Status                       |
|-------------------------------------------------------|------------------------------|
| **Map**                                               | **☑️ Partially implemented** |
| - Options                                             | **✅ Complete**               |
| **Events**                                            |                              |
| - on()                                                | **✅ Complete**               |
| - off()                                               | **❌ Not started**            |
| - once()                                              | **❌ Not started**            |
| **Methods**                                           |                              |
| - addControl()                                        | **☑️ Partially Implemented** |
| - addImage()                                          | **☑️ Partially Implemented** |
| - addLayer()                                          | **✅ Complete**               |
| - addSource()                                         | **✅ Complete**               |
| - addSprite()                                         | **☑️ Partially Implemented** |
| - areTilesLoaded()                                    | **✅ Complete**               |
| - calculateCameraOptionsFromCameraLngLatAltRotation() | **✅ Complete**               |  
| - calculateCameraOptionsFromTo()                      | **✅ Complete**               |  
| - cameraForBounds()                                   | **☑️ Partially Implemented** |  
| - easeTo()                                            | **✅ Complete**               |
| - fitBounds()                                         | **✅ Complete**               |
| - fitScreenCoordinates()                              | **✅ Complete**               |
| - flyTo()                                             | **✅ Complete**               |
| - getBearing()                                        | **✅ Complete**               |
| - getBounds()                                         | **✅ Complete**               |
| - getCameraTargetElevation()                          | **✅ Complete**               |
| - getCanvas()                                         | **✅ Complete**               |
| - getCanvasContainer()                                | **✅ Complete**               |
| - getCenter()                                         | **✅ Complete**               |
| - getCenterClampedToGround()                          | **✅ Complete**               |
| - getCenterElevation()                                | **✅ Complete**               |
| - getContainer()                                      | **✅ Complete**               |
| - getFeatureState()                                   | **☑️ Partially Implemented** |
| - getFilter()                                         | **✅ Complete**               |
| - getGlyphs()                                         | **✅ Complete**               |
| - getImage()                                          | **✅ Complete**               |
| - getLayer()                                          | **✅ Complete**               |
| - getLayersOrder()                                    | **✅ Complete**               |
| - getLayoutProperty()                                 | **☑️ Partially Implemented** |
| - getLight()                                          | **☑️ Partially Implemented** |
| - getMaxBounds()                                      | **☑️ Partially Implemented** |
| - getMaxPitch()                                       | **✅ Complete**               |
| - getMaxZoom()                                        | **✅ Complete**               |
| - getMinPitch()                                       | **✅ Complete**               |
| - getMinZoom()                                        | **✅ Complete**               |
| - getPadding()                                        | **☑️ Partially Implemented** |
| - getPaintProperty()                                  | **☑️ Partially Implemented** |
| - getPitch()                                          | **✅ Complete**               |
| - getPixelRatio()                                     | **✅ Complete**               |
| - getProjection()                                     | **☑️ Partially Implemented** |
| - getRenderWorldCopies()                              | **✅ Complete**               |
| - getRoll()                                           | **✅ Complete**               |
| - getSky()                                            | **☑️ Partially Implemented** |
| - getSource()                                         | **☑️ Partially Implemented** |
| - getSprite()                                         | **☑️ Partially Implemented** |
| - getStyle()                                          | **☑️ Partially Implemented** |
| - getTerrain()                                        | **☑️ Partially Implemented** |
| - getVerticalFieldOfView()                            | **✅ Complete**               |
| - getZoom()                                           | **✅ Complete**               |
| - hasControl()                                        | **☑️ Partially Implemented** |
| - hasImage()                                          | **✅ Complete**               |
| - isMoving()                                          | **✅ Complete**               |
| - isRotating()                                        | **✅ Complete**               |
| - isSourceLoaded()                                    | **✅ Complete**               |
| - isStyleLoaded()                                     | **✅ Complete**               |
| - isZooming()                                         | **✅ Complete**               |
| - jumpTo()                                            | **✅ Complete**               |
| - listens()                                           | **✅ Complete**               |
| - listImages()                                        | **✅ Complete**               |
| - loaded()                                            | **✅ Complete**               |
| - loadImage()                                         | **☑️ Partially Implemented** |
| - moveLayer()                                         | **✅ Complete**               |
| - panBy()                                             | **✅ Complete**               |
| - panTo()                                             | **✅ Complete**               |
| - project()                                           | **✅ Complete**               |
| - queryRenderedFeatures()                             | **☑️ Partially Implemented** |
| - querySourceFeatures()                               | **✅ Complete**               |
| - queryTerrainElevation()                             | **✅ Complete**               |
| - redraw()                                            | **✅ Complete**               |
| - remove()                                            | **✅ Complete**               |
| - removeControl()                                     | **☑️ Partially Implemented** |
| - removeFeatureState()                                | **✅ Complete**               |
| - removeImage()                                       | **✅ Complete**               |
| - removeLayer()                                       | **✅ Complete**               |
| - removeSource()                                      | **✅ Complete**               |
| - removeSprite()                                      | **✅ Complete**               |
| - resetNorth()                                        | **✅ Complete**               |
| - resetNorthPitch()                                   | **✅ Complete**               |
| - resize()                                            | **✅ Complete**               |
| - rotateTo()                                          | **✅ Complete**               |
| - setBearing()                                        | **✅ Complete**               |
| - setCenter()                                         | **✅ Complete**               |
| - setCenterClampedToGround()                          | **✅ Complete**               |
| - setCenterElevation()                                | **✅ Complete**               |
| - setFeatureState()                                   | **✅ Complete**               |
| - setZoom()                                           | **✅ Complete**               |
| - setStyle()                                          | **☑️ Partially Implemented** |
| - stop()                                              | **✅ Complete**               |
| - unproject()                                         | **✅ Complete**               |
| - updateImage()                                       | **☑️ Partially Implemented** |
| - zoomIn()                                            | **✅ Complete**               |
| - zoomOut()                                           | **✅ Complete**               |
| - zoomTo()                                            | **✅ Complete**               |

</details>

| Feature     | Status            |
|-------------|-------------------|
| **Markers** | **✅ Complete** |

| Feature   | Status           |
|-----------|------------------|
| **Popup** | **✅ Complete**  |



See the [open issues](https://github.com/Yet-another-solution/Blazor.MapLibre/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Top contributors:

<a href="https://github.com/Yet-another-solution/Blazor.MapLibre/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=Yet-another-solution/Blazor.MapLibre" alt="contrib.rocks image" />
</a>

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- DISCLAIMER -->
## Disclaimer

**Community.Blazor.MapLibre** is provided "as-is," without warranties of any kind, express or implied. The author(s) make no guarantees regarding the accuracy, reliability, or suitability of the software for any particular purpose.

By using this library, you acknowledge that:

- The software is **open-source and freely available**, with no assurances of ongoing maintenance or support.
- The author(s) are **not liable for any damages, data loss, or operational failures** resulting from its use.
- Users assume full responsibility for implementing proper **security and compliance measures** when integrating this library into their applications.
- The software may change over time, and updates may introduce modifications or remove certain features.

Use of **Community.Blazor.MapLibre** is **at your own risk**, and contributions or modifications by third parties are not officially vetted. If you encounter issues or bugs, reporting them via the official repository is encouraged, but resolution is not guaranteed.

For licensing details, refer to the **[Unlicense](https://choosealicense.com/licenses/unlicense/)** statement included with the repository.


<!-- LICENSE -->
## License

This project, **Community.Blazor.MapLibre**, is released under the **[Unlicense](https://choosealicense.com/licenses/unlicense/)**, meaning it is free and unencumbered software dedicated to the public domain.

However, this library is a wrapper around **MapLibre GL JS**, which is licensed under the **BSD-3-Clause** license. Users must comply with the original library’s license when using this wrapper.

For details on MapLibre GL JS licensing, visit: [MapLibre GL JS License](https://github.com/maplibre/maplibre-gl-js/blob/main/LICENSE.txt).


<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

Use this space to list resources you find helpful and would like to give credit to. I've included a few of my favorites to kick things off!

* [GitHub Emoji Cheat Sheet](https://www.webpagefx.com/tools/emoji-cheat-sheet)
* [Img Shields](https://shields.io)
* [GitHub Pages](https://pages.github.com)
* [Best-README-Template](https://github.com/othneildrew/Best-README-Template)

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[nuget-shield]: https://img.shields.io/nuget/v/Community.Blazor.MapLibre.svg?style=for-the-badge
[nuget-url]: https://www.nuget.org/packages/Community.Blazor.MapLibre
