using System.Text.Json;
using Community.Blazor.MapLibre.Models.Feature;
using Community.Blazor.MapLibre.Models.Feature.Dto;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests;

/// <summary>
/// Tests for the currently-untested types in Models/Feature: FeatureIdentifier,
/// MultiLineGeometry, MultiPointGeometry, and the JsFeatureCollection/JsFeature/JsGeometry
/// inbound-deserialization DTOs.
/// </summary>
public class FeatureSupportSerializationTests
{
    [Fact]
    public void FeatureIdentifier_Should_Serialize_All_Properties()
    {
        // Arrange
        var identifier = new FeatureIdentifier
        {
            Id = "feature1",
            Source = "my-source",
            SourceLayer = "my-layer"
        };

        // Act
        var json = JsonSerializer.Serialize(identifier);

        // Assert
        json.Should().Contain("\"id\":\"feature1\"");
        json.Should().Contain("\"source\":\"my-source\"");
        json.Should().Contain("\"sourceLayer\":\"my-layer\"");
    }

    [Fact]
    public void FeatureIdentifier_Should_Omit_Null_Properties_When_Serializing()
    {
        // Arrange
        var identifier = new FeatureIdentifier();

        // Act
        var json = JsonSerializer.Serialize(identifier);

        // Assert
        json.Should().Be("{}");
    }

    [Fact]
    public void FeatureIdentifier_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """{"id":"feature1","source":"my-source","sourceLayer":"my-layer"}""";

        // Act
        var identifier = JsonSerializer.Deserialize<FeatureIdentifier>(json);

