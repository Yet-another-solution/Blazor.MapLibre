using System.Text.Json;
using Community.Blazor.MapLibre.Models.Feature;
using Community.Blazor.MapLibre.Models.Sources;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests;

/// <summary>
/// Integration tests that simulate real-world usage scenarios from the issue report.
/// These tests verify that the exact code sample from the issue works correctly.
/// </summary>
public class RealWorldScenarioTests
{
    [Fact]
    public void SetSourceData_Scenario_Should_Serialize_Without_Throwing()
    {
        // Arrange - This is the exact scenario from the issue report
        var features = new List<IFeature>
        {
            new FeatureFeature
            {
                Id = "resource1",
                Geometry = new PointGeometry
                {
                    Coordinates = new[] { -73.935242, 40.730610 } // New York
                },
                Properties = new Dictionary<string, object>
                {
                    { "type", "resource" },
                    { "name", "Resource 1" },
                    { "status", "active" }
                }
            },
            new FeatureFeature
            {
                Id = "resource2",
                Geometry = new LineGeometry
                {
                    Coordinates = new[]
                    {
                        new[] { -73.935242, 40.730610 },
                        new[] { -73.925242, 40.720610 }
                    }
                },
                Properties = new Dictionary<string, object>
                {
                    { "type", "route" },
                    { "name", "Route A" }
                }
            },
            new FeatureFeature
            {
                Id = "resource3",
                Geometry = new PolygonGeometry
                {
                    Coordinates = new[]
                    {
                        new[]
                        {
                            new[] { -73.935242, 40.730610 },
                            new[] { -73.925242, 40.730610 },
                            new[] { -73.925242, 40.720610 },
                            new[] { -73.935242, 40.720610 },
                            new[] { -73.935242, 40.730610 }
                        }
                    }
                },
                Properties = new Dictionary<string, object>
                {
                    { "type", "area" },
                    { "name", "Zone 1" }
                }
            }
        };

        var geoJsonSource = new GeoJsonSource
        {
            Data = new FeatureCollection
            {
                Features = features
            }
        };

        // Act
        Action act = () => JsonSerializer.Serialize(geoJsonSource);

        // Assert - Should not throw Polymorphism_PropertyConflictsWithMetadataPropertyName
        act.Should().NotThrow<InvalidOperationException>(
            "the fix should allow SetSourceData scenario to work with .NET 10");
    }

