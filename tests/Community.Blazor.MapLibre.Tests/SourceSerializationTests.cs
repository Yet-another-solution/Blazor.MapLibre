using System.Text.Json;
using Community.Blazor.MapLibre.Models.Sources;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests;

/// <summary>
/// Serialization/deserialization tests for the non-GeoJSON <see cref="ISource"/> implementations:
/// CanvasSource, ImageSource, RasterDEMTileSource, RasterTileSource, VectorTileSource, VideoSource,
/// plus the standalone QuerySourceFeatureOptions type.
/// GeoJsonSource and ISource's GeoJSON-data plumbing are covered in SetSourceDataTests/FeatureSerializationTests.
/// </summary>
public class SourceSerializationTests
{
    [Fact]
    public void CanvasSource_Should_Serialize_With_Discriminator_And_Properties()
    {
        // Arrange
        var source = new CanvasSource
        {
            Coordinates =
            [
                [-122.51, 37.83],
                [-122.44, 37.83],
                [-122.44, 37.78],
                [-122.51, 37.78]
            ],
            Canvas = "my-canvas-id",
            Animate = true
        };

        // Act
        var json = JsonSerializer.Serialize<ISource>(source);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        // Assert
        root.GetProperty("type").GetString().Should().Be("canvas");
        root.GetProperty("canvas").GetString().Should().Be("my-canvas-id");
        root.GetProperty("animate").GetBoolean().Should().BeTrue();
        var coordinates = root.GetProperty("coordinates");
        coordinates.GetArrayLength().Should().Be(4);
        coordinates[0][0].GetDouble().Should().Be(-122.51);
        coordinates[0][1].GetDouble().Should().Be(37.83);
    }

    [Fact]
    public void CanvasSource_Should_Deserialize_To_Correct_Type_With_Values()
    {
        // Arrange
        var json = """
                   {
                       "type": "canvas",
                       "canvas": "animated-canvas",
                       "animate": false,
                       "coordinates": [
                           [-122.51, 37.83],
                           [-122.44, 37.83],
                           [-122.44, 37.78],
                           [-122.51, 37.78]
                       ]
                   }
                   """;

        // Act
        var source = JsonSerializer.Deserialize<ISource>(json);

        // Assert
        source.Should().BeOfType<CanvasSource>();
        var canvasSource = (CanvasSource)source!;
        canvasSource.Canvas.Should().Be("animated-canvas");
        canvasSource.Animate.Should().BeFalse();
        canvasSource.Coordinates.Should().HaveCount(4);
        canvasSource.Coordinates[2].Should().Equal(-122.44, 37.78);
    }

    [Fact]
    public void ImageSource_Should_Serialize_With_Discriminator_And_Properties()
    {
        // Arrange
        var source = new ImageSource
        {
            Url = "https://example.com/images/overlay.png",
            Coordinates =
            [
                [-80.425, 46.437],
                [-71.516, 46.437],
                [-71.516, 37.936],
                [-80.425, 37.936]
            ]
        };

        // Act
        var json = JsonSerializer.Serialize<ISource>(source);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        // Assert
        root.GetProperty("type").GetString().Should().Be("image");
        root.GetProperty("url").GetString().Should().Be("https://example.com/images/overlay.png");
        root.GetProperty("coordinates").GetArrayLength().Should().Be(4);
    }

    [Fact]
    public void ImageSource_Should_Deserialize_To_Correct_Type_With_Values()
    {
        // Arrange
        var json = """
                   {
                       "type": "image",
                       "url": "https://example.com/images/overlay.png",
                       "coordinates": [
                           [-80.425, 46.437],
                           [-71.516, 46.437],
                           [-71.516, 37.936],
                           [-80.425, 37.936]
                       ]
                   }
                   """;

        // Act
        var source = JsonSerializer.Deserialize<ISource>(json);

        // Assert
        source.Should().BeOfType<ImageSource>();
        var imageSource = (ImageSource)source!;
        imageSource.Url.Should().Be("https://example.com/images/overlay.png");
        imageSource.Coordinates.Should().HaveCount(4);
        imageSource.Coordinates[0].Should().Equal(-80.425, 46.437);
    }

