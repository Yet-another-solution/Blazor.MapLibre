using System.Text.Json;
using Community.Blazor.MapLibre.Models.Marker;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests;

/// <summary>
/// Tests for the marker support types that are not covered by MarkerOptionsSerializationTests
/// or EventModelSerializationTests: MarkerAlignment, MarkerAnchor, and MarkerOptionsExtensions.
/// </summary>
public class MarkerSupportSerializationTests
{
    [Theory]
    [InlineData(MarkerAlignment.Auto, "\"auto\"")]
    [InlineData(MarkerAlignment.Map, "\"map\"")]
    [InlineData(MarkerAlignment.Viewport, "\"viewport\"")]
    public void MarkerAlignment_Should_Serialize_To_Expected_String(MarkerAlignment alignment, string expectedJson)
    {
        var json = JsonSerializer.Serialize(alignment);

        json.Should().Be(expectedJson);
    }

    [Theory]
    [InlineData("\"auto\"", MarkerAlignment.Auto)]
    [InlineData("\"map\"", MarkerAlignment.Map)]
    [InlineData("\"viewport\"", MarkerAlignment.Viewport)]
    public void MarkerAlignment_Should_Deserialize_From_Expected_String(string json, MarkerAlignment expected)
    {
        var alignment = JsonSerializer.Deserialize<MarkerAlignment>(json);

        alignment.Should().Be(expected);
    }

    [Theory]
    [InlineData(MarkerAnchor.Center, "\"center\"")]
    [InlineData(MarkerAnchor.Top, "\"top\"")]
    [InlineData(MarkerAnchor.Bottom, "\"bottom\"")]
    [InlineData(MarkerAnchor.Left, "\"left\"")]
    [InlineData(MarkerAnchor.Right, "\"right\"")]
    [InlineData(MarkerAnchor.TopLeft, "\"top-left\"")]
    [InlineData(MarkerAnchor.TopRight, "\"top-right\"")]
    [InlineData(MarkerAnchor.BottomLeft, "\"bottom-left\"")]
    [InlineData(MarkerAnchor.BottomRight, "\"bottom-right\"")]
    public void MarkerAnchor_Should_Serialize_To_Expected_String(MarkerAnchor anchor, string expectedJson)
    {
        var json = JsonSerializer.Serialize(anchor);

        json.Should().Be(expectedJson);
    }

    [Theory]
    [InlineData("\"center\"", MarkerAnchor.Center)]
    [InlineData("\"top\"", MarkerAnchor.Top)]
    [InlineData("\"bottom\"", MarkerAnchor.Bottom)]
    [InlineData("\"left\"", MarkerAnchor.Left)]
    [InlineData("\"right\"", MarkerAnchor.Right)]
    [InlineData("\"top-left\"", MarkerAnchor.TopLeft)]
    [InlineData("\"top-right\"", MarkerAnchor.TopRight)]
    [InlineData("\"bottom-left\"", MarkerAnchor.BottomLeft)]
    [InlineData("\"bottom-right\"", MarkerAnchor.BottomRight)]
    public void MarkerAnchor_Should_Deserialize_From_Expected_String(string json, MarkerAnchor expected)
    {
        var anchor = JsonSerializer.Deserialize<MarkerAnchor>(json);

        anchor.Should().Be(expected);
    }

    [Fact]
    public void MarkerOptionsExtensions_Should_Serialize_With_Default_Empty_Strings()
    {
        // Arrange
        var extensions = new MarkerOptionsExtensions();

        // Act
        var json = JsonSerializer.Serialize(extensions);

        // Assert
        extensions.HtmlContent.Should().BeEmpty();
        extensions.PopupHtmlContent.Should().BeEmpty();
        json.Should().Contain("\"htmlContent\":\"\"");
        json.Should().Contain("\"popupHtmlContent\":\"\"");
    }

    [Fact]
    public void MarkerOptionsExtensions_Should_Serialize_With_Provided_Content()
    {
        // Arrange
        var extensions = new MarkerOptionsExtensions
        {
            HtmlContent = "<b>Marker</b>",
            PopupHtmlContent = "<p>Popup</p>"
        };

        // Act
        var json = JsonSerializer.Serialize(extensions);

        // Assert - System.Text.Json HTML-escapes '<'/'>' by default, so assert on the parsed value
        // rather than a raw substring; semantically transparent once JS's JSON.parse decodes it.
        using var doc = JsonDocument.Parse(json);
        doc.RootElement.GetProperty("htmlContent").GetString().Should().Be("<b>Marker</b>");
        doc.RootElement.GetProperty("popupHtmlContent").GetString().Should().Be("<p>Popup</p>");
    }

    [Fact]
    public void MarkerOptionsExtensions_Should_Deserialize_Successfully()
    {
        // Arrange
        var json = "{\"htmlContent\":\"<b>Marker</b>\",\"popupHtmlContent\":\"<p>Popup</p>\"}";

        // Act
        var extensions = JsonSerializer.Deserialize<MarkerOptionsExtensions>(json);

        // Assert
        extensions.Should().NotBeNull();
        extensions!.HtmlContent.Should().Be("<b>Marker</b>");
        extensions.PopupHtmlContent.Should().Be("<p>Popup</p>");
    }

    [Fact]
    public void MarkerOptionsExtensions_Should_Deserialize_Missing_Properties_To_Default_Empty_Strings()
    {
        // Arrange
        var json = "{}";

        // Act
        var extensions = JsonSerializer.Deserialize<MarkerOptionsExtensions>(json);

        // Assert
        extensions.Should().NotBeNull();
        extensions!.HtmlContent.Should().BeEmpty();
        extensions.PopupHtmlContent.Should().BeEmpty();
    }

    [Fact]
    public void MarkerOptions_Anchor_And_Alignments_Should_Round_Trip_Through_MarkerOptions()
    {
        // Arrange
        var options = new MarkerOptions
        {
            Anchor = MarkerAnchor.BottomLeft,
            PitchAlignment = MarkerAlignment.Map,
            RotationAlignment = MarkerAlignment.Viewport,
            Extensions = new MarkerOptionsExtensions
            {
                HtmlContent = "<span>Hi</span>"
            }
        };

        // Act
        var json = JsonSerializer.Serialize(options);
        var deserialized = JsonSerializer.Deserialize<MarkerOptions>(json);

        // Assert
        json.Should().Contain("\"anchor\":\"bottom-left\"");
        json.Should().Contain("\"pitchAlignment\":\"map\"");
        json.Should().Contain("\"rotationAlignment\":\"viewport\"");

        deserialized.Should().NotBeNull();
        deserialized!.Anchor.Should().Be(MarkerAnchor.BottomLeft);
        deserialized.PitchAlignment.Should().Be(MarkerAlignment.Map);
        deserialized.RotationAlignment.Should().Be(MarkerAlignment.Viewport);
        deserialized.Extensions.Should().NotBeNull();
        deserialized.Extensions!.HtmlContent.Should().Be("<span>Hi</span>");
        deserialized.Extensions.PopupHtmlContent.Should().BeEmpty();
    }
}
