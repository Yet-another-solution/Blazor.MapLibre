using System.Text.Json;
using Community.Blazor.MapLibre.Models.Feature;
using Community.Blazor.MapLibre.Models.Sources;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests;

/// <summary>
/// Tests to verify that IFeature serialization works correctly with .NET 9 and .NET 10.
/// This ensures the fix for Polymorphism_PropertyConflictsWithMetadataPropertyName works.
/// </summary>
public class FeatureSerializationTests
{
    [Fact]
    public void FeatureFeature_Should_Serialize_Successfully()
    {
        // Arrange
        var feature = new FeatureFeature
        {
            Id = "feature1",
            Geometry = new PointGeometry
            {
                Coordinates = new[] { -122.4194, 37.7749 }
            },
            Properties = new Dictionary<string, object>
            {
                { "name", "San Francisco" },
                { "population", 873965 }
            }
        };

        // Act
        var json = JsonSerializer.Serialize<IFeature>(feature);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"type\":\"Feature\"");
        json.Should().Contain("\"id\":\"feature1\"");
        json.Should().Contain("\"geometry\":");
        json.Should().Contain("\"properties\":");
        json.Should().Contain("\"name\":\"San Francisco\"");
    }

    [Fact]
    public void FeatureFeature_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = @"{
            ""$type"":""Feature"",
            ""id"":""feature1"",
            ""type"":""Feature"",
            ""geometry"":{
                ""$type"":""Point"",
                ""type"":""Point"",
                ""coordinates"":[-122.4194,37.7749]
            },
            ""properties"":{
                ""name"":""San Francisco""
            }
        }";

        // Act
        var feature = JsonSerializer.Deserialize<IFeature>(json);

        // Assert
        feature.Should().NotBeNull();
        feature.Should().BeOfType<FeatureFeature>();
        var featureFeature = (FeatureFeature)feature!;
        featureFeature.Type.Should().Be("Feature");
        featureFeature.Id.Should().Be("feature1");
        featureFeature.Geometry.Should().BeOfType<PointGeometry>();
        featureFeature.Properties.Should().ContainKey("name");
    }

    [Fact]
    public void FeatureCollection_Should_Serialize_Successfully()
    {
        // Arrange
        var featureCollection = new FeatureCollection
        {
            Features = new List<IFeature>
            {
                new FeatureFeature
                {
                    Id = "point1",
                    Geometry = new PointGeometry
                    {
                        Coordinates = new[] { -122.4194, 37.7749 }
                    },
                    Properties = new Dictionary<string, object>
                    {
                        { "name", "San Francisco" }
                    }
                },
                new FeatureFeature
                {
                    Id = "line1",
                    Geometry = new LineGeometry
                    {
                        Coordinates = new[]
                        {
                            new[] { -122.4194, 37.7749 },
                            new[] { -122.4184, 37.7739 }
                        }
                    },
                    Properties = new Dictionary<string, object>
                    {
                        { "name", "Market Street" }
                    }
                }
            }
        };

        // Act
        var json = JsonSerializer.Serialize<IFeature>(featureCollection);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"type\":\"FeatureCollection\"");
        json.Should().Contain("\"features\":");
        json.Should().Contain("\"San Francisco\"");
        json.Should().Contain("\"Market Street\"");
    }

    [Fact]
    public void FeatureCollection_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = @"{
            ""$type"":""FeatureCollection"",
            ""type"":""FeatureCollection"",
            ""features"":[
                {
                    ""$type"":""Feature"",
                    ""id"":""point1"",
                    ""type"":""Feature"",
                    ""geometry"":{
                        ""$type"":""Point"",
                        ""type"":""Point"",
                        ""coordinates"":[-122.4194,37.7749]
                    },
                    ""properties"":{
                        ""name"":""San Francisco""
                    }
                }
            ]
        }";

        // Act
        var feature = JsonSerializer.Deserialize<IFeature>(json);

        // Assert
        feature.Should().NotBeNull();
        feature.Should().BeOfType<FeatureCollection>();
        var featureCollection = (FeatureCollection)feature!;
        featureCollection.Type.Should().Be("FeatureCollection");
        featureCollection.Features.Should().HaveCount(1);
        featureCollection.Features[0].Should().BeOfType<FeatureFeature>();
    }

    [Fact]
    public void GeoJsonSource_With_FeatureCollection_Should_Serialize_Successfully()
    {
        // Arrange - This simulates the actual use case from the issue
        var features = new List<IFeature>
        {
            new FeatureFeature
            {
                Id = "resource1",
                Geometry = new PointGeometry
                {
                    Coordinates = new[] { -122.4194, 37.7749 }
                },
                Properties = new Dictionary<string, object>
                {
                    { "type", "office" },
                    { "name", "Main Office" }
                }
            }
        };

        var source = new GeoJsonSource
        {
            Data = new FeatureCollection
            {
                Features = features
            }
        };

        // Act
        var json = JsonSerializer.Serialize(source);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"type\":\"geojson\"");
        json.Should().Contain("\"FeatureCollection\"");
        json.Should().Contain("\"Main Office\"");
    }

    [Fact]
    public void Serialization_Should_Not_Throw_PropertyConflictsWithMetadataPropertyName_Exception()
    {
        // Arrange
        var featureCollection = new FeatureCollection
        {
            Features = new List<IFeature>
            {
                new FeatureFeature
                {
                    Id = "test",
                    Geometry = new PointGeometry
                    {
                        Coordinates = new[] { 0.0, 0.0 }
                    }
                }
            }
        };

        // Act
        Action act = () => JsonSerializer.Serialize<IFeature>(featureCollection);

        // Assert
        act.Should().NotThrow<InvalidOperationException>("the fix should resolve the .NET 10 polymorphism conflict");
    }

    [Fact]
    public void Empty_FeatureCollection_Should_Serialize_Successfully()
    {
        // Arrange
        var featureCollection = new FeatureCollection
        {
            Features = new List<IFeature>()
        };

        // Act
        var json = JsonSerializer.Serialize<IFeature>(featureCollection);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"type\":\"FeatureCollection\"");
        json.Should().Contain("\"features\":[]");
    }

    [Fact]
    public void GetBounds_Should_Work_On_FeatureFeature()
    {
        // Arrange
        var feature = new FeatureFeature
        {
            Geometry = new PointGeometry
            {
                Coordinates = new[] { -122.4194, 37.7749 }
            }
        };

        // Act
        var bounds = feature.GetBounds();

        // Assert
        bounds.Should().NotBeNull();
        bounds.Southwest.Longitude.Should().Be(-122.4194);
        bounds.Southwest.Latitude.Should().Be(37.7749);
    }

    [Fact]
    public void GetBounds_Should_Work_On_FeatureCollection()
    {
        // Arrange
        var featureCollection = new FeatureCollection
        {
            Features = new List<IFeature>
            {
                new FeatureFeature
                {
                    Geometry = new PointGeometry
                    {
                        Coordinates = new[] { -122.4194, 37.7749 }
                    }
                },
                new FeatureFeature
                {
                    Geometry = new PointGeometry
                    {
                        Coordinates = new[] { -122.4184, 37.7739 }
                    }
                }
            }
        };

        // Act
        var bounds = featureCollection.GetBounds();

        // Assert
        bounds.Should().NotBeNull();
        bounds.Southwest.Longitude.Should().BeLessThanOrEqualTo(-122.4184);
        bounds.Southwest.Latitude.Should().BeLessThanOrEqualTo(37.7749);
    }

    [Fact]
    public void FeatureCollection_With_Mixed_Geometry_Types_Should_Serialize_Successfully()
    {
        // Arrange
        var featureCollection = new FeatureCollection
        {
            Features = new List<IFeature>
            {
                new FeatureFeature
                {
                    Id = "point",
                    Geometry = new PointGeometry { Coordinates = new[] { 0.0, 0.0 } }
                },
                new FeatureFeature
                {
                    Id = "line",
                    Geometry = new LineGeometry
                    {
                        Coordinates = new[] { new[] { 0.0, 0.0 }, new[] { 1.0, 1.0 } }
                    }
                },
                new FeatureFeature
                {
                    Id = "polygon",
                    Geometry = new PolygonGeometry
                    {
                        Coordinates = new[]
                        {
                            new[]
                            {
                                new[] { 0.0, 0.0 },
                                new[] { 1.0, 0.0 },
                                new[] { 1.0, 1.0 },
                                new[] { 0.0, 0.0 }
                            }
                        }
                    }
                }
            }
        };

        // Act
        var json = JsonSerializer.Serialize<IFeature>(featureCollection);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"Point\"");
        json.Should().Contain("\"LineString\"");
        json.Should().Contain("\"Polygon\"");
    }

    [Fact]
    public void GeoJsonSource_With_URL_String_Should_Serialize_Successfully()
    {
        // Arrange - This is the exact scenario from GitHub issue #118
        var geoJsonSource = new GeoJsonSource
        {
            Data = "https://apigeoportal.rcpod.space/api/Torches"
        };

        // Act
        var json = JsonSerializer.Serialize(geoJsonSource);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"type\":\"geojson\"");
        json.Should().Contain("\"data\":\"https://apigeoportal.rcpod.space/api/Torches\"");
    }

    [Fact]
    public void GeoJsonSource_With_Inline_Feature_Should_Serialize_Successfully()
    {
        // Arrange
        var geoJsonSource = new GeoJsonSource
        {
            Data = new FeatureFeature
            {
                Id = "test-feature",
                Geometry = new PointGeometry
                {
                    Coordinates = new[] { -122.4194, 37.7749 }
                },
                Properties = new Dictionary<string, object>
                {
                    { "name", "Test Point" }
                }
            }
        };

        // Act
        var json = JsonSerializer.Serialize(geoJsonSource);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"type\":\"geojson\"");
        json.Should().Contain("\"type\":\"Feature\"");
        json.Should().Contain("\"name\":\"Test Point\"");
    }

    [Fact]
    public void GeoJsonSource_With_URL_Should_Be_Assignable_From_String()
    {
        // Arrange & Act - OneOf should support implicit conversion from string
        var source = new GeoJsonSource
        {
            Data = "https://example.com/data.geojson"
        };

        // Assert
        source.Should().NotBeNull();
        source.Data.IsT1.Should().BeTrue("Data should be recognized as the second type (string)");
        source.Data.AsT1.Should().Be("https://example.com/data.geojson");
    }

    [Fact]
    public void GeoJsonSource_With_Feature_Should_Be_Assignable_From_IFeature()
    {
        // Arrange
        var feature = new FeatureFeature
        {
            Geometry = new PointGeometry { Coordinates = new[] { 0.0, 0.0 } }
        };

        // Act - OneOf should support implicit conversion from IFeature
        var source = new GeoJsonSource
        {
            Data = feature
        };

        // Assert
        source.Should().NotBeNull();
        source.Data.IsT0.Should().BeTrue("Data should be recognized as the first type (IFeature)");
        source.Data.AsT0.Should().Be(feature);
    }
}