    [Fact]
    public void RasterDEMTileSource_Should_Serialize_With_Discriminator_And_Properties()
    {
        // Arrange
        var source = new RasterDEMTileSource
        {
            Tiles = ["https://example.com/tiles/dem/{z}/{x}/{y}.png"],
            Bounds = [-180.0, -85.051129, 180.0, 85.051129],
            Scheme = "xyz",
            MinZoom = 0,
            MaxZoom = 15,
            TileSize = 256,
            Attribution = "© Example DEM Provider",
            Encoding = "terrarium",
            RedFactor = 256.0f,
            GreenFactor = 1.0f,
            BlueFactor = 1.0f / 256.0f,
            BaseShift = -32768.0f
        };

        // Act
        var json = JsonSerializer.Serialize<ISource>(source);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        // Assert
        root.GetProperty("type").GetString().Should().Be("raster-dem");
        root.GetProperty("tiles")[0].GetString().Should().Be("https://example.com/tiles/dem/{z}/{x}/{y}.png");
        root.GetProperty("encoding").GetString().Should().Be("terrarium");
        root.GetProperty("tileSize").GetInt32().Should().Be(256);
        root.GetProperty("redFactor").GetSingle().Should().Be(256.0f);
        root.GetProperty("baseShift").GetSingle().Should().Be(-32768.0f);
        root.GetProperty("bounds").GetArrayLength().Should().Be(4);
    }

    [Fact]
    public void RasterDEMTileSource_Should_Deserialize_To_Correct_Type_With_Values()
    {
        // Arrange
        var json = """
                   {
                       "type": "raster-dem",
                       "url": "https://example.com/dem/tilejson.json",
                       "encoding": "mapbox",
                       "minzoom": 0,
                       "maxzoom": 14,
                       "tileSize": 512,
                       "redFactor": 1.0,
                       "greenFactor": 0.5,
                       "blueFactor": 0.25,
                       "baseShift": 0.0
                   }
                   """;

        // Act
        var source = JsonSerializer.Deserialize<ISource>(json);

        // Assert
        source.Should().BeOfType<RasterDEMTileSource>();
        var demSource = (RasterDEMTileSource)source!;
        demSource.Url.Should().Be("https://example.com/dem/tilejson.json");
        demSource.Encoding.Should().Be("mapbox");
        demSource.MaxZoom.Should().Be(14);
        demSource.TileSize.Should().Be(512);
        demSource.GreenFactor.Should().Be(0.5f);
    }

    [Fact]
    public void RasterTileSource_Should_Serialize_With_Discriminator_And_Properties()
    {
        // Arrange
        var source = new RasterTileSource
        {
            Tiles = ["https://example.com/tiles/{z}/{x}/{y}.png"],
            Bounds = [-180.0, -85.051129, 180.0, 85.051129],
            Scheme = "xyz",
            MinZoom = 0,
            MaxZoom = 19,
            TileSize = 256,
            Attribution = "© Example Raster Provider"
        };

        // Act
        var json = JsonSerializer.Serialize<ISource>(source);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        // Assert
        root.GetProperty("type").GetString().Should().Be("raster");
        root.GetProperty("tiles")[0].GetString().Should().Be("https://example.com/tiles/{z}/{x}/{y}.png");
        root.GetProperty("tileSize").GetInt32().Should().Be(256);
        root.GetProperty("attribution").GetString().Should().Be("© Example Raster Provider");
        root.GetProperty("maxzoom").GetSingle().Should().Be(19);
    }

    [Fact]
    public void RasterTileSource_Should_Deserialize_To_Correct_Type_With_Values()
    {
        // Arrange
        var json = """
                   {
                       "type": "raster",
                       "url": "https://example.com/raster/tilejson.json",
                       "scheme": "tms",
                       "minzoom": 2,
                       "maxzoom": 18,
                       "tileSize": 512,
                       "attribution": "© Example Raster Provider"
                   }
                   """;

        // Act
        var source = JsonSerializer.Deserialize<ISource>(json);

        // Assert
        source.Should().BeOfType<RasterTileSource>();
        var rasterSource = (RasterTileSource)source!;
        rasterSource.Url.Should().Be("https://example.com/raster/tilejson.json");
        rasterSource.Scheme.Should().Be("tms");
        rasterSource.MinZoom.Should().Be(2);
        rasterSource.TileSize.Should().Be(512);
    }

    [Fact]
    public void VectorTileSource_Should_Serialize_With_Discriminator_And_Properties()
    {
        // Arrange
        var source = new VectorTileSource
        {
            Tiles = ["https://example.com/tiles/{z}/{x}/{y}.pbf"],
            Bounds = [-180.0, -85.051129, 180.0, 85.051129],
            Scheme = "xyz",
            MinZoom = 0,
            MaxZoom = 14,
            Attribution = "© Example Vector Provider",
            TileSize = 512,
            PromoteId = "featureId",
            Encoding = "mvt"
        };

        // Act
        var json = JsonSerializer.Serialize<ISource>(source);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        // Assert
        root.GetProperty("type").GetString().Should().Be("vector");
        root.GetProperty("tiles")[0].GetString().Should().Be("https://example.com/tiles/{z}/{x}/{y}.pbf");
        root.GetProperty("promoteId").GetString().Should().Be("featureId");
        root.GetProperty("encoding").GetString().Should().Be("mvt");
        root.GetProperty("maxzoom").GetSingle().Should().Be(14);
    }

