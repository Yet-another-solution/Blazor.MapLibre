using System.Text.Json;
using System.Text.Json.Nodes;
using Community.Blazor.MapLibre.Models.Feature;
using Community.Blazor.MapLibre.Models.Sources;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests;

/// <summary>
/// Tests to compare different serialization approaches and verify the simplest working solution.
/// </summary>
public class SerializationApproachTests
{
    [Fact]
    public void Approach1_StringParse_Works()
    {
        // Arrange - Current implementation approach
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

        // Act - Serialize -> Parse -> Extract
        var json = JsonSerializer.Serialize(source);
        using var jsonDoc = JsonDocument.Parse(json);
        var dataElement = jsonDoc.RootElement.GetProperty("data");
        var resultJson = JsonSerializer.Serialize(dataElement);

        // Assert
        resultJson.Should().Contain("FeatureCollection");
        resultJson.Should().NotContain("geojson");
    }

    [Fact]
    public void Approach2_JsonNode_Works()
    {
        // Arrange - Alternative using JsonNode
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

        // Act - Serialize to JsonNode and extract
        var jsonNode = JsonSerializer.SerializeToNode(source);
        var dataNode = jsonNode!["data"];
        var resultJson = JsonSerializer.Serialize(dataNode);

        // Assert
        resultJson.Should().Contain("FeatureCollection");
        resultJson.Should().NotContain("geojson");
    }

    [Fact]
    public void Approach3_DirectSerialization_Fails()
    {
        // Arrange - What happens if we try to serialize source.Data directly?
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

        // Act - Try to serialize the Data property directly
        // This SHOULD work because Data is IFeature which has proper polymorphic attributes
        var json = JsonSerializer.Serialize(source.Data.AsT0);

        // Assert
        json.Should().Contain("FeatureCollection");
    }

    [Fact]
    public void Approach_Comparison_URL_String()
    {
        // Arrange
        var source = new GeoJsonSource
        {
            Data = "https://example.com/data.geojson"
        };

        // Act - Test both approaches with URL string
        // Approach 1: String -> Parse -> Extract
        var json1 = JsonSerializer.Serialize(source);
        using var jsonDoc1 = JsonDocument.Parse(json1);
        var data1 = jsonDoc1.RootElement.GetProperty("data");

        // Approach 2: JsonNode
        var jsonNode2 = JsonSerializer.SerializeToNode(source);
        var data2 = jsonNode2!["data"];

        // Assert - Both should produce same result
        data1.GetString().Should().Be("https://example.com/data.geojson");
        data2!.GetValue<string>().Should().Be("https://example.com/data.geojson");
    }

    [Fact]
    public void JsonNode_Can_Be_Passed_To_JSInterop()
    {
        // Arrange - Verify JsonNode serializes correctly for JS interop
        var source = new GeoJsonSource
        {
            Data = new FeatureCollection
            {
                Features = new List<IFeature>
                {
                    new FeatureFeature
                    {
                        Id = "interop-test",
                        Geometry = new PointGeometry { Coordinates = new[] { 10.0, 20.0 } }
                    }
                }
            }
        };

        var jsonNode = JsonSerializer.SerializeToNode(source);
        var dataNode = jsonNode!["data"];

        // Act - Simulate what JS interop would do
        var serializedForInterop = JsonSerializer.Serialize(dataNode);

        // Assert
        serializedForInterop.Should().Contain("FeatureCollection");
        serializedForInterop.Should().Contain("interop-test");
    }

    [Fact]
    public void JsonElement_Vs_JsonNode_Comparison()
    {
        // Arrange
        var source = new GeoJsonSource
        {
            Data = new FeatureFeature
            {
                Id = "compare",
                Geometry = new PointGeometry { Coordinates = new[] { 1.0, 2.0 } },
                Properties = new Dictionary<string, object>
                {
                    { "name", "Test" }
                }
            }
        };

        // Act - Compare JsonElement vs JsonNode output
        var json = JsonSerializer.Serialize(source);
        using var jsonDoc = JsonDocument.Parse(json);
        var jsonElement = jsonDoc.RootElement.GetProperty("data");
        var elementResult = JsonSerializer.Serialize(jsonElement);

        var jsonNode = JsonSerializer.SerializeToNode(source);
        var dataNode = jsonNode!["data"];
        var nodeResult = JsonSerializer.Serialize(dataNode);

        // Assert - Both should produce identical JSON
        elementResult.Should().Be(nodeResult);
    }
}
