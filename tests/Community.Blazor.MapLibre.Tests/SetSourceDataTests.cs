using System.Text.Json;
using Community.Blazor.MapLibre.Models.Feature;
using Community.Blazor.MapLibre.Models.Sources;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests;

/// <summary>
/// Comprehensive tests for the SetSourceData method to ensure proper serialization
/// of GeoJSON data with the GeoJsonDataConverter applied correctly.
/// </summary>
public class SetSourceDataTests
{
    [Fact]
    public void GeoJsonSource_Serialization_Should_Apply_Converter_To_FeatureCollection()
    {
        // Arrange
        var source = new GeoJsonSource
        {
            Data = new FeatureCollection
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
                    }
                }
            }
        };

        // Act
        var json = JsonSerializer.Serialize(source);
        using var jsonDoc = JsonDocument.Parse(json);
        var dataElement = jsonDoc.RootElement.GetProperty("data");

        // Assert
        dataElement.ValueKind.Should().Be(JsonValueKind.Object);
        dataElement.GetProperty("type").GetString().Should().Be("FeatureCollection");
        dataElement.GetProperty("features").ValueKind.Should().Be(JsonValueKind.Array);
        dataElement.GetProperty("features").GetArrayLength().Should().Be(1);
    }

    [Fact]
    public void GeoJsonSource_Serialization_Should_Apply_Converter_To_Single_Feature()
    {
        // Arrange
        var source = new GeoJsonSource
        {
            Data = new FeatureFeature
            {
                Id = "single-point",
                Geometry = new PointGeometry
                {
                    Coordinates = new[] { 10.0, 20.0 }
                },
                Properties = new Dictionary<string, object>
                {
                    { "category", "landmark" }
                }
            }
        };

        // Act
        var json = JsonSerializer.Serialize(source);
        using var jsonDoc = JsonDocument.Parse(json);
        var dataElement = jsonDoc.RootElement.GetProperty("data");

        // Assert
        dataElement.ValueKind.Should().Be(JsonValueKind.Object);
        dataElement.GetProperty("type").GetString().Should().Be("Feature");
        dataElement.GetProperty("id").GetString().Should().Be("single-point");
        dataElement.GetProperty("geometry").GetProperty("type").GetString().Should().Be("Point");
    }

    [Fact]
    public void GeoJsonSource_Serialization_Should_Apply_Converter_To_URL_String()
    {
        // Arrange
        var source = new GeoJsonSource
        {
            Data = "https://api.example.com/geojson/data.json"
        };

        // Act
        var json = JsonSerializer.Serialize(source);
        using var jsonDoc = JsonDocument.Parse(json);
        var dataElement = jsonDoc.RootElement.GetProperty("data");

        // Assert
        dataElement.ValueKind.Should().Be(JsonValueKind.String);
        dataElement.GetString().Should().Be("https://api.example.com/geojson/data.json");
    }

    [Fact]
    public void Extracted_Data_Should_Not_Contain_Source_Type()
    {
        // Arrange - The extracted data should only contain GeoJSON, not the source wrapper
        var source = new GeoJsonSource
        {
            Data = new FeatureCollection
            {
                Features = new List<IFeature>
                {
                    new FeatureFeature
                    {
                        Geometry = new PointGeometry { Coordinates = new[] { 0.0, 0.0 } }
                    }
                }
            },
            Cluster = true,
            ClusterRadius = 50
        };

        // Act
        var json = JsonSerializer.Serialize(source);
        using var jsonDoc = JsonDocument.Parse(json);
        var dataElement = jsonDoc.RootElement.GetProperty("data");
        var dataJson = dataElement.ToString();

        // Assert
        dataJson.Should().NotContain("geojson", "data field should not contain source type");
        dataJson.Should().NotContain("cluster", "data field should not contain source properties");
        dataJson.Should().Contain("FeatureCollection");
    }

    [Fact]
    public void Complex_FeatureCollection_Should_Serialize_All_Geometry_Types()
    {
        // Arrange
        var source = new GeoJsonSource
        {
            Data = new FeatureCollection
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
                                    new[] { 0.0, 1.0 },
                                    new[] { 0.0, 0.0 }
                                }
                            }
                        }
                    }
                }
            }
        };

        // Act
        var json = JsonSerializer.Serialize(source);
        using var jsonDoc = JsonDocument.Parse(json);
        var dataElement = jsonDoc.RootElement.GetProperty("data");
        var dataJson = dataElement.ToString();

        // Assert
        dataJson.Should().Contain("Point");
        dataJson.Should().Contain("LineString");
        dataJson.Should().Contain("Polygon");

        var features = dataElement.GetProperty("features");
        features.GetArrayLength().Should().Be(3);
    }

    [Fact]
    public void Empty_FeatureCollection_Should_Serialize_Correctly()
    {
        // Arrange
        var source = new GeoJsonSource
        {
            Data = new FeatureCollection
            {
                Features = new List<IFeature>()
            }
        };

        // Act
        var json = JsonSerializer.Serialize(source);
        using var jsonDoc = JsonDocument.Parse(json);
        var dataElement = jsonDoc.RootElement.GetProperty("data");

        // Assert
        dataElement.GetProperty("type").GetString().Should().Be("FeatureCollection");
        dataElement.GetProperty("features").GetArrayLength().Should().Be(0);
    }

    [Fact]
    public void Feature_With_Null_Properties_Should_Serialize()
    {
        // Arrange
        var source = new GeoJsonSource
        {
            Data = new FeatureFeature
            {
                Id = "no-props",
                Geometry = new PointGeometry { Coordinates = new[] { 0.0, 0.0 } },
                Properties = null
            }
        };

        // Act
        Action act = () =>
        {
            var json = JsonSerializer.Serialize(source);
            using var jsonDoc = JsonDocument.Parse(json);
            var dataElement = jsonDoc.RootElement.GetProperty("data");
        };

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Feature_With_Complex_Properties_Should_Serialize()
    {
        // Arrange
        var source = new GeoJsonSource
        {
            Data = new FeatureFeature
            {
                Id = "complex",
                Geometry = new PointGeometry { Coordinates = new[] { -122.4194, 37.7749 } },
                Properties = new Dictionary<string, object>
                {
                    { "name", "Complex Feature" },
                    { "count", 42 },
                    { "active", true },
                    { "rating", 4.5 },
                    { "tags", new[] { "tag1", "tag2", "tag3" } },
                    { "metadata", new Dictionary<string, object>
                        {
                            { "created", "2024-01-01" },
                            { "updated", "2024-01-15" }
                        }
                    }
                }
            }
        };

        // Act
        var json = JsonSerializer.Serialize(source);
        using var jsonDoc = JsonDocument.Parse(json);
        var dataElement = jsonDoc.RootElement.GetProperty("data");
        var dataJson = dataElement.ToString();

        // Assert
        dataJson.Should().Contain("Complex Feature");
        dataJson.Should().Contain("42");
        dataJson.Should().Contain("true");
        dataJson.Should().Contain("4.5");
        dataJson.Should().Contain("tag1");
        dataJson.Should().Contain("metadata");
    }

    [Fact]
    public void Large_FeatureCollection_Should_Extract_Data_Efficiently()
    {
        // Arrange
        var features = Enumerable.Range(0, 1000).Select(i => new FeatureFeature
        {
            Id = $"feature-{i}",
            Geometry = new PointGeometry
            {
                Coordinates = new[] { -180.0 + i * 0.36, -90.0 + i * 0.18 }
            },
            Properties = new Dictionary<string, object>
            {
                { "index", i },
                { "name", $"Feature {i}" }
            }
        }).Cast<IFeature>().ToList();

        var source = new GeoJsonSource
        {
            Data = new FeatureCollection { Features = features }
        };

        // Act
        var startTime = DateTime.UtcNow;
        var json = JsonSerializer.Serialize(source);
        using var jsonDoc = JsonDocument.Parse(json);
        var dataElement = jsonDoc.RootElement.GetProperty("data");
        var duration = DateTime.UtcNow - startTime;

        // Assert
        duration.Should().BeLessThan(TimeSpan.FromSeconds(2),
            "extracting data from large collection should be fast");
        dataElement.GetProperty("features").GetArrayLength().Should().Be(1000);
    }

    [Fact]
    public void MultiPolygon_Geometry_Should_Serialize_Correctly()
    {
        // Arrange
        var source = new GeoJsonSource
        {
            Data = new FeatureFeature
            {
                Id = "multi-poly",
                Geometry = new MultiPolygonGeometry
                {
                    Coordinates = new[]
                    {
                        new[]
                        {
                            new[]
                            {
                                new[] { 0.0, 0.0 },
                                new[] { 1.0, 0.0 },
                                new[] { 1.0, 1.0 },
                                new[] { 0.0, 1.0 },
                                new[] { 0.0, 0.0 }
                            }
                        },
                        new[]
                        {
                            new[]
                            {
                                new[] { 2.0, 2.0 },
                                new[] { 3.0, 2.0 },
                                new[] { 3.0, 3.0 },
                                new[] { 2.0, 3.0 },
                                new[] { 2.0, 2.0 }
                            }
                        }
                    }
                }
            }
        };

        // Act
        var json = JsonSerializer.Serialize(source);
        using var jsonDoc = JsonDocument.Parse(json);
        var dataElement = jsonDoc.RootElement.GetProperty("data");

        // Assert
        dataElement.GetProperty("geometry").GetProperty("type").GetString()
            .Should().Be("MultiPolygon");
        dataElement.GetProperty("geometry").GetProperty("coordinates").GetArrayLength()
            .Should().Be(2);
    }

    [Fact]
    public void GeoJsonSource_With_All_Optional_Properties_Should_Not_Affect_Data_Extraction()
    {
        // Arrange
        var source = new GeoJsonSource
        {
            Data = new FeatureCollection
            {
                Features = new List<IFeature>
                {
                    new FeatureFeature
                    {
                        Geometry = new PointGeometry { Coordinates = new[] { 0.0, 0.0 } }
                    }
                }
            },
            Cluster = true,
            ClusterMaxZoom = 14,
            ClusterRadius = 50,
            ClusterMinPoints = 2,
            GenerateId = true,
            Buffer = 128,
            Tolerance = 0.375f,
            LineMetrics = true,
            MinZoom = 0,
            MaxZoom = 22,
            TileSize = 512,
            Attribution = "© Test Attribution"
        };

        // Act
        var json = JsonSerializer.Serialize(source);
        using var jsonDoc = JsonDocument.Parse(json);
        var dataElement = jsonDoc.RootElement.GetProperty("data");

        // Assert - The data element should only contain the GeoJSON data
        dataElement.GetProperty("type").GetString().Should().Be("FeatureCollection");
        dataElement.TryGetProperty("cluster", out _).Should().BeFalse(
            "data should not contain source configuration");
        dataElement.TryGetProperty("buffer", out _).Should().BeFalse(
            "data should not contain source configuration");
    }

    [Fact]
    public void JsonNode_Should_Work_For_Bulk_Transactions()
    {
        // Arrange - Test that JsonNode works for bulk transactions (no Clone needed)
        var source = new GeoJsonSource
        {
            Data = new FeatureCollection
            {
                Features = new List<IFeature>
                {
                    new FeatureFeature
                    {
                        Id = "node-test",
                        Geometry = new PointGeometry { Coordinates = new[] { 1.0, 2.0 } }
                    }
                }
            }
        };

        // Act - Using JsonNode (simpler approach)
        var jsonNode = JsonSerializer.SerializeToNode(source);
        var dataNode = jsonNode!["data"];

        // Assert - Node should serialize correctly
        var nodeJson = JsonSerializer.Serialize(dataNode);
        nodeJson.Should().Contain("FeatureCollection");
        nodeJson.Should().Contain("node-test");
    }

    [Fact]
    public void Relative_URL_String_Should_Serialize_Correctly()
    {
        // Arrange
        var source = new GeoJsonSource
        {
            Data = "/api/geojson/features"
        };

        // Act
        var json = JsonSerializer.Serialize(source);
        using var jsonDoc = JsonDocument.Parse(json);
        var dataElement = jsonDoc.RootElement.GetProperty("data");

        // Assert
        dataElement.ValueKind.Should().Be(JsonValueKind.String);
        dataElement.GetString().Should().Be("/api/geojson/features");
    }

    [Fact]
    public void Feature_With_Numeric_String_Id_Should_Serialize()
    {
        // Arrange
        var source = new GeoJsonSource
        {
            Data = new FeatureFeature
            {
                Id = "12345",
                Geometry = new PointGeometry { Coordinates = new[] { 0.0, 0.0 } }
            }
        };

        // Act
        var json = JsonSerializer.Serialize(source);
        using var jsonDoc = JsonDocument.Parse(json);
        var dataElement = jsonDoc.RootElement.GetProperty("data");

        // Assert
        dataElement.GetProperty("id").GetString().Should().Be("12345");
    }
}