    [Fact]
    public void VectorTileSource_Should_Deserialize_To_Correct_Type_With_Values()
    {
        // Arrange
        var json = """
                   {
                       "type": "vector",
                       "url": "https://example.com/vector/tilejson.json",
                       "minzoom": 1,
                       "maxzoom": 16,
                       "attribution": "© Example Vector Provider",
                       "promoteId": { "buildings": "id" }
                   }
                   """;

        // Act
        var source = JsonSerializer.Deserialize<ISource>(json);

        // Assert
        source.Should().BeOfType<VectorTileSource>();
        var vectorSource = (VectorTileSource)source!;
        vectorSource.Url.Should().Be("https://example.com/vector/tilejson.json");
        vectorSource.MaxZoom.Should().Be(16);
        vectorSource.Attribution.Should().Be("© Example Vector Provider");
        vectorSource.PromoteId.Should().NotBeNull();
    }

    [Fact]
    public void VideoSource_Should_Serialize_With_Discriminator_And_Properties()
    {
        // Arrange
        var source = new VideoSource
        {
            Urls =
            [
                "https://example.com/videos/patricia_nasa.mp4",
                "https://example.com/videos/patricia_nasa.webm"
            ],
            Coordinates =
            [
                [-80.425, 46.437],
                [-71.516, 46.437],
                [-71.516, 37.936],
                [-80.425, 37.936]
            ]
        };

        // Act
        var json = JsonSerializer.Serialize<ISource>(source);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        // Assert
        root.GetProperty("type").GetString().Should().Be("video");
        root.GetProperty("urls").GetArrayLength().Should().Be(2);
        root.GetProperty("urls")[0].GetString().Should().Be("https://example.com/videos/patricia_nasa.mp4");
        root.GetProperty("coordinates").GetArrayLength().Should().Be(4);
    }

    [Fact]
    public void VideoSource_Should_Deserialize_To_Correct_Type_With_Values()
    {
        // Arrange
        var json = """
                   {
                       "type": "video",
                       "urls": [
                           "https://example.com/videos/patricia_nasa.mp4",
                           "https://example.com/videos/patricia_nasa.webm"
                       ],
                       "coordinates": [
                           [-80.425, 46.437],
                           [-71.516, 46.437],
                           [-71.516, 37.936],
                           [-80.425, 37.936]
                       ]
                   }
                   """;

        // Act
        var source = JsonSerializer.Deserialize<ISource>(json);

        // Assert
        source.Should().BeOfType<VideoSource>();
        var videoSource = (VideoSource)source!;
        videoSource.Urls.Should().HaveCount(2);
        videoSource.Urls.Should().Contain("https://example.com/videos/patricia_nasa.webm");
        videoSource.Coordinates.Should().HaveCount(4);
    }

    [Fact]
    public void QuerySourceFeatureOptions_Should_Serialize_Properties()
    {
        // Arrange
        var options = new QuerySourceFeatureOptions
        {
            Filter = new object[] { "==", "class", "residential" },
            SourceLayer = "buildings",
            Validate = false
        };

        // Act
        var json = JsonSerializer.Serialize(options);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        // Assert
        root.GetProperty("sourceLayer").GetString().Should().Be("buildings");
        root.GetProperty("validate").GetBoolean().Should().BeFalse();
        var filter = root.GetProperty("filter");
        filter.ValueKind.Should().Be(JsonValueKind.Array);
        filter.GetArrayLength().Should().Be(3);
        filter[1].GetString().Should().Be("class");
    }

    [Fact]
    public void QuerySourceFeatureOptions_Should_Deserialize_Properties()
    {
        // Arrange
        var json = """
                   {
                       "filter": ["==", "class", "residential"],
                       "sourceLayer": "buildings",
                       "validate": true
                   }
                   """;

        // Act
        var options = JsonSerializer.Deserialize<QuerySourceFeatureOptions>(json);

        // Assert
        options.Should().NotBeNull();
        options!.SourceLayer.Should().Be("buildings");
        options.Validate.Should().BeTrue();
        options.Filter.Should().NotBeNull();
    }

    [Fact]
    public void QuerySourceFeatureOptions_Null_Properties_Should_Be_Omitted_From_Json()
    {
        // Arrange
        var options = new QuerySourceFeatureOptions();

        // Act
        var json = JsonSerializer.Serialize(options);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        // Assert
        root.TryGetProperty("filter", out _).Should().BeFalse();
        root.TryGetProperty("sourceLayer", out _).Should().BeFalse();
        root.TryGetProperty("validate", out _).Should().BeFalse();
    }
}
