using System.Text.Json;
using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Models.Control;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests;

/// <summary>
/// Serialization round-trip tests for every enum in Models/Control:
/// <see cref="ControlPosition"/> and <see cref="ControlType"/>.
/// Used by <c>MapLibre.AddControl(ControlType, ControlPosition?)</c> in MapLibre.razor.cs.
/// </summary>
public class ControlSerializationTests
{
    // ControlPosition members carry [JsonStringEnumMemberName], which only takes effect
    // when a JsonStringEnumConverter is applied (mirrors how MapMouseEvent.Type is wired
    // via a property-level [JsonConverter(typeof(JsonStringEnumConverter))]).
    private static readonly JsonSerializerOptions StringEnumOptions = new()
    {
        Converters = { new JsonStringEnumConverter() }
    };

    [Theory]
    [InlineData(ControlPosition.TopLeft, "\"top-left\"")]
    [InlineData(ControlPosition.TopRight, "\"top-right\"")]
    [InlineData(ControlPosition.BottomLeft, "\"bottom-left\"")]
    [InlineData(ControlPosition.BottomRight, "\"bottom-right\"")]
    public void ControlPosition_SerializesToExpectedJsonString(ControlPosition position, string expectedJson)
    {
        var json = JsonSerializer.Serialize(position, StringEnumOptions);

        json.Should().Be(expectedJson);
    }

    [Theory]
    [InlineData(ControlPosition.TopLeft, "\"top-left\"")]
    [InlineData(ControlPosition.TopRight, "\"top-right\"")]
    [InlineData(ControlPosition.BottomLeft, "\"bottom-left\"")]
    [InlineData(ControlPosition.BottomRight, "\"bottom-right\"")]
    public void ControlPosition_RoundTrips_ThroughJson(ControlPosition expected, string json)
    {
        var result = JsonSerializer.Deserialize<ControlPosition>(json, StringEnumOptions);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(ControlType.AttributionControl)]
    [InlineData(ControlType.FullscreenControl)]
    [InlineData(ControlType.GeolocateControl)]
    [InlineData(ControlType.GlobeControl)]
    [InlineData(ControlType.LogoControl)]
    [InlineData(ControlType.NavigationControl)]
    [InlineData(ControlType.ScaleControl)]
    [InlineData(ControlType.TerrainControl)]
    public void ControlType_RoundTrips_ThroughDefaultJsonSerialization(ControlType controlType)
    {
        // ControlType has no JsonStringEnumMemberName/JsonStringEnumConverter attribute,
        // so it uses System.Text.Json's default numeric enum representation.
        var json = JsonSerializer.Serialize(controlType);
        var result = JsonSerializer.Deserialize<ControlType>(json);

        result.Should().Be(controlType);
    }

    [Theory]
    [InlineData(ControlType.AttributionControl, "AttributionControl")]
    [InlineData(ControlType.FullscreenControl, "FullscreenControl")]
    [InlineData(ControlType.GeolocateControl, "GeolocateControl")]
    [InlineData(ControlType.GlobeControl, "GlobeControl")]
    [InlineData(ControlType.LogoControl, "LogoControl")]
    [InlineData(ControlType.NavigationControl, "NavigationControl")]
    [InlineData(ControlType.ScaleControl, "ScaleControl")]
    [InlineData(ControlType.TerrainControl, "TerrainControl")]
    public void ControlType_ToString_MatchesJsInteropControlName(ControlType controlType, string expectedName)
    {
        // MapLibre.razor.cs's AddControl() calls controlType.ToString() and passes it
        // straight through to the control-type map in MapLibre.razor.js
        // (AttributionControl, FullscreenControl, ... TerrainControl) - this is the
        // actual wire format used for JS interop, not JSON serialization.
        controlType.ToString().Should().Be(expectedName);
    }
}
