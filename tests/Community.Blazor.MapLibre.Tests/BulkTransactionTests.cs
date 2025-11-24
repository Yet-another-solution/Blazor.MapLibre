using System.Text.Json;
using Community.Blazor.MapLibre.Models;
using Community.Blazor.MapLibre.Models.Feature;
using Community.Blazor.MapLibre.Models.Sources;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests;

/// <summary>
/// Tests for bulk transaction support, especially for setSourceData operations.
/// </summary>
public class BulkTransactionTests
{
    [Fact]
    public void BulkTransaction_Should_Support_SetSourceData()
    {
        // Arrange
        var transaction = new BulkTransaction();
        var source = new GeoJsonSource
        {
            Data = new FeatureCollection
            {
                Features = new List<IFeature>
                {
                    new FeatureFeature
                    {
                        Id = "test",
                        Geometry = new PointGeometry { Coordinates = new[] { 0.0, 0.0 } }
                    }
                }
            }
        };

        // Simulate what SetSourceData does (using JsonNode - simpler!)
        var jsonNode = JsonSerializer.SerializeToNode(source);
        var dataNode = jsonNode!["data"];

        // Act
        transaction.Add("setSourceData", "test-source", dataNode);

        // Assert
        transaction.Transactions.Should().HaveCount(1);
        transaction.Transactions[0].Event.Should().Be("setSourceData");
        transaction.Transactions[0].Data.Should().HaveCount(2);
        transaction.Transactions[0].Data![0].Should().Be("test-source");
    }

    [Fact]
    public void BulkTransaction_Should_Serialize_SetSourceData_Correctly()
    {
        // Arrange
        var transaction = new BulkTransaction();
        var source = new GeoJsonSource
        {
            Data = new FeatureCollection
            {
                Features = new List<IFeature>
                {
                    new FeatureFeature
                    {
                        Id = "bulk-test",
                        Geometry = new PointGeometry { Coordinates = new[] { 10.0, 20.0 } },
                        Properties = new Dictionary<string, object>
                        {
                            { "name", "Bulk Transaction Test" }
                        }
                    }
                }
            }
        };

        var jsonNode = JsonSerializer.SerializeToNode(source);
        var dataNode = jsonNode!["data"];
        

        transaction.Add("setSourceData", "source-id", dataNode);

        // Act
        var transactionJson = JsonSerializer.Serialize(transaction.Transactions);

        // Assert
        transactionJson.Should().Contain("setSourceData");
        transactionJson.Should().Contain("source-id");
        transactionJson.Should().Contain("FeatureCollection");
        transactionJson.Should().Contain("Bulk Transaction Test");
    }

    [Fact]
    public void BulkTransaction_Should_Handle_Multiple_SetSourceData_Operations()
    {
        // Arrange
        var transaction = new BulkTransaction();

        var source1 = new GeoJsonSource
        {
            Data = new FeatureFeature
            {
                Id = "feature1",
                Geometry = new PointGeometry { Coordinates = new[] { 0.0, 0.0 } }
            }
        };

        var source2 = new GeoJsonSource
        {
            Data = "https://example.com/data.geojson"
        };

        var json1 = JsonSerializer.Serialize(source1);
        using var jsonDoc1 = JsonDocument.Parse(json1);
        var dataNode1 = jsonDoc1.RootElement.GetProperty("data").Clone();

        var json2 = JsonSerializer.Serialize(source2);
        using var jsonDoc2 = JsonDocument.Parse(json2);
        var dataNode2 = jsonDoc2.RootElement.GetProperty("data").Clone();

        // Act
        transaction.Add("setSourceData", "source1", dataNode1);
        transaction.Add("setSourceData", "source2", dataNode2);

        // Assert
        transaction.Transactions.Should().HaveCount(2);
        transaction.Transactions[0].Event.Should().Be("setSourceData");
        transaction.Transactions[1].Event.Should().Be("setSourceData");
    }