        // Assert
        identifier.Should().NotBeNull();
        identifier!.Id.Should().Be("feature1");
        identifier.Source.Should().Be("my-source");
        identifier.SourceLayer.Should().Be("my-layer");
    }

    [Fact]
    public void MultiLineGeometry_Should_Serialize_With_Type_Discriminator_Only()
    {
        // Arrange - MultiLineGeometry currently has no Coordinates property, so only
        // the polymorphic type discriminator round-trips; coordinate data is lost.
        var geometry = new MultiLineGeometry();

        // Act
        var json = JsonSerializer.Serialize<IGeometry>(geometry);

        // Assert
        json.Should().Be("{\"type\":\"MultiLineString\"}");
    }

    [Fact]
    public void MultiLineGeometry_Should_Deserialize_From_Type_Discriminator()
    {
        // Arrange
        var json = "{\"type\":\"MultiLineString\"}";

        // Act
        var geometry = JsonSerializer.Deserialize<IGeometry>(json);

        // Assert
        geometry.Should().NotBeNull();
        geometry.Should().BeOfType<MultiLineGeometry>();
    }

    [Fact]
    public void MultiLineGeometry_Deserialize_Should_Ignore_Coordinates_Data()
    {
        // Arrange - even if coordinates are present in the JSON, there is no property
        // to bind them to, so they are silently dropped.
        var json = "{\"type\":\"MultiLineString\",\"coordinates\":[[[0.0,0.0],[1.0,1.0]]]}";

        // Act
        var geometry = JsonSerializer.Deserialize<IGeometry>(json);

        // Assert
        geometry.Should().BeOfType<MultiLineGeometry>();
    }

    [Fact]
    public void MultiLineGeometry_GetBounds_Should_Throw_NotImplementedException()
    {
        // Arrange
        var geometry = new MultiLineGeometry();

        // Act
        Action act = () => geometry.GetBounds();

        // Assert
        act.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void MultiPointGeometry_Should_Serialize_With_Type_Discriminator_Only()
    {
        // Arrange - MultiPointGeometry currently has no Coordinates property either.
        var geometry = new MultiPointGeometry();

        // Act
        var json = JsonSerializer.Serialize<IGeometry>(geometry);

        // Assert
        json.Should().Be("{\"type\":\"MultiPoint\"}");
    }

    [Fact]
    public void MultiPointGeometry_Should_Deserialize_From_Type_Discriminator()
    {
        // Arrange
        var json = "{\"type\":\"MultiPoint\"}";

        // Act
        var geometry = JsonSerializer.Deserialize<IGeometry>(json);

        // Assert
        geometry.Should().NotBeNull();
        geometry.Should().BeOfType<MultiPointGeometry>();
    }

    [Fact]
    public void MultiPointGeometry_GetBounds_Should_Throw_NotImplementedException()
    {
        // Arrange
        var geometry = new MultiPointGeometry();

        // Act
        Action act = () => geometry.GetBounds();

        // Assert
        act.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void JsGeometry_ToGeometry_Should_Convert_Point()
    {
        // Arrange
        var json = "{\"type\":\"Point\",\"coordinates\":[-122.4194,37.7749]}";
        var jsGeometry = JsonSerializer.Deserialize<JsGeometry>(json)!;

        // Act
        var geometry = jsGeometry.ToGeometry();

        // Assert
        geometry.Should().BeOfType<PointGeometry>();
        var point = (PointGeometry)geometry;
        point.Coordinates.Should().Equal(-122.4194, 37.7749);
    }

    [Fact]
    public void JsGeometry_ToGeometry_Should_Convert_LineString()
    {
        // Arrange
        var json = "{\"type\":\"LineString\",\"coordinates\":[[-122.4194,37.7749],[-122.4184,37.7739]]}";
        var jsGeometry = JsonSerializer.Deserialize<JsGeometry>(json)!;

        // Act
        var geometry = jsGeometry.ToGeometry();

        // Assert
        geometry.Should().BeOfType<LineGeometry>();
        var line = (LineGeometry)geometry;
        line.Coordinates.Should().HaveCount(2);
    }

    [Fact]
    public void JsGeometry_ToGeometry_Should_Convert_Polygon()
    {
        // Arrange
        var json = "{\"type\":\"Polygon\",\"coordinates\":[[[0.0,0.0],[1.0,0.0],[1.0,1.0],[0.0,0.0]]]}";
        var jsGeometry = JsonSerializer.Deserialize<JsGeometry>(json)!;

        // Act
        var geometry = jsGeometry.ToGeometry();

        // Assert
        geometry.Should().BeOfType<PolygonGeometry>();
        var polygon = (PolygonGeometry)geometry;
        polygon.Coordinates.Should().HaveCount(1);
        polygon.Coordinates[0].Should().HaveCount(4);
    }

    [Fact]
    public void JsGeometry_ToGeometry_Should_Throw_NotSupportedException_For_Unsupported_Type()
    {
        // Arrange - MultiPoint (and any other type) is not handled by JsGeometry.ToGeometry()
        var json = "{\"type\":\"MultiPoint\",\"coordinates\":[[0.0,0.0],[1.0,1.0]]}";
        var jsGeometry = JsonSerializer.Deserialize<JsGeometry>(json)!;

        // Act
        Action act = () => jsGeometry.ToGeometry();

        // Assert
        act.Should().Throw<NotSupportedException>()
            .WithMessage("Geometry type 'MultiPoint' is not supported");
    }

    [Fact]
    public void JsFeature_ToFeatureFeature_Should_Convert_Id_Geometry_And_Properties()
    {
        // Arrange
        var json = """
                   {
                        "id":"feature1",
                        "type":"Feature",
                        "geometry":{"type":"Point","coordinates":[-122.4194,37.7749]},
                        "properties":{"name":"San Francisco","population":873965,"active":true}
                   }
                   """;
        var jsFeature = JsonSerializer.Deserialize<JsFeature>(json)!;

        // Act
        var feature = jsFeature.ToFeatureFeature();

        // Assert
        feature.Id.Should().Be("feature1");
        feature.Geometry.Should().BeOfType<PointGeometry>();
        feature.Properties.Should().NotBeNull();
        feature.Properties!["name"].Should().Be("San Francisco");
        feature.Properties["population"].Should().Be(873965);
        feature.Properties["active"].Should().Be(true);
    }

    [Fact]
    public void JsFeature_ToFeatureFeature_Should_Default_Geometry_When_Missing()
    {
        // Arrange - no geometry supplied
        var jsFeature = new JsFeature { Id = "no-geometry" };

        // Act
        var feature = jsFeature.ToFeatureFeature();

        // Assert
        feature.Geometry.Should().BeOfType<PointGeometry>();
        ((PointGeometry)feature.Geometry).Coordinates.Should().Equal(0, 0);
    }

    [Fact]
    public void JsFeature_ToFeatureFeature_Should_Have_Null_Properties_When_None_Supplied()
    {
        // Arrange
        var jsFeature = new JsFeature
        {
            Id = "no-props",
            Geometry = new JsGeometry
            {
                Type = "Point",
                Coordinates = JsonSerializer.SerializeToElement(new[] { 0.0, 0.0 })
            }
        };

        // Act
        var feature = jsFeature.ToFeatureFeature();

        // Assert
        feature.Properties.Should().BeNull();
    }

    [Fact]
    public void JsFeatureCollection_ToFeatureCollection_Should_Convert_All_Features()
    {
        // Arrange
        var json = """
                   {
                        "type":"FeatureCollection",
                        "features":[
                            {
                                "id":"point1",
                                "type":"Feature",
                                "geometry":{"type":"Point","coordinates":[-122.4194,37.7749]},
                                "properties":{"name":"San Francisco"}
                            },
                            {
                                "id":"line1",
                                "type":"Feature",
                                "geometry":{"type":"LineString","coordinates":[[0.0,0.0],[1.0,1.0]]}
                            }
                        ]
                   }
                   """;
        var jsCollection = JsonSerializer.Deserialize<JsFeatureCollection>(json)!;

        // Act
        var collection = jsCollection.ToFeatureCollection();

        // Assert
        collection.Features.Should().HaveCount(2);
        collection.Features[0].Should().BeOfType<FeatureFeature>();
        ((FeatureFeature)collection.Features[0]).Id.Should().Be("point1");
        collection.Features[1].Should().BeOfType<FeatureFeature>();
        ((FeatureFeature)collection.Features[1]).Geometry.Should().BeOfType<LineGeometry>();
    }

    [Fact]
    public void JsFeatureCollection_ToFeatureCollection_Should_Return_Empty_Collection_When_No_Features()
    {
        // Arrange
        var jsCollection = new JsFeatureCollection();

        // Act
        var collection = jsCollection.ToFeatureCollection();

        // Assert
        collection.Features.Should().BeEmpty();
    }

    [Fact]
    public void JsFeatureCollection_Should_Default_Type_To_FeatureCollection()
    {
        // Arrange
        var jsCollection = new JsFeatureCollection();

        // Assert - defaults declared on the DTO itself
        jsCollection.Type.Should().Be("FeatureCollection");
        jsCollection.Features.Should().BeEmpty();
    }
}
