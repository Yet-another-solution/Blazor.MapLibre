using System.Text.Json;
using Community.Blazor.MapLibre.Models;
using Community.Blazor.MapLibre.Models.Camera;
using Community.Blazor.MapLibre.Models.Padding;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests;

/// <summary>
/// Tests covering serialization/deserialization of every type in Models/Camera.
/// </summary>
/// <remarks>
/// <see cref="IAnimationOptions"/>, <see cref="ICameraOptions"/> and <see cref="ICenterZoomBearing"/> are plain
/// interfaces with no <c>[JsonPolymorphic]</c>/<c>[JsonDerivedType]</c> wiring (unlike <c>IGeometry</c>/<c>IFeature</c>),
/// so they are never serialized/deserialized directly through the interface type anywhere in this codebase.
/// Their members are exercised here through the concrete classes that implement them
/// (<see cref="AnimationOptions"/>, <see cref="CameraOptions"/>, <see cref="EaseToOptions"/>,
/// <see cref="FlyToOptions"/>, <see cref="CenterZoomBearing"/>).
/// </remarks>
public class CameraSerializationTests
{
    [Fact]
    public void AnimationOptions_Should_Serialize_Successfully()
    {
        // Arrange
        var options = new AnimationOptions
        {
            Animate = true,
            Duration = 1200,
            Essential = true,
            FreezeElevation = false,
            Offset = new PointLike(10, 20)
        };

        // Act
        var json = JsonSerializer.Serialize(options);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"animate\":true");
        json.Should().Contain("\"duration\":1200");
        json.Should().Contain("\"essential\":true");
        json.Should().Contain("\"freezeElevation\":false");
        json.Should().Contain("\"offset\":{\"x\":10,\"y\":20}");
        json.Should().NotContain("\"easing\"", "Easing was left null and should be omitted, not written as null");
    }

    [Fact]
    public void AnimationOptions_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """
                   {
                       "animate":true,
                       "duration":1200,
                       "essential":true,
                       "freezeElevation":false,
                       "offset":{"x":10,"y":20}
                   }
                   """;

        // Act
        var options = JsonSerializer.Deserialize<AnimationOptions>(json);

        // Assert
        options.Should().NotBeNull();
        options!.Animate.Should().BeTrue();
        options.Duration.Should().Be(1200);
        options.Essential.Should().BeTrue();
        options.FreezeElevation.Should().BeFalse();
        options.Offset.Should().NotBeNull();
        options.Offset!.X.Should().Be(10);
        options.Offset.Y.Should().Be(20);
    }

    [Fact]
    public void CameraOptions_Should_Serialize_Successfully()
    {
        // Arrange - San Francisco
        var options = new CameraOptions
        {
            Center = new LngLat(-122.4194, 37.7749),
            Zoom = 12.5,
            Bearing = 45,
            Pitch = 30,
            Roll = 0,
            Elevation = 100
        };

        // Act
        var json = JsonSerializer.Serialize(options);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"center\":{\"lng\":-122.4194,\"lat\":37.7749}");
        json.Should().Contain("\"zoom\":12.5");
        json.Should().Contain("\"bearing\":45");
        json.Should().Contain("\"pitch\":30");
        json.Should().Contain("\"roll\":0");
        json.Should().Contain("\"elevation\":100");
    }

    [Fact]
    public void CameraOptions_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """
                   {
                       "center":{"lng":-122.4194,"lat":37.7749},
                       "zoom":12.5,
                       "bearing":45,
                       "pitch":30,
                       "roll":0,
                       "elevation":100
                   }
                   """;

        // Act
        var options = JsonSerializer.Deserialize<CameraOptions>(json);

        // Assert
        options.Should().NotBeNull();
        options!.Center.Should().NotBeNull();
        options.Center!.Longitude.Should().Be(-122.4194);
        options.Center.Latitude.Should().Be(37.7749);
        options.Zoom.Should().Be(12.5);
        options.Bearing.Should().Be(45);
        options.Pitch.Should().Be(30);
        options.Roll.Should().Be(0);
        options.Elevation.Should().Be(100);
    }

    [Fact]
    public void CameraOptions_With_Null_Optional_Properties_Should_Omit_Them_From_Json()
    {
        // Arrange - only Center and Zoom are set, everything else is left null.
        // Every optional property on CameraOptions is marked
        // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)], mirroring the pattern used
        // throughout the codebase (see MarkerOptions), so unset properties must be omitted entirely
        // rather than serialized as `null`.
        var options = new CameraOptions
        {
            Center = new LngLat(-122.4194, 37.7749),
            Zoom = 12.5
        };

        // Act
        var json = JsonSerializer.Serialize(options);

        // Assert
        json.Should().Contain("\"center\"");
        json.Should().Contain("\"zoom\"");
        json.Should().NotContain("\"bearing\"");
        json.Should().NotContain("\"pitch\"");
        json.Should().NotContain("\"roll\"");
        json.Should().NotContain("\"elevation\"");
        json.Should().NotContain("null");
    }

    [Fact]
    public void CenterZoomBearing_Should_Serialize_Successfully()
    {
        // Arrange - Paris. Note: unlike CameraOptions, CenterZoomBearing has no [JsonPropertyName]
        // attributes of its own, so its properties serialize using their PascalCase C# names.
        var centerZoomBearing = new CenterZoomBearing
        {
            Center = new LngLat(2.3522, 48.8566),
            Zoom = 10,
            Bearing = 90
        };

        // Act
        var json = JsonSerializer.Serialize(centerZoomBearing);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"Center\":{\"lng\":2.3522,\"lat\":48.8566}");
        json.Should().Contain("\"Zoom\":10");
        json.Should().Contain("\"Bearing\":90");
    }

    [Fact]
    public void CenterZoomBearing_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """
                   {
                       "Center":{"lng":2.3522,"lat":48.8566},
                       "Zoom":10,
                       "Bearing":90
                   }
                   """;

        // Act
        var centerZoomBearing = JsonSerializer.Deserialize<CenterZoomBearing>(json);

        // Assert
        centerZoomBearing.Should().NotBeNull();
        centerZoomBearing!.Center.Should().NotBeNull();
        centerZoomBearing.Center!.Longitude.Should().Be(2.3522);
        centerZoomBearing.Center.Latitude.Should().Be(48.8566);
        centerZoomBearing.Zoom.Should().Be(10);
        centerZoomBearing.Bearing.Should().Be(90);
    }

    [Fact]
    public void CameraForBoundsOptions_Should_Serialize_Successfully()
    {
        // Arrange - New York City. Padding is now OneOf<decimal, PaddingOptions>? with
        // OneOfJsonConverter<decimal, PaddingOptions> applied, fixing what used to be an
        // unconditional InvalidOperationException on every use of this type.
        var options = new CameraForBoundsOptions
        {
            Center = new LngLat(-74.006, 40.7128),
            MaxZoom = 15,
            Offset = new PointLike(0, 0),
            Padding = 10m
        };

        // Act
        var json = JsonSerializer.Serialize(options);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"center\":{\"lng\":-74.006,\"lat\":40.7128}");
        json.Should().Contain("\"maxZoom\":15");
        json.Should().Contain("\"offset\":{\"x\":0,\"y\":0}");
        json.Should().Contain("\"padding\":10");
    }

    [Fact]
    public void CameraForBoundsOptions_With_PaddingOptions_Should_Serialize_Successfully()
    {
        var options = new CameraForBoundsOptions
        {
            Center = new LngLat(-74.006, 40.7128),
            Padding = new PaddingOptions { Top = 10, Bottom = 10, Left = 20, Right = 20 }
        };

        var json = JsonSerializer.Serialize(options);

        json.Should().Contain("\"padding\":{");
        json.Should().Contain("\"top\":10");
        json.Should().Contain("\"left\":20");
    }

    [Fact]
    public void CameraForBoundsOptions_Should_Deserialize_Successfully()
    {
        var json = """
                   {
                       "center":{"lng":-74.006,"lat":40.7128},
                       "maxZoom":15,
                       "offset":{"x":0,"y":0},
                       "padding":10
                   }
                   """;

        var options = JsonSerializer.Deserialize<CameraForBoundsOptions>(json);

        options.Should().NotBeNull();
        options!.Center.Should().NotBeNull();
        options.Center!.Longitude.Should().Be(-74.006);
        options.Center.Latitude.Should().Be(40.7128);
        options.MaxZoom.Should().Be(15);
        options.Offset.Should().NotBeNull();
        options.Offset!.X.Should().Be(0);
        options.Offset.Y.Should().Be(0);
        options.Padding.Should().NotBeNull();
        options.Padding!.Value.AsT0.Should().Be(10m);
    }

    [Fact]
    public void CameraForBoundsOptions_With_Null_Padding_Should_Omit_It_From_Json()
    {
        var options = new CameraForBoundsOptions { Center = new LngLat(-74.006, 40.7128) };

        var json = JsonSerializer.Serialize(options);

        json.Should().NotContain("\"padding\"");
    }

    [Fact]
    public void EaseToOptions_Should_Serialize_Successfully()
    {
        // Arrange - Tokyo. Padding is intentionally left unset (see the dedicated throw test below).
        var options = new EaseToOptions
        {
            Center = new LngLat(139.6917, 35.6895),
            Zoom = 14,
            Bearing = 180,
            Pitch = 45,
            Roll = 0,
            Elevation = 10,
            Around = new LngLat(139.6917, 35.6895),
            DelayEndEvents = 200,
            EaseId = "ease-1",
            NoMoveStart = false,
            Animate = true,
            Duration = 500,
            Essential = true,
            FreezeElevation = false,
            Offset = new PointLike(5, 5)
        };

        // Act
        var json = JsonSerializer.Serialize(options);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"center\":{\"lng\":139.6917,\"lat\":35.6895}");
        json.Should().Contain("\"zoom\":14");
        json.Should().Contain("\"bearing\":180");
        json.Should().Contain("\"pitch\":45");
        json.Should().Contain("\"roll\":0");
        json.Should().Contain("\"elevation\":10");
        json.Should().Contain("\"around\":{\"lng\":139.6917,\"lat\":35.6895}");
        json.Should().Contain("\"delayEndEvents\":200");
        json.Should().Contain("\"easeId\":\"ease-1\"");
        json.Should().Contain("\"noMoveStart\":false");
        json.Should().Contain("\"animate\":true");
        json.Should().Contain("\"duration\":500");
        json.Should().Contain("\"essential\":true");
        json.Should().Contain("\"freezeElevation\":false");
        json.Should().Contain("\"offset\":{\"x\":5,\"y\":5}");
        json.Should().NotContain("\"padding\"", "Padding was left null and should be omitted, not written as null");
    }

    [Fact]
    public void EaseToOptions_Should_Deserialize_Successfully()
    {
        // Arrange - lowercase "center"/"zoom"/"bearing" is the real MapLibre-shaped JSON.
        var json = """
                   {
                       "center":{"lng":139.6917,"lat":35.6895},
                       "zoom":14,
                       "bearing":180,
                       "pitch":45,
                       "roll":0,
                       "elevation":10,
                       "around":{"lng":139.6917,"lat":35.6895},
                       "delayEndEvents":200,
                       "easeId":"ease-1",
                       "noMoveStart":false,
                       "animate":true,
                       "duration":500,
                       "essential":true,
                       "freezeElevation":false,
                       "offset":{"x":5,"y":5}
                   }
                   """;

        // Act
        var options = JsonSerializer.Deserialize<EaseToOptions>(json);

        // Assert
        options.Should().NotBeNull();
        options!.Center!.Longitude.Should().Be(139.6917);
        options.Center.Latitude.Should().Be(35.6895);
        options.Zoom.Should().Be(14);
        options.Bearing.Should().Be(180);
        options.Pitch.Should().Be(45);
        options.Roll.Should().Be(0);
        options.Elevation.Should().Be(10);
        options.Around.Should().NotBeNull();
        options.Around!.Longitude.Should().Be(139.6917);
        options.DelayEndEvents.Should().Be(200);
        options.EaseId.Should().Be("ease-1");
        options.NoMoveStart.Should().BeFalse();
        options.Animate.Should().BeTrue();
        options.Duration.Should().Be(500);
        options.Essential.Should().BeTrue();
        options.FreezeElevation.Should().BeFalse();
        options.Offset.Should().NotBeNull();
        options.Offset!.X.Should().Be(5);
        options.Offset.Y.Should().Be(5);
    }

    [Fact]
    public void EaseToOptions_With_Padding_Set_Should_Serialize_Successfully()
    {
        // EaseToOptions.Padding is OneOf<double, PaddingOptions>? with
        // OneOfJsonConverter<double, PaddingOptions> applied, fixing what used to throw
        // InvalidOperationException whenever Padding was assigned a value.
        var options = new EaseToOptions
        {
            Padding = 20.0
        };

        var json = JsonSerializer.Serialize(options);

        json.Should().Contain("\"padding\":20");
    }

    [Fact]
    public void EaseToOptions_With_PaddingOptions_Should_Serialize_Successfully()
    {
        var options = new EaseToOptions
        {
            Padding = new PaddingOptions { Top = 5, Bottom = 5 }
        };

        var json = JsonSerializer.Serialize(options);

        json.Should().Contain("\"padding\":{");
        json.Should().Contain("\"top\":5");
    }

    [Fact]
    public void FlyToOptions_Should_Serialize_Successfully()
    {
        // Arrange - Sydney
        var options = new FlyToOptions
        {
            Center = new LngLat(151.2093, -33.8688),
            Zoom = 10,
            Bearing = 0,
            Pitch = 0,
            Curve = 1.42,
            Speed = 1.2,
            Duration = 3000,
            Essential = true,
            Padding = 20,
            MinZoom = 5,
            MaxDuration = 4000,
            ScreenSpeed = 1.5,
            Offset = new PointLike(0, 0)
        };

        // Act
        var json = JsonSerializer.Serialize(options);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"center\":{\"lng\":151.2093,\"lat\":-33.8688}");
        json.Should().Contain("\"zoom\":10");
        json.Should().Contain("\"bearing\":0");
        json.Should().Contain("\"pitch\":0");
        json.Should().Contain("\"curve\":1.42");
        json.Should().Contain("\"speed\":1.2");
        json.Should().Contain("\"duration\":3000");
        json.Should().Contain("\"essential\":true");
        json.Should().Contain("\"padding\":20");
        json.Should().Contain("\"minZoom\":5");
        json.Should().Contain("\"maxDuration\":4000");
        json.Should().Contain("\"screenSpeed\":1.5");
        json.Should().Contain("\"offset\":{\"x\":0,\"y\":0}");
    }

    [Fact]
    public void FlyToOptions_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """
                   {
                       "center":{"lng":151.2093,"lat":-33.8688},
                       "zoom":10,
                       "bearing":0,
                       "pitch":0,
                       "curve":1.42,
                       "speed":1.2,
                       "duration":3000,
                       "essential":true,
                       "padding":20,
                       "minZoom":5,
                       "maxDuration":4000,
                       "screenSpeed":1.5,
                       "offset":{"x":0,"y":0}
                   }
                   """;

        // Act
        var options = JsonSerializer.Deserialize<FlyToOptions>(json);

        // Assert
        options.Should().NotBeNull();
        options!.Center!.Longitude.Should().Be(151.2093);
        options.Center.Latitude.Should().Be(-33.8688);
        options.Zoom.Should().Be(10);
        options.Bearing.Should().Be(0);
        options.Pitch.Should().Be(0);
        options.Curve.Should().Be(1.42);
        options.Speed.Should().Be(1.2);
        options.Duration.Should().Be(3000);
        options.Essential.Should().BeTrue();
        options.Padding.Should().Be(20);
        options.MinZoom.Should().Be(5);
        options.MaxDuration.Should().Be(4000);
        options.ScreenSpeed.Should().Be(1.5);
        options.Offset.Should().NotBeNull();
        options.Offset!.X.Should().Be(0);
        options.Offset.Y.Should().Be(0);
    }

    [Fact]
    public void FitBoundOptions_Should_Serialize_Successfully()
    {
        // Arrange - bounding box fit that flies over central London
        var options = new FitBoundOptions
        {
            Center = new LngLat(-0.1278, 51.5074),
            Zoom = 11,
            Curve = 1.42,
            Speed = 1.2,
            Linear = true,
            MaxZoom = 16,
            Offset = new PointLike(10, 20)
        };

        // Act
        var json = JsonSerializer.Serialize(options);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"center\":{\"lng\":-0.1278,\"lat\":51.5074}");
        json.Should().Contain("\"zoom\":11");
        json.Should().Contain("\"curve\":1.42");
        json.Should().Contain("\"speed\":1.2");
        json.Should().Contain("\"linear\":true");
        json.Should().Contain("\"maxZoom\":16");
        json.Should().Contain("\"offset\":{\"x\":10,\"y\":20}");
    }

    [Fact]
    public void FitBoundOptions_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """
                   {
                       "center":{"lng":-0.1278,"lat":51.5074},
                       "zoom":11,
                       "curve":1.42,
                       "speed":1.2,
                       "linear":true,
                       "maxZoom":16,
                       "offset":{"x":10,"y":20}
                   }
                   """;

        // Act
        var options = JsonSerializer.Deserialize<FitBoundOptions>(json);

        // Assert
        options.Should().NotBeNull();
        options!.Center!.Longitude.Should().Be(-0.1278);
        options.Center.Latitude.Should().Be(51.5074);
        options.Zoom.Should().Be(11);
        options.Curve.Should().Be(1.42);
        options.Speed.Should().Be(1.2);
        options.Linear.Should().BeTrue();
        options.MaxZoom.Should().Be(16);
        options.Offset.Should().NotBeNull();
        options.Offset!.X.Should().Be(10);
        options.Offset.Y.Should().Be(20);
    }

    [Fact]
    public void JumpToOptions_Should_Serialize_Successfully()
    {
        // Arrange - London
        var options = new JumpToOptions
        {
            Center = new LngLat(-0.1278, 51.5074),
            Zoom = 9,
            Bearing = 0,
            Padding = new PaddingOptions
            {
                Top = 10,
                Bottom = 10,
                Left = 5,
                Right = 5
            }
        };

        // Act
        var json = JsonSerializer.Serialize(options);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"center\":{\"lng\":-0.1278,\"lat\":51.5074}");
        json.Should().Contain("\"zoom\":9");
        json.Should().Contain("\"bearing\":0");
        json.Should().Contain("\"padding\":{\"top\":10,\"bottom\":10,\"right\":5,\"left\":5}");
    }

    [Fact]
    public void JumpToOptions_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """
                   {
                       "center":{"lng":-0.1278,"lat":51.5074},
                       "zoom":9,
                       "bearing":0,
                       "padding":{"top":10,"bottom":10,"right":5,"left":5}
                   }
                   """;

        // Act
        var options = JsonSerializer.Deserialize<JumpToOptions>(json);

        // Assert
        options.Should().NotBeNull();
        options!.Center!.Longitude.Should().Be(-0.1278);
        options.Center.Latitude.Should().Be(51.5074);
        options.Zoom.Should().Be(9);
        options.Bearing.Should().Be(0);
        options.Padding.Should().NotBeNull();
        options.Padding!.Top.Should().Be(10);
        options.Padding.Bottom.Should().Be(10);
        options.Padding.Left.Should().Be(5);
        options.Padding.Right.Should().Be(5);
    }

    [Fact]
    public void JumpToOptions_With_Null_Padding_Should_Omit_It_From_Json()
    {
        // Arrange
        var options = new JumpToOptions
        {
            Center = new LngLat(-0.1278, 51.5074),
            Zoom = 9
        };

        // Act
        var json = JsonSerializer.Serialize(options);

        // Assert
        json.Should().Contain("\"center\"");
        json.Should().NotContain("\"padding\"", "Padding was left null and should be omitted, not written as null");
    }
}