    [Fact]
    public void BulkTransaction_Should_Mix_SetSourceData_With_Other_Operations()
    {
        // Arrange
        var transaction = new BulkTransaction();

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
            }
        };

        var jsonNode = JsonSerializer.SerializeToNode(source);
        var dataNode = jsonNode!["data"];
        

        // Act - Mix different transaction types
        transaction.Add("addSource", "test-source", source);
        transaction.Add("setSourceData", "test-source", dataNode);
        transaction.Add("removeSource", "old-source");

        // Assert
        transaction.Transactions.Should().HaveCount(3);
        transaction.Transactions[0].Event.Should().Be("addSource");
        transaction.Transactions[1].Event.Should().Be("setSourceData");
        transaction.Transactions[2].Event.Should().Be("removeSource");
    }

    [Fact]
    public void BulkTransaction_SetSourceData_Should_Preserve_Complex_GeoJSON()
    {
        // Arrange
        var transaction = new BulkTransaction();
        var source = new GeoJsonSource
        {
            Data = new FeatureCollection
            {
                Features = new List<IFeature>
                {
                    new FeatureFeature
                    {
                        Id = "complex1",
                        Geometry = new PolygonGeometry
                        {
                            Coordinates = new[]
                            {
                                new[]
                                {
                                    new[] { 0.0, 0.0 },
                                    new[] { 10.0, 0.0 },
                                    new[] { 10.0, 10.0 },
                                    new[] { 0.0, 10.0 },
                                    new[] { 0.0, 0.0 }
                                }
                            }
                        },
                        Properties = new Dictionary<string, object>
                        {
                            { "area", "test-area" },
                            { "size", 100 },
                            { "tags", new[] { "tag1", "tag2" } }
                        }
                    }
                }
            }
        };

        var jsonNode = JsonSerializer.SerializeToNode(source);
        var dataNode = jsonNode!["data"];
        

        transaction.Add("setSourceData", "complex-source", dataNode);

        // Act
        var transactionJson = JsonSerializer.Serialize(transaction.Transactions);

        // Assert
        transactionJson.Should().Contain("Polygon");
        transactionJson.Should().Contain("test-area");
        transactionJson.Should().Contain("tag1");
        transactionJson.Should().Contain("tag2");
    }

    [Fact]
    public void BulkTransaction_Should_Handle_Empty_FeatureCollection()
    {
        // Arrange
        var transaction = new BulkTransaction();
        var source = new GeoJsonSource
        {
            Data = new FeatureCollection
            {
                Features = new List<IFeature>()
            }
        };

        var jsonNode = JsonSerializer.SerializeToNode(source);
        var dataNode = jsonNode!["data"];
        

        // Act
        transaction.Add("setSourceData", "empty-source", dataNode);
        var transactionJson = JsonSerializer.Serialize(transaction.Transactions);

        // Assert
        transactionJson.Should().Contain("FeatureCollection");
        transactionJson.Should().Contain("\"features\":[]");
    }

    [Fact]
    public void BulkTransaction_With_URL_String_Should_Serialize()
    {
        // Arrange
        var transaction = new BulkTransaction();
        var source = new GeoJsonSource
        {
            Data = "https://api.example.com/features.geojson"
        };

        var jsonNode = JsonSerializer.SerializeToNode(source);
        var dataNode = jsonNode!["data"];
        

        // Act
        transaction.Add("setSourceData", "url-source", dataNode);
        var transactionJson = JsonSerializer.Serialize(transaction.Transactions);

        // Assert
        transactionJson.Should().Contain("https://api.example.com/features.geojson");
    }

    [Fact]
    public void BulkTransaction_Serialization_Should_Be_Efficient()
    {
        // Arrange
        var transaction = new BulkTransaction();

        for (int i = 0; i < 50; i++)
        {
            var source = new GeoJsonSource
            {
                Data = new FeatureFeature
                {
                    Id = $"feature-{i}",
                    Geometry = new PointGeometry { Coordinates = new[] { (double)i, (double)i } },
                    Properties = new Dictionary<string, object>
                    {
                        { "index", i }
                    }
                }
            };

            var jsonNode = JsonSerializer.SerializeToNode(source);
            var dataNode = jsonNode!["data"];
            

            transaction.Add("setSourceData", $"source-{i}", dataNode);
        }

        // Act
        var startTime = DateTime.UtcNow;
        var transactionJson = JsonSerializer.Serialize(transaction.Transactions);
        var duration = DateTime.UtcNow - startTime;

        // Assert
        duration.Should().BeLessThan(TimeSpan.FromSeconds(1),
            "bulk transaction serialization should be fast");
        transaction.Transactions.Should().HaveCount(50);
    }

    [Fact]
    public void BulkTransaction_Should_Maintain_Order_Of_Operations()
    {
        // Arrange
        var transaction = new BulkTransaction();

        var source1 = new GeoJsonSource { Data = "https://example.com/1.json" };
        var source2 = new GeoJsonSource { Data = "https://example.com/2.json" };
        var source3 = new GeoJsonSource { Data = "https://example.com/3.json" };

        var json1 = JsonSerializer.Serialize(source1);
        using var jsonDoc1 = JsonDocument.Parse(json1);
        var data1 = jsonDoc1.RootElement.GetProperty("data").Clone();

        var json2 = JsonSerializer.Serialize(source2);
        using var jsonDoc2 = JsonDocument.Parse(json2);
        var data2 = jsonDoc2.RootElement.GetProperty("data").Clone();

        var json3 = JsonSerializer.Serialize(source3);
        using var jsonDoc3 = JsonDocument.Parse(json3);
        var data3 = jsonDoc3.RootElement.GetProperty("data").Clone();

        // Act
        transaction.Add("setSourceData", "source-A", data1);
        transaction.Add("setSourceData", "source-B", data2);
        transaction.Add("setSourceData", "source-C", data3);

        // Assert
        transaction.Transactions[0].Data![0].Should().Be("source-A");
        transaction.Transactions[1].Data![0].Should().Be("source-B");
        transaction.Transactions[2].Data![0].Should().Be("source-C");
    }
}
