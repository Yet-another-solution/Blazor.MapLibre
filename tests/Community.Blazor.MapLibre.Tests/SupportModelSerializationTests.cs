using System.Text.Json;
using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Models;
using Community.Blazor.MapLibre.Models.Image;
using Community.Blazor.MapLibre.Models.Padding;
using Community.Blazor.MapLibre.Models.Sprite;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests;

/// <summary>
/// Tests covering the small, previously-untested support model folders:
/// Models/Image (ImageData, StyleImageMetadata), Models/Sprite (StyleSetterOptions),
/// and Models/Padding (PaddingOptions).
/// </summary>
public class SupportModelSerializationTests
{
    // TextFit carries [JsonStringEnumMemberName], which only takes effect when a
    // JsonStringEnumConverter is applied (StyleImageMetadata.TextFitHeight/TextFitWidth
    // have no property-level [JsonConverter], so plain JsonSerializer calls serialize
    // them as their underlying numeric value; mirrors ControlSerializationTests).
    private static readonly JsonSerializerOptions StringEnumOptions = new()
    {
        Converters = { new JsonStringEnumConverter() }
    };

    // ----- ImageData -----

    [Fact]
    public void ImageData_Should_Serialize_Successfully()
    {
        // Arrange
        var imageData = new ImageData
        {
            Width = 800,
            Height = 600
        };

        // Act
        var json = JsonSerializer.Serialize(imageData);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"Width\":800");
        json.Should().Contain("\"Height\":600");
    }

    [Fact]
    public void ImageData_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """
                   {
                        "Width":256,
                        "Height":128
                   }
                   """;

        // Act
        var imageData = JsonSerializer.Deserialize<ImageData>(json);

        // Assert
        imageData.Should().NotBeNull();
        imageData!.Width.Should().Be(256);
        imageData.Height.Should().Be(128);
    }

    // ----- StyleImageMetadata -----

    [Fact]
    public void StyleImageMetadata_Should_Serialize_Successfully_With_All_Properties()
    {
        // Arrange - realistic values as used by MapLibre.AddImage
        var metadata = new StyleImageMetadata
        {
            Content = new[] { 10.0, 10.0, 20.0, 20.0 },
            PixelRatio = 2.0,
            Sdf = true,
            StretchX = new[] { new[] { 5.0, 15.0 } },
            StretchY = new[] { new[] { 5.0, 15.0 } },
            TextFitHeight = TextFit.Proportional,
            TextFitWidth = TextFit.StretchOrShrink
        };

        // Act
        var json = JsonSerializer.Serialize(metadata);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"content\":[10,10,20,20]");
        json.Should().Contain("\"pixelRatio\":2");
        json.Should().Contain("\"sdf\":true");
        json.Should().Contain("\"stretchX\":[[5,15]]");
        json.Should().Contain("\"stretchY\":[[5,15]]");
        // TextFit carries [JsonConverter(typeof(JsonStringEnumConverter))] on the enum itself,
        // so even a plain Serialize call writes the MapLibre style-spec string, not a number.
        json.Should().Contain("\"textFitHeight\":\"proportional\"");
        json.Should().Contain("\"textFitWidth\":\"stretchOrShrink\"");
    }

    [Fact]
    public void StyleImageMetadata_With_StringEnumConverter_Should_Serialize_TextFit_As_String()
    {
        // Arrange - an explicitly-supplied JsonStringEnumConverter changes nothing here (the
        // enum's own type-level converter is already active), but confirms it's harmless to add one.
        var metadata = new StyleImageMetadata
        {
            TextFitHeight = TextFit.Proportional,
            TextFitWidth = TextFit.StretchOrShrink
        };

        // Act
        var json = JsonSerializer.Serialize(metadata, StringEnumOptions);

        // Assert
        json.Should().Contain("\"textFitHeight\":\"proportional\"");
        json.Should().Contain("\"textFitWidth\":\"stretchOrShrink\"");
    }

    [Fact]
    public void StyleImageMetadata_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """
                   {
                        "content":[10,10,20,20],
                        "pixelRatio":2,
                        "sdf":true,
                        "stretchX":[[5,15]],
                        "stretchY":[[5,15]],
                        "textFitHeight":"proportional",
                        "textFitWidth":"stretchOnly"
                   }
                   """;

        // Act
        var metadata = JsonSerializer.Deserialize<StyleImageMetadata>(json, StringEnumOptions);

        // Assert
        metadata.Should().NotBeNull();
        metadata!.Content.Should().Equal(10.0, 10.0, 20.0, 20.0);
        metadata.PixelRatio.Should().Be(2.0);
        metadata.Sdf.Should().BeTrue();
        metadata.StretchX.Should().BeEquivalentTo(new[] { new[] { 5.0, 15.0 } });
        metadata.StretchY.Should().BeEquivalentTo(new[] { new[] { 5.0, 15.0 } });
        metadata.TextFitHeight.Should().Be(TextFit.Proportional);
        metadata.TextFitWidth.Should().Be(TextFit.StretchOnly);
    }

    [Fact]
    public void StyleImageMetadata_With_Null_Optional_Properties_Should_Omit_Them_From_Json()
    {
        // Arrange - all properties are optional and JsonIgnore(WhenWritingNull)
        var metadata = new StyleImageMetadata();

        // Act
        var json = JsonSerializer.Serialize(metadata);

        // Assert
        json.Should().Be("{}");
        json.Should().NotContain("content");
        json.Should().NotContain("pixelRatio");
        json.Should().NotContain("sdf");
        json.Should().NotContain("stretchX");
        json.Should().NotContain("stretchY");
        json.Should().NotContain("textFitHeight");
        json.Should().NotContain("textFitWidth");
    }

    [Fact]
    public void StyleImageMetadata_Empty_Json_Should_Deserialize_To_All_Null_Properties()
    {
        // Arrange
        var json = "{}";

        // Act
        var metadata = JsonSerializer.Deserialize<StyleImageMetadata>(json);

        // Assert
        metadata.Should().NotBeNull();
        metadata!.Content.Should().BeNull();
        metadata.PixelRatio.Should().BeNull();
        metadata.Sdf.Should().BeNull();
        metadata.StretchX.Should().BeNull();
        metadata.StretchY.Should().BeNull();
        metadata.TextFitHeight.Should().BeNull();
        metadata.TextFitWidth.Should().BeNull();
    }

    // ----- StyleSetterOptions -----

    [Fact]
    public void StyleSetterOptions_Should_Serialize_Successfully()
    {
        // Arrange - realistic usage as passed to MapLibre.SetFilter / SetLayoutProperty / AddSprite
        var options = new StyleSetterOptions
        {
            Validate = false
        };

        // Act
        var json = JsonSerializer.Serialize(options);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"validate\":false");
    }

    [Fact]
    public void StyleSetterOptions_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """
                   {
                        "validate":true
                   }
                   """;

        // Act
        var options = JsonSerializer.Deserialize<StyleSetterOptions>(json);

        // Assert
        options.Should().NotBeNull();
        options!.Validate.Should().BeTrue();
    }

    [Fact]
    public void StyleSetterOptions_With_Null_Validate_Should_Omit_It_From_Json()
    {
        // Arrange
        var options = new StyleSetterOptions();

        // Act
        var json = JsonSerializer.Serialize(options);

        // Assert
        json.Should().Be("{}");
        json.Should().NotContain("validate");
    }

    // ----- PaddingOptions -----

    [Fact]
    public void PaddingOptions_Should_Serialize_Successfully()
    {
        // Arrange - realistic usage as shown in MapLibre.FitScreenCoordinates docs
        var padding = new PaddingOptions
        {
            Top = 10,
            Bottom = 25,
            Left = 15,
            Right = 5
        };

        // Act
        var json = JsonSerializer.Serialize(padding);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"top\":10");
        json.Should().Contain("\"bottom\":25");
        json.Should().Contain("\"right\":5");
        json.Should().Contain("\"left\":15");
    }

    [Fact]
    public void PaddingOptions_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = """
                   {
                        "top":10,
                        "bottom":25,
                        "right":5,
                        "left":15
                   }
                   """;

        // Act
        var padding = JsonSerializer.Deserialize<PaddingOptions>(json);

        // Assert
        padding.Should().NotBeNull();
        padding!.Top.Should().Be(10);
        padding.Bottom.Should().Be(25);
        padding.Right.Should().Be(5);
        padding.Left.Should().Be(15);
    }

    [Fact]
    public void PaddingOptions_With_Null_Properties_Should_Omit_Them_From_Json()
    {
        // Arrange
        var padding = new PaddingOptions();

        // Act
        var json = JsonSerializer.Serialize(padding);

        // Assert
        json.Should().Be("{}");
        json.Should().NotContain("top");
        json.Should().NotContain("bottom");
        json.Should().NotContain("right");
        json.Should().NotContain("left");
    }

    [Fact]
    public void PaddingOptions_With_Partial_Values_Should_Only_Include_NonNull_Properties()
    {
        // Arrange - a common real usage: uniform padding via only some sides set
        var padding = new PaddingOptions
        {
            Top = 50
        };

        // Act
        var json = JsonSerializer.Serialize(padding);

        // Assert
        json.Should().Be("{\"top\":50}");
    }
}
