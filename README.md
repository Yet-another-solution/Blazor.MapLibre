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

### Installation

Install the package:
```bash
dotnet add package Community.Blazor.MapLibre
```

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- USAGE EXAMPLES -->
## Usage

After the package is installed you can use it with simple:
```csharp
<Map />
```

You can customize the map more with options using `MapOptions.cs`:
```csharp
<Map Options="_mapOptions"></Map>

@code
{
    private readonly MapOptions _mapOptions = new MapOptions();
}
```


<!-- ROADMAP -->
## Roadmap
- ✅ Completed  
- ☑️ Partially implemented
- ❌ Not started

| Feature                                               | Status                       |
|-------------------------------------------------------|------------------------------|
| **Map**                                               | **☑️ Partially implemented** |
| - Options                                             | **✅ Complete**               |
| **Events**                                            |                              |
| - on()                                                | **✅ Complete**               |
| - off()                                               | **❌ Not started**            |
| - once()                                              | **❌ Not started**            |
| **Methods**                                           |                |
| - addControl()                                        | **✅ Complete**               |
| - addImage()                                          | **✅ Complete**               |
| - addLayer()                                          | **✅ Complete**               |
| - addSource()                                         | **✅ Complete**               |
| - addSprite()                                         | **✅ Complete**               |
| - areTilesLoaded()                                    | **✅ Complete**               |
| - calculateCameraOptionsFromCameraLngLatAltRotation() | **✅ Complete**               |  
| - calculateCameraOptionsFromTo()                      | **✅ Complete**               |  
| - cameraForBounds()                                   | **✅ Complete**               |  
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
| - getCenterClampedToGround()                          | **❌ Not started**            |
| - getCenterElevation()                                | **❌ Not started**            |
| - getContainer()                                      | **❌ Not started**            |
| - getFeatureState()                                   | **❌ Not started**            |
| - getFilter()                                         | **❌ Not started**            |
| - getGlyphs()                                         | **❌ Not started**            |
| - getImage()                                          | **❌ Not started**            |
| - getLayer()                                          | **❌ Not started**            |
| - getLayersOrder()                                    | **❌ Not started**            |
| - getLayoutProperty()                                 | **❌ Not started**            |
| - getLight()                                          | **❌ Not started**            |
| - getMaxBounds()                                      | **❌ Not started**            |
| - getMaxPitch()                                       | **❌ Not started**            |
| - getMaxZoom()                                        | **❌ Not started**            |
| - getMinPitch()                                       | **❌ Not started**            |
| - getMinZoom()                                        | **❌ Not started**            |
| - getPadding()                                        | **❌ Not started**            |
| - getPaintProperty()                                  | **❌ Not started**            |
| - getPitch()                                          | **❌ Not started**            |
| - getPixelRatio()                                     | **❌ Not started**            |
| - getProjection()                                     | **❌ Not started**            |
| - getRenderWorldCopies()                              | **❌ Not started**            |
| - getRoll()                                           | **❌ Not started**            |
| - getSky()                                            | **❌ Not started**            |
| - getSource()                                         | **❌ Not started**            |
| - getSprite()                                         | **❌ Not started**            |
| - getStyle()                                          | **❌ Not started**            |
| - getTerrain()                                        | **❌ Not started**            |
| - getVerticalFieldOfView()                            | **❌ Not started**            |
| - getZoom()                                           | **❌ Not started**            |
| - hasControl()                                        | **❌ Not started**            |
| - hasImage()                                          | **❌ Not started**            |
| - isMoving()                                          | **❌ Not started**            |
| - isRotating()                                        | **❌ Not started**            |
| - isSourceLoaded()                                    | **❌ Not started**            |
| - isStyleLoaded()                                     | **❌ Not started**            |
| - isZooming()                                         | **❌ Not started**            |
| - jumpTo()                                            | **❌ Not started**            |
| - listens()                                           | **❌ Not started**            |
| - listImages()                                        | **❌ Not started**            |
| - loaded()                                            | **❌ Not started**            |
| - loadImage()                                         | **❌ Not started**            |
| - moveLayer()                                         | **❌ Not started**            |
| - panBy()                                             | **❌ Not started**            |
| - panTo()                                             | **❌ Not started**            |
| - project()                                           | **❌ Not started**            |
| - queryRenderedFeatures()                             | **❌ Not started**            |
| - querySourceFeatures()                               | **❌ Not started**            |
| - queryTerrainElevation()                             | **❌ Not started**            |
| - redraw()                                            | **❌ Not started**            |
| - remove()                                            | **❌ Not started**            |
| - removeControl()                                     | **❌ Not started**            |
| - removeFeatureState()                                | **❌ Not started**            |
| - removeImage()                                       | **❌ Not started**            |
| - removeLayer()                                       | **❌ Not started**            |
| - removeSource()                                      | **❌ Not started**            |
| - removeSprite()                                      | **❌ Not started**            |
| - resetNorth()                                        | **❌ Not started**            |
| - resetNorthPitch()                                   | **❌ Not started**            |
| - resize()                                            | **❌ Not started**            |
| - rotateTo()                                          | **❌ Not started**            |
| - setBearing()                                        | **❌ Not started**            |
| - setCenter()                                         | **❌ Not started**            |
| - setCenterClampedToGround()                          | **❌ Not started**            |
| - setCenterElevation()                                | **❌ Not started**            |
| - setEventedParent()                                  | **❌ Not started**            |
| - setFeatureState()                                   | **❌ Not started**            |
| - setFilter()                                         | **❌ Not started**            |
| - setGlyphs()                                         | **❌ Not started**            |
| - setLayerZoomRange()                                 | **❌ Not started**            |
| - setLayoutProperty()                                 | **❌ Not started**            |
| - setLight()                                          | **❌ Not started**            |
| - setMaxBounds()                                      | **❌ Not started**            |
| - setMaxPitch()                                       | **❌ Not started**            |
| - setMaxZoom()                                        | **❌ Not started**            |
| - setMinPitch()                                       | **❌ Not started**            |
| - setMinZoom()                                        | **❌ Not started**            |
| - setPadding()                                        | **❌ Not started**            |
| - setPaintProperty()                                  | **❌ Not started**            |
| - setPitch()                                          | **❌ Not started**            |
| - setPixelRatio()                                     | **❌ Not started**            |
| - setProjection()                                     | **❌ Not started**            |
| - setRenderWorldCopies()                              | **❌ Not started**            |
| - setRoll()                                           | **❌ Not started**            |
| - setSky()                                            | **❌ Not started**            |
| - setSprite()                                         | **❌ Not started**            |
| - setStyle()                                          | **❌ Not started**            |
| - setTerrain()                                        | **❌ Not started**            |
| - setTransformRequest()                               | **❌ Not started**            |
| - setVerticalFieldOfView()                            | **❌ Not started**            |
| - setZoom()                                           | **❌ Not started**            |
| - snapToNorth()                                       | **❌ Not started**            |
| - stop()                                              | **❌ Not started**            |
| - triggerRepaint()                                    | **❌ Not started**            |
| - unproject()                                         | **❌ Not started**            |
| - updateImage()                                       | **❌ Not started**            |
| - zoomIn()                                            | **❌ Not started**            |
| - zoomOut()                                           | **❌ Not started**            |
| - zoomTo()                                            | **❌ Not started**            |


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



<!-- LICENSE -->
## License

Distributed under the Unlicense License. See `UNLICENSE` for more information.

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
