using System.Text.Json;
using Community.Blazor.MapLibre.Models.Layers;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests;

/// <summary>
/// Tests to verify that the Layer polymorphic hierarchy (background, circle, fill-extrusion, fill,
/// heatmap, hillshade, line, raster, symbol) serializes and deserializes correctly through the
/// "type" discriminator, mirroring the approach used for IFeature in FeatureSerializationTests.
/// </summary>
public class LayerSerializationTests
{
    [Fact]
    public void BackgroundLayer_Should_Serialize_Successfully()
    {
        // Arrange
        var layer = new BackgroundLayer
        {
            Id = "background-layer"
        };

        // Act
        var json = JsonSerializer.Serialize<Layer>(layer);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"type\":\"background\"");
        json.Should().Contain("\"id\":\"background-layer\"");
    }

    [Fact]
    public void BackgroundLayer_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """
                   {
                        "type":"background",
                        "id":"background-layer"
                   }
                   """;

        // Act
        var layer = JsonSerializer.Deserialize<Layer>(json);

        // Assert
        layer.Should().NotBeNull();
        layer.Should().BeOfType<BackgroundLayer>();
        var backgroundLayer = (BackgroundLayer)layer!;
        backgroundLayer.Id.Should().Be("background-layer");
    }

    [Fact]
    public void CircleLayer_Should_Serialize_Successfully()
    {
        // Arrange
        var layer = new CircleLayer
        {
            Id = "circle-layer",
            Source = "resources",
            Paint = new CircleLayerPaint
            {
                CircleRadius = 8,
                CircleColor = "#007cbf",
                CircleOpacity = 0.5
            }
        };

        // Act
        var json = JsonSerializer.Serialize<Layer>(layer);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"type\":\"circle\"");
        json.Should().Contain("\"id\":\"circle-layer\"");
        json.Should().Contain("\"source\":\"resources\"");
        json.Should().Contain("\"circle-radius\":8");
        json.Should().Contain("\"circle-color\":\"#007cbf\"");
        json.Should().Contain("\"circle-opacity\":0.5");
    }

    [Fact]
    public void CircleLayer_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """
                   {
                        "type":"circle",
                        "id":"circle-layer",
                        "source":"resources",
                        "paint":{
                            "circle-radius":8,
                            "circle-color":"#007cbf",
                            "circle-opacity":0.5
                        }
                   }
                   """;

        // Act
        var layer = JsonSerializer.Deserialize<Layer>(json);

        // Assert
        layer.Should().NotBeNull();
        layer.Should().BeOfType<CircleLayer>();
        var circleLayer = (CircleLayer)layer!;
        circleLayer.Id.Should().Be("circle-layer");
        circleLayer.Source.Should().Be("resources");
        circleLayer.Paint.Should().NotBeNull();
        circleLayer.Paint!.CircleRadius!.Value.AsT0.Should().Be(8);
        circleLayer.Paint.CircleColor!.Value.AsT0.Should().Be("#007cbf");
        circleLayer.Paint.CircleOpacity!.Value.AsT0.Should().Be(0.5);
    }

    [Fact]
    public void FillExtrusionLayer_Should_Serialize_Successfully()
    {
        // Arrange
        var layer = new FillExtrusionLayer
        {
            Id = "extrusion-layer",
            Source = "buildings"
        };

        // Act
        var json = JsonSerializer.Serialize<Layer>(layer);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"type\":\"fill-extrusion\"");
        json.Should().Contain("\"id\":\"extrusion-layer\"");
        json.Should().Contain("\"source\":\"buildings\"");
    }

    [Fact]
    public void FillExtrusionLayer_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """
                   {
                        "type":"fill-extrusion",
                        "id":"extrusion-layer",
                        "source":"buildings",
                        "source-layer":"buildings-layer"
                   }
                   """;

        // Act
        var layer = JsonSerializer.Deserialize<Layer>(json);

        // Assert
        layer.Should().NotBeNull();
        layer.Should().BeOfType<FillExtrusionLayer>();
        var fillExtrusionLayer = (FillExtrusionLayer)layer!;
        fillExtrusionLayer.Id.Should().Be("extrusion-layer");
        fillExtrusionLayer.Source.Should().Be("buildings");
        fillExtrusionLayer.SourceLayer.Should().Be("buildings-layer");
    }

    [Fact]
    public void FillLayer_Should_Serialize_Successfully()
    {
        // Arrange
        var layer = new FillLayer
        {
            Id = "GeoJsonLayerId",
            Source = "maine",
            Paint = new FillLayerPaint
            {
                FillColor = "#4e8752",
                FillOpacity = 0.8,
                FillOutlineColor = "#000000"
            }
        };

        // Act
        var json = JsonSerializer.Serialize<Layer>(layer);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"type\":\"fill\"");
        json.Should().Contain("\"id\":\"GeoJsonLayerId\"");
        json.Should().Contain("\"source\":\"maine\"");
        json.Should().Contain("\"fill-color\":\"#4e8752\"");
        json.Should().Contain("\"fill-opacity\":0.8");
        json.Should().Contain("\"fill-outline-color\":\"#000000\"");
    }

    [Fact]
    public void FillLayer_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """
                   {
                        "type":"fill",
                        "id":"GeoJsonLayerId",
                        "source":"maine",
                        "paint":{
                            "fill-color":"#4e8752",
                            "fill-opacity":0.8,
                            "fill-outline-color":"#000000"
                        }
                   }
                   """;

        // Act
        var layer = JsonSerializer.Deserialize<Layer>(json);

        // Assert
        layer.Should().NotBeNull();
        layer.Should().BeOfType<FillLayer>();
        var fillLayer = (FillLayer)layer!;
        fillLayer.Id.Should().Be("GeoJsonLayerId");
        fillLayer.Source.Should().Be("maine");
        fillLayer.Paint.Should().NotBeNull();
        fillLayer.Paint!.FillColor!.Value.AsT0.Should().Be("#4e8752");
        fillLayer.Paint.FillOpacity!.Value.AsT0.Should().Be(0.8);
        fillLayer.Paint.FillOutlineColor!.Value.AsT0.Should().Be("#000000");
    }

    [Fact]
    public void HeatMapLayer_Should_Serialize_Successfully()
    {
        // Arrange
        var layer = new HeatMapLayer
        {
            Id = "heatmap-layer",
            Source = "earthquakes"
        };

        // Act
        var json = JsonSerializer.Serialize<Layer>(layer);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"type\":\"heatmap\"");
        json.Should().Contain("\"id\":\"heatmap-layer\"");
        json.Should().Contain("\"source\":\"earthquakes\"");
    }

    [Fact]
    public void HeatMapLayer_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """
                   {
                        "type":"heatmap",
                        "id":"heatmap-layer",
                        "source":"earthquakes",
                        "source-layer":"quakes"
                   }
                   """;

        // Act
        var layer = JsonSerializer.Deserialize<Layer>(json);

        // Assert
        layer.Should().NotBeNull();
        layer.Should().BeOfType<HeatMapLayer>();
        var heatMapLayer = (HeatMapLayer)layer!;
        heatMapLayer.Id.Should().Be("heatmap-layer");
        heatMapLayer.Source.Should().Be("earthquakes");
        heatMapLayer.SourceLayer.Should().Be("quakes");
    }

    [Fact]
    public void HillShadeLayer_Should_Serialize_Successfully()
    {
        // Arrange
        var layer = new HillShadeLayer
        {
            Id = "hillshade-layer",
            Source = "terrain"
        };

        // Act
        var json = JsonSerializer.Serialize<Layer>(layer);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"type\":\"hillshade\"");
        json.Should().Contain("\"id\":\"hillshade-layer\"");
        json.Should().Contain("\"source\":\"terrain\"");
    }

    [Fact]
    public void HillShadeLayer_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """
                   {
                        "type":"hillshade",
                        "id":"hillshade-layer",
                        "source":"terrain"
                   }
                   """;

        // Act
        var layer = JsonSerializer.Deserialize<Layer>(json);

        // Assert
        layer.Should().NotBeNull();
        layer.Should().BeOfType<HillShadeLayer>();
        var hillShadeLayer = (HillShadeLayer)layer!;
        hillShadeLayer.Id.Should().Be("hillshade-layer");
        hillShadeLayer.Source.Should().Be("terrain");
    }

    [Fact]
    public void LineLayer_Should_Serialize_Successfully()
    {
        // Arrange
        var layer = new LineLayer
        {
            Id = "line-layer",
            Source = "roads",
            Paint = new LineLayerPaint
            {
                LineColor = "#ff0000",
                LineWidth = 2,
                LineOpacity = 0.9
            }
        };

        // Act
        var json = JsonSerializer.Serialize<Layer>(layer);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"type\":\"line\"");
        json.Should().Contain("\"id\":\"line-layer\"");
        json.Should().Contain("\"source\":\"roads\"");
        json.Should().Contain("\"line-color\":\"#ff0000\"");
        json.Should().Contain("\"line-width\":2");
        json.Should().Contain("\"line-opacity\":0.9");
    }

    [Fact]
    public void LineLayer_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """
                   {
                        "type":"line",
                        "id":"line-layer",
                        "source":"roads",
                        "paint":{
                            "line-color":"#ff0000",
                            "line-width":2,
                            "line-opacity":0.9
                        }
                   }
                   """;

        // Act
        var layer = JsonSerializer.Deserialize<Layer>(json);

        // Assert
        layer.Should().NotBeNull();
        layer.Should().BeOfType<LineLayer>();
        var lineLayer = (LineLayer)layer!;
        lineLayer.Id.Should().Be("line-layer");
        lineLayer.Source.Should().Be("roads");
        lineLayer.Paint.Should().NotBeNull();
        lineLayer.Paint!.LineColor!.Value.AsT0.Should().Be("#ff0000");
        lineLayer.Paint.LineWidth!.Value.AsT0.Should().Be(2);
        lineLayer.Paint.LineOpacity!.Value.AsT0.Should().Be(0.9);
    }

    [Fact]
    public void RasterLayer_Should_Serialize_Successfully()
    {
        // Arrange
        var layer = new RasterLayer
        {
            Id = "raster-layer",
            Source = "satellite"
        };

        // Act
        var json = JsonSerializer.Serialize<Layer>(layer);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"type\":\"raster\"");
        json.Should().Contain("\"id\":\"raster-layer\"");
        json.Should().Contain("\"source\":\"satellite\"");
    }

    [Fact]
    public void RasterLayer_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """
                   {
                        "type":"raster",
                        "id":"raster-layer",
                        "source":"satellite"
                   }
                   """;

        // Act
        var layer = JsonSerializer.Deserialize<Layer>(json);

        // Assert
        layer.Should().NotBeNull();
        layer.Should().BeOfType<RasterLayer>();
        var rasterLayer = (RasterLayer)layer!;
        rasterLayer.Id.Should().Be("raster-layer");
        rasterLayer.Source.Should().Be("satellite");
    }

    [Fact]
    public void SymbolLayer_Should_Serialize_Successfully()
    {
        // Arrange
        var layer = new SymbolLayer
        {
            Id = "symbol-layer",
            Source = "places",
            Paint = new SymbolLayerPaint
            {
                IconColor = "#000000",
                TextColor = "#ffffff",
                IconOpacity = 1
            }
        };

        // Act
        var json = JsonSerializer.Serialize<Layer>(layer);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"type\":\"symbol\"");
        json.Should().Contain("\"id\":\"symbol-layer\"");
        json.Should().Contain("\"source\":\"places\"");
        json.Should().Contain("\"icon-color\":\"#000000\"");
        json.Should().Contain("\"text-color\":\"#ffffff\"");
        json.Should().Contain("\"icon-opacity\":1");
    }

    [Fact]
    public void SymbolLayer_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """
                   {
                        "type":"symbol",
                        "id":"symbol-layer",
                        "source":"places",
                        "paint":{
                            "icon-color":"#000000",
                            "text-color":"#ffffff",
                            "icon-opacity":1
                        }
                   }
                   """;

        // Act
        var layer = JsonSerializer.Deserialize<Layer>(json);

        // Assert
        layer.Should().NotBeNull();
        layer.Should().BeOfType<SymbolLayer>();
        var symbolLayer = (SymbolLayer)layer!;
        symbolLayer.Id.Should().Be("symbol-layer");
        symbolLayer.Source.Should().Be("places");
        symbolLayer.Paint.Should().NotBeNull();
        symbolLayer.Paint!.IconColor!.Value.AsT0.Should().Be("#000000");
        symbolLayer.Paint.TextColor!.Value.AsT0.Should().Be("#ffffff");
        symbolLayer.Paint.IconOpacity!.Value.AsT0.Should().Be(1);
    }
}
