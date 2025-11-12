using System.Text.Json;
using Community.Blazor.MapLibre.Models.Feature;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests;

/// <summary>
/// Tests to verify that IGeometry serialization works correctly with .NET 9 and .NET 10.
/// This ensures the fix for Polymorphism_PropertyConflictsWithMetadataPropertyName works.
/// </summary>
public class GeometrySerializationTests
{
    [Fact]
    public void PointGeometry_Should_Serialize_Successfully()
    {
        // Arrange
        var point = new PointGeometry
        {
            Coordinates = new[] { -122.4194, 37.7749 } // San Francisco
        };

        // Act
        var json = JsonSerializer.Serialize<IGeometry>(point);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"type\":\"Point\"");
        json.Should().Contain("\"coordinates\":[-122.4194,37.7749]");
    }

    [Fact]
    public void PointGeometry_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = "{\"$type\":\"Point\",\"type\":\"Point\",\"coordinates\":[-122.4194,37.7749]}";

        // Act
        var geometry = JsonSerializer.Deserialize<IGeometry>(json);

        // Assert
        geometry.Should().NotBeNull();
        geometry.Should().BeOfType<PointGeometry>();
        var point = (PointGeometry)geometry!;
        point.Type.Should().Be(GeometryType.Point);
        point.Coordinates.Should().HaveCount(2);
        point.Coordinates[0].Should().Be(-122.4194);
        point.Coordinates[1].Should().Be(37.7749);
    }

    [Fact]
    public void LineGeometry_Should_Serialize_Successfully()
    {
        // Arrange
        var line = new LineGeometry
        {
            Coordinates = new[]
            {
                new[] { -122.4194, 37.7749 },
                new[] { -122.4184, 37.7739 }
            }
        };

        // Act
        var json = JsonSerializer.Serialize<IGeometry>(line);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"type\":\"LineString\"");
        json.Should().Contain("\"coordinates\":");
    }

    [Fact]
    public void LineGeometry_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = "{\"$type\":\"LineString\",\"type\":\"LineString\",\"coordinates\":[[-122.4194,37.7749],[-122.4184,37.7739]]}";

        // Act
        var geometry = JsonSerializer.Deserialize<IGeometry>(json);

        // Assert
        geometry.Should().NotBeNull();
        geometry.Should().BeOfType<LineGeometry>();
        var line = (LineGeometry)geometry!;
        line.Type.Should().Be(GeometryType.Line);
        line.Coordinates.Should().HaveCount(2);
    }

    [Fact]
    public void PolygonGeometry_Should_Serialize_Successfully()
    {
        // Arrange
        var polygon = new PolygonGeometry
        {
            Coordinates = new[]
            {
                new[]
                {
                    new[] { -122.4194, 37.7749 },
                    new[] { -122.4184, 37.7739 },
                    new[] { -122.4204, 37.7729 },
                    new[] { -122.4194, 37.7749 } // Close the polygon
                }
            }
        };

        // Act
        var json = JsonSerializer.Serialize<IGeometry>(polygon);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"type\":\"Polygon\"");
        json.Should().Contain("\"coordinates\":");
    }

    [Fact]
    public void PolygonGeometry_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = "{\"$type\":\"Polygon\",\"type\":\"Polygon\",\"coordinates\":[[[-122.4194,37.7749],[-122.4184,37.7739],[-122.4204,37.7729],[-122.4194,37.7749]]]}";

        // Act
        var geometry = JsonSerializer.Deserialize<IGeometry>(json);

        // Assert
        geometry.Should().NotBeNull();
        geometry.Should().BeOfType<PolygonGeometry>();
        var polygon = (PolygonGeometry)geometry!;
        polygon.Type.Should().Be(GeometryType.Polygon);
        polygon.Coordinates.Should().HaveCount(1);
        polygon.Coordinates[0].Should().HaveCount(4);
    }

    [Fact]
    public void MultiPolygonGeometry_Should_Serialize_Successfully()
    {
        // Arrange
        var multiPolygon = new MultiPolygonGeometry
        {
            Coordinates = new[] {
                new[] {
                    new[]
                    {
                        new[] { -122.4194, 37.7749 },
                        new[] { -122.4184, 37.7739 },
                        new[] { -122.4204, 37.7729 },
                        new[] { -122.4194, 37.7749 }
                    }
                }
            }
        };

        // Act
        var json = JsonSerializer.Serialize<IGeometry>(multiPolygon);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"type\":\"MultiPolygon\"");
        json.Should().Contain("\"coordinates\":");
    }

    [Fact]
    public void Serialization_Should_Not_Throw_PropertyConflictsWithMetadataPropertyName_Exception()
    {
        // Arrange
        var point = new PointGeometry
        {
            Coordinates = new[] { -122.4194, 37.7749 }
        };

        // Act
        Action act = () => JsonSerializer.Serialize<IGeometry>(point);

        // Assert
        act.Should().NotThrow<InvalidOperationException>("the fix should resolve the .NET 10 polymorphism conflict");
    }

    [Fact]
    public void GetBounds_Should_Work_On_PointGeometry()
    {
        // Arrange
        var point = new PointGeometry
        {
            Coordinates = new[] { -122.4194, 37.7749 }
        };

        // Act
        var bounds = point.GetBounds();

        // Assert
        bounds.Should().NotBeNull();
        bounds.Southwest.Longitude.Should().Be(-122.4194);
        bounds.Southwest.Latitude.Should().Be(37.7749);
        bounds.Northeast.Longitude.Should().Be(-122.4194);
        bounds.Northeast.Latitude.Should().Be(37.7749);
    }

    [Fact]
    public void GetBounds_Should_Work_On_LineGeometry()
    {
        // Arrange
        var line = new LineGeometry
        {
            Coordinates = new[]
            {
                new[] { -122.4194, 37.7749 },
                new[] { -122.4184, 37.7739 },
                new[] { -122.4204, 37.7759 }
            }
        };

        // Act
        var bounds = line.GetBounds();

        // Assert
        bounds.Should().NotBeNull();
        bounds.Southwest.Longitude.Should().Be(-122.4204);
        bounds.Southwest.Latitude.Should().Be(37.7739);
        bounds.Northeast.Longitude.Should().Be(-122.4184);
        bounds.Northeast.Latitude.Should().Be(37.7759);
    }
}
