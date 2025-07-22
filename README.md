<a id="readme-top"></a>

<!-- PROJECT SHIELDS -->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Unlicense License][license-shield]][license-url]
[![Issues][issues-shield]][issues-url]
[![NuGet][nuget-shield]][nuget-url]



<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a>
    <img src="https://maplibre.org/_astro/maplibre-logo.wyLiUNdu_Zcg5mX.svg" alt="Logo" width="300" height="100">
  </a>

<h3 align="center">Blazor.MapLibre</h3>

  <p align="center">
    A C# wrapper around MapLibre GL JS library
    <br />
    <br />
    <a href="https://yet-another-solution.github.io/Blazor.MapLibre/">View Demo</a>
    &middot;
    <a href="https://github.com/Yet-another-solution/Blazor.MapLibre/issues/new?labels=bug&template=bug-report---.md">Report Bug</a>
    &middot;
    <a href="https://github.com/Yet-another-solution/Blazor.MapLibre/issues/new?labels=enhancement&template=feature-request---.md">Request Feature</a>
  </p>
</div>



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

This project is created on .NET 9 so you need to use this or never version to run it.

### Installation

Install the package:
```bash
dotnet add package Community.Blazor.MapLibre
```

Add this to head of your file to load the css for the maps.
```html
<link href="_content/Community.Blazor.MapLibre/maplibre-5.3.0.min.css" rel="stylesheet" />
```

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
[contributors-shield]: https://img.shields.io/github/contributors/Yet-another-solution/Blazor.MapLibre.svg?style=for-the-badge
[contributors-url]: https://github.com/Yet-another-solution/Blazor.MapLibre/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/Yet-another-solution/Blazor.MapLibre.svg?style=for-the-badge
[forks-url]: https://github.com/Yet-another-solution/Blazor.MapLibre/network/members
[stars-shield]: https://img.shields.io/github/stars/Yet-another-solution/Blazor.MapLibre.svg?style=for-the-badge
[stars-url]: https://github.com/Yet-another-solution/Blazor.MapLibre/stargazers
[issues-shield]: https://img.shields.io/github/issues/Yet-another-solution/Blazor.MapLibre.svg?style=for-the-badge
[issues-url]: https://github.com/Yet-another-solution/Blazor.MapLibre/issues
[license-shield]: https://img.shields.io/github/license/Yet-another-solution/Blazor.MapLibre.svg?style=for-the-badge
[license-url]: https://github.com/Yet-another-solution/Blazor.MapLibre/blob/master/LICENSE.txt
[nuget-shield]: https://img.shields.io/nuget/v/Community.Blazor.MapLibre.svg?style=for-the-badge
[nuget-url]: https://www.nuget.org/packages/Community.Blazor.MapLibre
