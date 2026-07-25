using System.Text.Json;
using Community.Blazor.MapLibre.Models.Event;
using Community.Blazor.MapLibre.Models.Marker;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests;

/// <summary>
/// Tests for the event payload models sent by MapLibre.razor.js's "onMarker" (marker click/drag events,
/// issue #162) and generic map-event ("rotate"/"pitch", issue #163) dispatch.
/// </summary>
public class EventModelSerializationTests
{
    [Fact]
    public void MarkerEvent_Should_Deserialize_From_Js_Dispatch_Payload()
    {
        const string json = """{"type":"dragend","lngLat":{"lng":-96,"lat":37.8}}""";

        var markerEvent = JsonSerializer.Deserialize<MarkerEvent>(json);

        markerEvent.Should().NotBeNull();
        markerEvent!.Type.Should().Be("dragend");
        markerEvent.LngLat.Longitude.Should().Be(-96);
        markerEvent.LngLat.Latitude.Should().Be(37.8);
    }

    [Theory]
    [InlineData(MapEventType.Rotate, "rotate")]
    [InlineData(MapEventType.Pitch, "pitch")]
    public void MapLibreEvent_Should_Round_Trip_Type(MapEventType type, string jsonValue)
    {
        var mapLibreEvent = new MapLibreEvent { Type = type };

        var json = JsonSerializer.Serialize(mapLibreEvent);
        json.Should().Be($$"""{"type":"{{jsonValue}}"}""");

        var deserialized = JsonSerializer.Deserialize<MapLibreEvent>(json);
        deserialized!.Type.Should().Be(type);
    }
}
