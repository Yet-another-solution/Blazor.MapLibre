using System.Text.Json;
using Community.Blazor.MapLibre.Models.Feature;
using Community.Blazor.MapLibre.Models.LayerFeatures;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests;

/// <summary>
/// Tests for the currently-untested types in Models/LayerFeatures: the LayerFeature
/// polymorphic base class and LayerFeatureCollection. LayerFeatureFeature is already
/// covered elsewhere.
/// </summary>
public class LayerFeatureSupportTests
{
    [Fact]
    public void LayerFeatureCollection_Should_Serialize_With_FeatureCollection_Discriminator()
    {
        // Arrange
        var collection = new LayerFeatureCollection
        {
            Source = "my-source",
            Features = new List<IFeature>
            {
                new FeatureFeature
                {
                    Id = "point1",
                    Geometry = new PointGeometry { Coordinates = new[] { -122.4194, 37.7749 } }
                }
            }
        };

        // Act
        var json = JsonSerializer.Serialize<LayerFeature>(collection);

        // Assert
        json.Should().Contain("\"type\":\"FeatureCollection\"");
        json.Should().Contain("\"source\":\"my-source\"");
        json.Should().Contain("\"features\":");
        json.Should().Contain("\"state\":");
    }

    [Fact]
    public void LayerFeatureCollection_Should_Deserialize_Via_LayerFeature_Discriminator()
    {
        // Arrange
        var json = """
                   {
                        "type":"FeatureCollection",
                        "source":"my-source",
                        "state":{"hover":true},
                        "features":[
                            {
                                "type":"Feature",
                                "id":"point1",
                                "geometry":{"type":"Point","coordinates":[-122.4194,37.7749]}
                            }
                        ]
                   }
                   """;

        // Act
        var layerFeature = JsonSerializer.Deserialize<LayerFeature>(json);

        // Assert
        layerFeature.Should().NotBeNull();
        layerFeature.Should().BeOfType<LayerFeatureCollection>();
        var collection = (LayerFeatureCollection)layerFeature!;
        collection.Source.Should().Be("my-source");
        collection.State.Should().ContainKey("hover");
        collection.Features.Should().HaveCount(1);
        collection.Features[0].Should().BeOfType<FeatureFeature>();
    }

    [Fact]
    public void LayerFeatureFeature_Should_Deserialize_Via_LayerFeature_Discriminator()
    {
        // Arrange - exercises the "Feature" branch of the LayerFeature discriminator
        var json = """
                   {
                        "type":"Feature",
                        "source":"my-source",
                        "id":"feature1",
                        "geometry":{"type":"Point","coordinates":[0.0,0.0]}
                   }
                   """;

        // Act
        var layerFeature = JsonSerializer.Deserialize<LayerFeature>(json);

        // Assert
        layerFeature.Should().NotBeNull();
        layerFeature.Should().BeOfType<LayerFeatureFeature>();
    }

    [Fact]
    public void LayerFeatureCollection_State_Should_Default_To_Empty_Dictionary()
    {
        // Arrange & Act
        var collection = new LayerFeatureCollection { Source = "my-source" };

        // Assert
        collection.State.Should().NotBeNull();
        collection.State.Should().BeEmpty();
    }

    [Fact]
    public void LayerFeatureCollection_Features_Should_Default_To_Empty_List()
    {
        // Arrange & Act
        var collection = new LayerFeatureCollection { Source = "my-source" };

        // Assert
        collection.Features.Should().NotBeNull();
        collection.Features.Should().BeEmpty();
    }

    [Fact]
    public void LayerFeatureCollection_GetBounds_Should_Return_Zero_Bounds_When_Empty()
    {
        // Arrange
        var collection = new LayerFeatureCollection { Source = "my-source" };

        // Act
        var bounds = collection.GetBounds();

        // Assert
        bounds.Should().NotBeNull();
        bounds.Southwest.Longitude.Should().Be(0);
        bounds.Southwest.Latitude.Should().Be(0);
        bounds.Northeast.Longitude.Should().Be(0);
        bounds.Northeast.Latitude.Should().Be(0);
    }

    [Fact]
    public void LayerFeatureCollection_GetBounds_Should_Extend_Across_All_Features()
    {
        // Arrange
        var collection = new LayerFeatureCollection
        {
            Source = "my-source",
            Features = new List<IFeature>
            {
                new FeatureFeature
                {
                    Geometry = new PointGeometry { Coordinates = new[] { -122.4194, 37.7749 } }
                },
                new FeatureFeature
                {
                    Geometry = new PointGeometry { Coordinates = new[] { -122.4184, 37.7739 } }
                }
            }
        };

        // Act
        var bounds = collection.GetBounds();

        // Assert
        bounds.Southwest.Longitude.Should().Be(-122.4194);
        bounds.Southwest.Latitude.Should().Be(37.7739);
        bounds.Northeast.Longitude.Should().Be(-122.4184);
        bounds.Northeast.Latitude.Should().Be(37.7749);
    }

    [Fact]
    public void LayerFeatureCollection_Source_Should_RoundTrip()
    {
        // Arrange
        var collection = new LayerFeatureCollection { Source = "roads" };

        // Act
        var json = JsonSerializer.Serialize<LayerFeature>(collection);
        var deserialized = JsonSerializer.Deserialize<LayerFeature>(json);

        // Assert
        deserialized.Should().BeOfType<LayerFeatureCollection>();
        ((LayerFeatureCollection)deserialized!).Source.Should().Be("roads");
    }
}
