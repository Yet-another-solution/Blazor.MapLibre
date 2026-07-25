using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Models;
using FluentAssertions;
using OneOf;
using Xunit;

namespace Community.Blazor.MapLibre.Tests;

/// <summary>
/// Tests covering the previously-untested types that live directly in Models/ (no subfolder):
/// LngLatBounds, PointLike, Popup, PopupOptions, PositionAnchor, ProjectionSpecification,
/// TextFit, Visibility, WebGLContextAttributes.
/// (LngLat and BulkTransaction are covered elsewhere, e.g. BulkTransactionTests.cs.)
/// </summary>
public class TopLevelModelSerializationTests
{
    // PositionAnchor/TextFit/Visibility carry [JsonStringEnumMemberName], which only takes
    // effect when a JsonStringEnumConverter is applied (mirrors ControlSerializationTests /
    // SupportModelSerializationTests). Plain JsonSerializer calls on properties of these
    // enum types serialize the underlying numeric value instead.
    private static readonly JsonSerializerOptions StringEnumOptions = new()
    {
        Converters = { new JsonStringEnumConverter() }
    };

    // ----- LngLatBounds -----

    [Fact]
    public void LngLatBounds_Should_Serialize_Successfully()
    {
        // Arrange - roughly the bounding box of San Francisco
        var bounds = new LngLatBounds
        {
            Southwest = new LngLat(-122.5155, 37.7080),
            Northeast = new LngLat(-122.3568, 37.8324)
        };

        // Act
        var json = JsonSerializer.Serialize(bounds);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"_sw\":");
        json.Should().Contain("\"_ne\":");
        json.Should().Contain("\"lng\":-122.5155");
        json.Should().Contain("\"lat\":37.708");
    }

    [Fact]
    public void LngLatBounds_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """
                   {
                        "_sw":{"lng":-122.5155,"lat":37.708},
                        "_ne":{"lng":-122.3568,"lat":37.8324}
                   }
                   """;

        // Act
        var bounds = JsonSerializer.Deserialize<LngLatBounds>(json);

        // Assert
        bounds.Should().NotBeNull();
        bounds!.Southwest.Longitude.Should().Be(-122.5155);
        bounds.Southwest.Latitude.Should().Be(37.708);
        bounds.Northeast.Longitude.Should().Be(-122.3568);
        bounds.Northeast.Latitude.Should().Be(37.8324);
    }

    [Fact]
    public void LngLatBounds_Extend_With_Point_Should_Grow_Bounds_To_Include_It()
    {
        // Arrange
        var bounds = new LngLatBounds
        {
            Southwest = new LngLat(0, 0),
            Northeast = new LngLat(10, 10)
        };

        // Act - a point outside the current bounds on both axes
        bounds.Extend(new LngLat(-5, 15));

        // Assert
        bounds.Southwest.Longitude.Should().Be(-5);
        bounds.Southwest.Latitude.Should().Be(0);
        bounds.Northeast.Longitude.Should().Be(10);
        bounds.Northeast.Latitude.Should().Be(15);
    }

    [Fact]
    public void LngLatBounds_Extend_With_Bounds_Should_Grow_To_Include_Other_Bounds()
    {
        // Arrange
        var bounds = new LngLatBounds
        {
            Southwest = new LngLat(0, 0),
            Northeast = new LngLat(10, 10)
        };
        var other = new LngLatBounds
        {
            Southwest = new LngLat(-5, -5),
            Northeast = new LngLat(20, 20)
        };

        // Act
        bounds.Extend(other);

        // Assert
        bounds.Southwest.Longitude.Should().Be(-5);
        bounds.Southwest.Latitude.Should().Be(-5);
        bounds.Northeast.Longitude.Should().Be(20);
        bounds.Northeast.Latitude.Should().Be(20);
    }

    [Fact]
    public void LngLatBounds_GetCenter_Should_Return_Midpoint()
    {
        // Arrange
        var bounds = new LngLatBounds
        {
            Southwest = new LngLat(-10, -20),
            Northeast = new LngLat(10, 20)
        };

        // Act
        var center = bounds.GetCenter();

        // Assert
        center.Longitude.Should().Be(0);
        center.Latitude.Should().Be(0);
    }

    [Theory]
    [InlineData(5, 5, true)]
    [InlineData(0, 0, true)] // on the boundary
    [InlineData(20, 5, false)]
    [InlineData(5, -20, false)]
    public void LngLatBounds_Contains_Should_Detect_Points_Inside_And_Outside(double lng, double lat, bool expected)
    {
        // Arrange
        var bounds = new LngLatBounds
        {
            Southwest = new LngLat(0, 0),
            Northeast = new LngLat(10, 10)
        };

        // Act
        var result = bounds.Contains(new LngLat(lng, lat));

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void LngLatBounds_Contains_Should_Handle_AntiMeridian_Crossing_Bounds()
    {
        // Arrange - bounds crossing the ±180 meridian (sw.lng > ne.lng)
        var bounds = new LngLatBounds
        {
            Southwest = new LngLat(170, -10),
            Northeast = new LngLat(-170, 10)
        };

        // Act & Assert - 175 and -175 are inside the wrapped range, 0 is not
        bounds.Contains(new LngLat(175, 0)).Should().BeTrue();
        bounds.Contains(new LngLat(-175, 0)).Should().BeTrue();
        bounds.Contains(new LngLat(0, 0)).Should().BeFalse();
    }

    [Fact]
    public void LngLatBounds_ToArray_Should_Return_Nested_SwNe_Array()
    {
        // Arrange
        var bounds = new LngLatBounds
        {
            Southwest = new LngLat(-1, -2),
            Northeast = new LngLat(3, 4)
        };

        // Act
        var array = bounds.ToArray();

        // Assert
        array.Should().HaveCount(2);
        array[0].Should().Equal(-1, -2);
        array[1].Should().Equal(3, 4);
    }

    [Fact]
    public void LngLatBounds_ToString_Should_Format_Coordinates()
    {
        // Arrange
        var bounds = new LngLatBounds
        {
            Southwest = new LngLat(-1, -2),
            Northeast = new LngLat(3, 4)
        };

        // Act
        var result = bounds.ToString();

        // Assert
        result.Should().Be("LngLatBounds([-1, -2], [3, 4])");
    }

    [Fact]
    public void LngLatBounds_AdjustAntiMeridian_Should_Unwrap_Crossing_Bounds()
    {
        // Arrange - southwest longitude greater than northeast longitude means the bounds cross the meridian
        var bounds = new LngLatBounds
        {
            Southwest = new LngLat(170, -10),
            Northeast = new LngLat(-170, 10)
        };

        // Act
        var adjusted = bounds.AdjustAntiMeridian();

        // Assert
        adjusted.Southwest.Longitude.Should().Be(170);
        adjusted.Northeast.Longitude.Should().Be(190); // -170 + 360
    }

    [Fact]
    public void LngLatBounds_AdjustAntiMeridian_Should_Return_Same_Bounds_When_Not_Crossing()
    {
        // Arrange
        var bounds = new LngLatBounds
        {
            Southwest = new LngLat(0, 0),
            Northeast = new LngLat(10, 10)
        };

        // Act
        var adjusted = bounds.AdjustAntiMeridian();

        // Assert
        adjusted.Should().BeSameAs(bounds);
    }

    [Fact]
    public void LngLatBounds_FromLngLat_Should_Create_Bounds_Around_Center_Point()
    {
        // Arrange
        var center = new LngLat(0, 0);

        // Act
        var bounds = LngLatBounds.FromLngLat(center, 1000);

        // Assert
        bounds.Southwest.Longitude.Should().BeLessThan(center.Longitude);
        bounds.Southwest.Latitude.Should().BeLessThan(center.Latitude);
        bounds.Northeast.Longitude.Should().BeGreaterThan(center.Longitude);
        bounds.Northeast.Latitude.Should().BeGreaterThan(center.Latitude);
        bounds.Contains(center).Should().BeTrue();
    }

    // ----- PointLike -----

    [Fact]
    public void PointLike_Should_Serialize_Successfully()
    {
        // Arrange
        var point = new PointLike(100.5, 200.25);

        // Act
        var json = JsonSerializer.Serialize(point);

        // Assert
        json.Should().Be("{\"x\":100.5,\"y\":200.25}");
    }

    [Fact]
    public void PointLike_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = "{\"x\":50,\"y\":75}";

        // Act
        var point = JsonSerializer.Deserialize<PointLike>(json);

        // Assert
        point.Should().NotBeNull();
        point!.X.Should().Be(50);
        point.Y.Should().Be(75);
    }

    [Fact]
    public void PointLike_FromArray_Should_Create_Point_From_TwoElement_Array()
    {
        // Act
        var point = PointLike.FromArray(new[] { 12.5, -7.5 });

        // Assert
        point.X.Should().Be(12.5);
        point.Y.Should().Be(-7.5);
    }

    [Theory]
    [InlineData(new double[] { 1.0 })]
    [InlineData(new double[] { 1.0, 2.0, 3.0 })]
    [InlineData(new double[] { })]
    public void PointLike_FromArray_Should_Throw_When_Not_Exactly_Two_Elements(double[] coordinates)
    {
        // Act
        Action act = () => PointLike.FromArray(coordinates);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void PointLike_ToString_Should_Format_Coordinates()
    {
        // Arrange
        var point = new PointLike(1, 2);

        // Act & Assert
        point.ToString().Should().Be("PointLike(1, 2)");
    }

    // ----- Popup -----

    [Fact]
    public void Popup_Should_Serialize_Successfully()
    {
        // Arrange
        var popup = new Popup
        {
            Content = "<p>Hello world</p>",
            Coordinates = new LngLat(-122.4194, 37.7749)
        };

        // Act
        var json = JsonSerializer.Serialize(popup);

        // Assert - System.Text.Json HTML-escapes '<'/'>' by default (e.g. "<p>"), so assert
        // on the parsed value rather than a raw substring; the escaping is semantically transparent
        // once JS's JSON.parse decodes it back.
        using var doc = JsonDocument.Parse(json);
        doc.RootElement.GetProperty("content").GetString().Should().Be("<p>Hello world</p>");
        json.Should().Contain("\"lngLat\":");
        json.Should().Contain("\"lng\":-122.4194");
        json.Should().Contain("\"lat\":37.7749");
    }

    [Fact]
    public void Popup_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """
                   {
                        "content":"<b>Marker info</b>",
                        "lngLat":{"lng":10.5,"lat":20.5}
                   }
                   """;

        // Act
        var popup = JsonSerializer.Deserialize<Popup>(json);

        // Assert
        popup.Should().NotBeNull();
        popup!.Content.Should().Be("<b>Marker info</b>");
        popup.Coordinates.Longitude.Should().Be(10.5);
        popup.Coordinates.Latitude.Should().Be(20.5);
    }

    // ----- PopupOptions -----

    [Fact]
    public void PopupOptions_Should_Serialize_Successfully_With_All_Properties()
    {
        // Arrange - realistic usage as passed to MapLibre popup APIs
        var options = new PopupOptions
        {
            ClassName = "my-popup",
            CloseButton = false,
            CloseOnClick = true,
            CloseOnMove = true,
            FocusAfterOpen = false,
            LocationOccludedOpacity = 0.2,
            MaxWidth = "320px",
            Offset = 10,
            Anchor = PositionAnchor.Bottom,
            SubpixelPositioning = true
        };

        // Act
        var json = JsonSerializer.Serialize(options, StringEnumOptions);

        // Assert
        json.Should().Contain("\"className\":\"my-popup\"");
        json.Should().Contain("\"closeButton\":false");
        json.Should().Contain("\"closeOnClick\":true");
        json.Should().Contain("\"closeOnMove\":true");
        json.Should().Contain("\"focusAfterOpen\":false");
        json.Should().Contain("\"locationOccludedOpacity\":0.2");
        json.Should().Contain("\"maxWidth\":\"320px\"");
        json.Should().Contain("\"offset\":10");
        json.Should().Contain("\"anchor\":\"bottom\"");
        json.Should().Contain("\"subpixelPositioning\":true");
    }

    [Fact]
    public void PopupOptions_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """
                   {
                        "className":"my-popup",
                        "closeButton":true,
                        "closeOnClick":false,
                        "maxWidth":"none",
                        "anchor":"top-left"
                   }
                   """;

        // Act
        var options = JsonSerializer.Deserialize<PopupOptions>(json, StringEnumOptions);

        // Assert
        options.Should().NotBeNull();
        options!.ClassName.Should().Be("my-popup");
        options.CloseButton.Should().BeTrue();
        options.CloseOnClick.Should().BeFalse();
        options.MaxWidth.Should().Be("none");
        options.Anchor.Should().Be(PositionAnchor.TopLeft);
    }

    [Fact]
    public void PopupOptions_With_Null_Properties_Should_Omit_Them_From_Json()
    {
        // Arrange
        var options = new PopupOptions();

        // Act
        var json = JsonSerializer.Serialize(options);

        // Assert
        json.Should().Be("{}");
    }

    // ----- PositionAnchor -----

    [Theory]
    [InlineData(PositionAnchor.Center)]
    [InlineData(PositionAnchor.Top)]
    [InlineData(PositionAnchor.Bottom)]
    [InlineData(PositionAnchor.Left)]
    [InlineData(PositionAnchor.Right)]
    [InlineData(PositionAnchor.TopLeft)]
    [InlineData(PositionAnchor.TopRight)]
    [InlineData(PositionAnchor.BottomLeft)]
    [InlineData(PositionAnchor.BottomRight)]
    public void PositionAnchor_Should_RoundTrip_Through_Default_Json_Serialization(PositionAnchor anchor)
    {
        // PositionAnchor has no JsonStringEnumConverter attribute on the enum itself, so a
        // plain Serialize/Deserialize round-trips through the numeric representation.
        var json = JsonSerializer.Serialize(anchor);
        var result = JsonSerializer.Deserialize<PositionAnchor>(json);

        result.Should().Be(anchor);
    }

    [Theory]
    [InlineData(PositionAnchor.Center, "center")]
    [InlineData(PositionAnchor.Top, "top")]
    [InlineData(PositionAnchor.Bottom, "bottom")]
    [InlineData(PositionAnchor.Left, "left")]
    [InlineData(PositionAnchor.Right, "right")]
    [InlineData(PositionAnchor.TopLeft, "top-left")]
    [InlineData(PositionAnchor.TopRight, "top-right")]
    [InlineData(PositionAnchor.BottomLeft, "bottom-left")]
    [InlineData(PositionAnchor.BottomRight, "bottom-right")]
    public void PositionAnchor_With_StringEnumConverter_Should_Serialize_To_MapLibre_Anchor_Name(
        PositionAnchor anchor, string expectedName)
    {
        var json = JsonSerializer.Serialize(anchor, StringEnumOptions);

        json.Should().Be($"\"{expectedName}\"");
    }

    [Theory]
    [InlineData("center", PositionAnchor.Center)]
    [InlineData("top", PositionAnchor.Top)]
    [InlineData("bottom", PositionAnchor.Bottom)]
    [InlineData("left", PositionAnchor.Left)]
    [InlineData("right", PositionAnchor.Right)]
    [InlineData("top-left", PositionAnchor.TopLeft)]
    [InlineData("top-right", PositionAnchor.TopRight)]
    [InlineData("bottom-left", PositionAnchor.BottomLeft)]
    [InlineData("bottom-right", PositionAnchor.BottomRight)]
    public void PositionAnchor_With_StringEnumConverter_Should_Deserialize_From_MapLibre_Anchor_Name(
        string json, PositionAnchor expected)
    {
        var result = JsonSerializer.Deserialize<PositionAnchor>($"\"{json}\"", StringEnumOptions);

        result.Should().Be(expected);
    }

    // ----- ProjectionSpecification -----

    [Fact]
    public void ProjectionSpecification_With_String_Type_Should_Serialize_Successfully()
    {
        // Arrange
        var projection = new ProjectionSpecification
        {
            Type = "globe"
        };

        // Act
        var json = JsonSerializer.Serialize(projection);

        // Assert
        json.Should().Be("{\"type\":\"globe\"}");
    }

    [Fact]
    public void ProjectionSpecification_With_Array_Type_Should_Serialize_Successfully()
    {
        // Arrange - an interpolation expression by zoom level
        var expression = new JsonArray("interpolate", new JsonArray("linear"), "zoom", 0, "mercator", 5, "globe");
        var projection = new ProjectionSpecification
        {
            Type = expression
        };

        // Act
        var json = JsonSerializer.Serialize(projection);

        // Assert
        json.Should().Contain("\"type\":[\"interpolate\"");
        json.Should().Contain("\"globe\"");
    }

    [Fact]
    public void ProjectionSpecification_With_Null_Type_Should_Omit_It_From_Json()
    {
        // Arrange
        var projection = new ProjectionSpecification();

        // Act
        var json = JsonSerializer.Serialize(projection);

        // Assert
        json.Should().Be("{}");
    }

    [Fact]
    public void ProjectionSpecification_Should_Deserialize_String_Type_Successfully()
    {
        // Arrange
        var json = "{\"type\":\"mercator\"}";

        // Act
        var projection = JsonSerializer.Deserialize<ProjectionSpecification>(json);

        // Assert
        projection.Should().NotBeNull();
        projection!.Type.Should().NotBeNull();
        projection.Type!.Value.IsT0.Should().BeTrue();
        projection.Type.Value.AsT0.Should().Be("mercator");
    }

    [Fact]
    public void ProjectionSpecification_Deserialize_Of_Array_Type_Should_Throw()
    {
        // Arrange - OneOfJsonConverter.Read only implements the T1 (string) branch; it's
        // documented as "only intended for serialization" and throws for array input.
        var json = "{\"type\":[\"interpolate\",[\"linear\"],\"zoom\"]}";

        // Act
        Action act = () => JsonSerializer.Deserialize<ProjectionSpecification>(json);

        // Assert
        act.Should().Throw<NotImplementedException>();
    }

    // ----- TextFit -----

    [Theory]
    [InlineData(TextFit.Proportional)]
    [InlineData(TextFit.StretchOnly)]
    [InlineData(TextFit.StretchOrShrink)]
    public void TextFit_Should_RoundTrip_Through_Default_Json_Serialization(TextFit textFit)
    {
        var json = JsonSerializer.Serialize(textFit);
        var result = JsonSerializer.Deserialize<TextFit>(json);

        result.Should().Be(textFit);
    }

    [Theory]
    [InlineData(TextFit.Proportional, "proportional")]
    [InlineData(TextFit.StretchOnly, "stretchOnly")]
    [InlineData(TextFit.StretchOrShrink, "stretchOrShrink")]
    public void TextFit_With_StringEnumConverter_Should_Serialize_To_MapLibre_Name(TextFit textFit, string expectedName)
    {
        var json = JsonSerializer.Serialize(textFit, StringEnumOptions);

        json.Should().Be($"\"{expectedName}\"");
    }

    [Theory]
    [InlineData("proportional", TextFit.Proportional)]
    [InlineData("stretchOnly", TextFit.StretchOnly)]
    [InlineData("stretchOrShrink", TextFit.StretchOrShrink)]
    public void TextFit_With_StringEnumConverter_Should_Deserialize_From_MapLibre_Name(string json, TextFit expected)
    {
        var result = JsonSerializer.Deserialize<TextFit>($"\"{json}\"", StringEnumOptions);

        result.Should().Be(expected);
    }

    // ----- Visibility -----

    [Theory]
    [InlineData(Visibility.Visible)]
    [InlineData(Visibility.None)]
    public void Visibility_Should_RoundTrip_Through_Default_Json_Serialization(Visibility visibility)
    {
        var json = JsonSerializer.Serialize(visibility);
        var result = JsonSerializer.Deserialize<Visibility>(json);

        result.Should().Be(visibility);
    }

    [Theory]
    [InlineData(Visibility.Visible, "visible")]
    [InlineData(Visibility.None, "none")]
    public void Visibility_With_StringEnumConverter_Should_Serialize_To_MapLibre_Name(Visibility visibility, string expectedName)
    {
        var json = JsonSerializer.Serialize(visibility, StringEnumOptions);

        json.Should().Be($"\"{expectedName}\"");
    }

    [Theory]
    [InlineData("visible", Visibility.Visible)]
    [InlineData("none", Visibility.None)]
    public void Visibility_With_StringEnumConverter_Should_Deserialize_From_MapLibre_Name(string json, Visibility expected)
    {
        var result = JsonSerializer.Deserialize<Visibility>($"\"{json}\"", StringEnumOptions);

        result.Should().Be(expected);
    }

    // ----- WebGLContextAttributes -----

    [Fact]
    public void WebGLContextAttributes_Should_Have_Expected_Default_Values()
    {
        // Arrange & Act
        var attributes = new WebGLContextAttributes();

        // Assert
        attributes.PowerPreference.Should().Be("high-performance");
        attributes.ContextType.Should().Be("webgl2withfallback");
        attributes.Antialias.Should().BeNull();
        attributes.PreserveDrawingBuffer.Should().BeNull();
        attributes.FailIfMajorPerformanceCaveat.Should().BeNull();
        attributes.Desynchronized.Should().BeNull();
    }

    [Fact]
    public void WebGLContextAttributes_Should_Serialize_Successfully_With_All_Properties()
    {
        // Arrange - no [JsonPropertyName] attributes on this type, so property names
        // serialize as-is (PascalCase).
        var attributes = new WebGLContextAttributes
        {
            Antialias = true,
            PowerPreference = "low-power",
            PreserveDrawingBuffer = false,
            FailIfMajorPerformanceCaveat = true,
            Desynchronized = false,
            ContextType = "webgl2"
        };

        // Act
        var json = JsonSerializer.Serialize(attributes);

        // Assert
        json.Should().Contain("\"Antialias\":true");
        json.Should().Contain("\"PowerPreference\":\"low-power\"");
        json.Should().Contain("\"PreserveDrawingBuffer\":false");
        json.Should().Contain("\"FailIfMajorPerformanceCaveat\":true");
        json.Should().Contain("\"Desynchronized\":false");
        json.Should().Contain("\"ContextType\":\"webgl2\"");
    }

    [Fact]
    public void WebGLContextAttributes_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """
                   {
                        "Antialias":false,
                        "PowerPreference":"default",
                        "PreserveDrawingBuffer":true,
                        "FailIfMajorPerformanceCaveat":false,
                        "Desynchronized":true,
                        "ContextType":"webgl"
                   }
                   """;

        // Act
        var attributes = JsonSerializer.Deserialize<WebGLContextAttributes>(json);

        // Assert
        attributes.Should().NotBeNull();
        attributes!.Antialias.Should().BeFalse();
        attributes.PowerPreference.Should().Be("default");
        attributes.PreserveDrawingBuffer.Should().BeTrue();
        attributes.FailIfMajorPerformanceCaveat.Should().BeFalse();
        attributes.Desynchronized.Should().BeTrue();
        attributes.ContextType.Should().Be("webgl");
    }

    [Fact]
    public void WebGLContextAttributes_Deserialize_Of_Empty_Json_Should_Use_Defaults()
    {
        // Arrange
        var json = "{}";

        // Act
        var attributes = JsonSerializer.Deserialize<WebGLContextAttributes>(json);

        // Assert
        attributes.Should().NotBeNull();
        attributes!.PowerPreference.Should().Be("high-performance");
        attributes.ContextType.Should().Be("webgl2withfallback");
        attributes.Antialias.Should().BeNull();
    }
}