    [Fact]
    public void SetSourceData_Scenario_Should_Produce_Valid_GeoJson()
    {
        // Arrange
        var features = new List<IFeature>
        {
            new FeatureFeature
            {
                Id = "resource1",
                Geometry = new PointGeometry
                {
                    Coordinates = new[] { -73.935242, 40.730610 }
                },
                Properties = new Dictionary<string, object>
                {
                    { "category", "landmark" }
                }
            }
        };

        var geoJsonSource = new GeoJsonSource
        {
            Data = new FeatureCollection
            {
                Features = features
            }
        };

        // Act
        var json = JsonSerializer.Serialize(geoJsonSource);

        // Assert - Verify the output is valid GeoJSON-like structure
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"type\":\"geojson\"");
        json.Should().Contain("\"type\":\"FeatureCollection\"");
        json.Should().Contain("\"type\":\"Feature\"");
        json.Should().Contain("\"type\":\"Point\"");
        json.Should().Contain("\"coordinates\":[-73.935242,40.73061]");
        json.Should().Contain("\"category\":\"landmark\"");
    }

    [Fact]
    public void Complex_FeatureCollection_With_Multiple_Geometry_Types_Should_Serialize()
    {
        // Arrange - Complex real-world scenario with all geometry types
        var features = new List<IFeature>
        {
            new FeatureFeature
            {
                Id = "point",
                Geometry = new PointGeometry { Coordinates = new[] { 0.0, 0.0 } },
                Properties = new Dictionary<string, object> { { "type", "marker" } }
            },
            new FeatureFeature
            {
                Id = "line",
                Geometry = new LineGeometry
                {
                    Coordinates = new[] { new[] { 0.0, 0.0 }, new[] { 1.0, 1.0 } }
                },
                Properties = new Dictionary<string, object> { { "type", "path" } }
            },
            new FeatureFeature
            {
                Id = "polygon",
                Geometry = new PolygonGeometry
                {
                    Coordinates = new[]
                    {
                        new[] {
                            new[] { 0.0, 0.0 },
                            new[] { 1.0, 0.0 },
                            new[] { 1.0, 1.0 },
                            new[] { 0.0, 1.0 },
                            new[] { 0.0, 0.0 }
                        }
                    }
                },
                Properties = new Dictionary<string, object> { { "type", "zone" } }
            },
            new FeatureFeature
            {
                Id = "multipolygon",
                Geometry = new MultiPolygonGeometry
                {
                    Coordinates = new[]
                    {
                        new[]
                        {
                            new[] {
                                new[] { 0.0, 0.0 },
                                new[] { 1.0, 0.0 },
                                new[] { 1.0, 1.0 },
                                new[] { 0.0, 0.0 }
                            }
                        }
                    }
                },
                Properties = new Dictionary<string, object> { { "type", "regions" } }
            }
        };

        var geoJsonSource = new GeoJsonSource
        {
            Data = new FeatureCollection
            {
                Features = features
            }
        };

        // Act
        var json = JsonSerializer.Serialize(geoJsonSource);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"Point\"");
        json.Should().Contain("\"LineString\"");
        json.Should().Contain("\"Polygon\"");
        json.Should().Contain("\"MultiPolygon\"");
    }

    [Fact]
    public void Round_Trip_Serialization_Should_Work()
    {
        // Arrange
        var original = new GeoJsonSource
        {
            Data = new FeatureCollection
            {
                Features = new List<IFeature>
                {
                    new FeatureFeature
                    {
                        Id = "test",
                        Geometry = new PointGeometry
                        {
                            Coordinates = new[] { -122.4194, 37.7749 }
                        },
                        Properties = new Dictionary<string, object>
                        {
                            { "name", "Test Point" }
                        }
                    }
                }
            }
        };

        // Act
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<GeoJsonSource>(json);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Type.Should().Be("geojson");
        deserialized.Data.AsT0.Should().BeOfType<FeatureCollection>();

        var featureCollection = (FeatureCollection)deserialized.Data.AsT0;
        featureCollection.Features.Should().HaveCount(1);
        featureCollection.Features[0].Should().BeOfType<FeatureFeature>();

        var feature = (FeatureFeature)featureCollection.Features[0];
        feature.Id.Should().Be("test");
        feature.Geometry.Should().BeOfType<PointGeometry>();

        var point = (PointGeometry)feature.Geometry;
        point.Coordinates.Should().BeEquivalentTo(new[] { -122.4194, 37.7749 });
    }

    [Fact]
    public void FeatureCollection_With_Null_Properties_Should_Serialize()
    {
        // Arrange
        var features = new List<IFeature>
        {
            new FeatureFeature
            {
                Id = "no-props",
                Geometry = new PointGeometry
                {
                    Coordinates = new[] { 0.0, 0.0 }
                },
                Properties = null
            }
        };

        var geoJsonSource = new GeoJsonSource
        {
            Data = new FeatureCollection
            {
                Features = features
            }
        };

        // Act
        Action act = () => JsonSerializer.Serialize(geoJsonSource);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Large_FeatureCollection_Should_Serialize_Efficiently()
    {
        // Arrange - Create a larger dataset to test performance
        var features = Enumerable.Range(0, 100).Select(i => new FeatureFeature
        {
            Id = $"feature{i}",
            Geometry = new PointGeometry
            {
                Coordinates = new[] { -122.0 + i * 0.01, 37.0 + i * 0.01 }
            },
            Properties = new Dictionary<string, object>
            {
                { "index", i },
                { "name", $"Feature {i}" }
            }
        }).Cast<IFeature>().ToList();

        var geoJsonSource = new GeoJsonSource
        {
            Data = new FeatureCollection
            {
                Features = features
            }
        };

        // Act
        var startTime = DateTime.UtcNow;
        var json = JsonSerializer.Serialize(geoJsonSource);
        var duration = DateTime.UtcNow - startTime;

        // Assert
        json.Should().NotBeNullOrEmpty();
        duration.Should().BeLessThan(TimeSpan.FromSeconds(1),
            "serialization should be reasonably fast");

        // Verify all features are included
        for (int i = 0; i < 100; i++)
        {
            json.Should().Contain($"feature{i}");
        }
    }

    [Fact]
    public void SetSourceData_Data_Extraction_Should_Serialize_Properly()
    {
        // Arrange - Simulate what SetSourceData does internally
        var geoJsonSource = new GeoJsonSource
        {
            Data = new FeatureCollection
            {
                Features = new List<IFeature>
                {
                    new FeatureFeature
                    {
                        Id = "test",
                        Geometry = new PointGeometry
                        {
                            Coordinates = new[] { -122.4194, 37.7749 }
                        },
                        Properties = new Dictionary<string, object>
                        {
                            { "name", "Test Location" }
                        }
                    }
                }
            }
        };

        // Act - This simulates the SetSourceData fix
        var json = JsonSerializer.Serialize(geoJsonSource);
        using var jsonDoc = JsonDocument.Parse(json);
        var dataElement = jsonDoc.RootElement.GetProperty("data");
        var dataJson = JsonSerializer.Serialize(dataElement);

        // Assert - The extracted data should be valid GeoJSON
        dataJson.Should().NotBeNullOrEmpty();
        dataJson.Should().Contain("\"type\":\"FeatureCollection\"");
        dataJson.Should().Contain("\"type\":\"Feature\"");
        dataJson.Should().Contain("\"type\":\"Point\"");
        dataJson.Should().Contain("\"name\":\"Test Location\"");
        dataJson.Should().NotContain("\"type\":\"geojson\"",
            "the data field should not contain the source type");
    }

    [Fact]
    public void SetSourceData_With_URL_String_Should_Extract_Properly()
    {
        // Arrange
        var geoJsonSource = new GeoJsonSource
        {
            Data = "https://example.com/data.geojson"
        };

        // Act - This simulates the SetSourceData fix
        var json = JsonSerializer.Serialize(geoJsonSource);
        using var jsonDoc = JsonDocument.Parse(json);
        var dataElement = jsonDoc.RootElement.GetProperty("data");

        // Assert
        dataElement.ValueKind.Should().Be(JsonValueKind.String);
        dataElement.GetString().Should().Be("https://example.com/data.geojson");
    }
}
