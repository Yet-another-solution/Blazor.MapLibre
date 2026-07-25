using System.Text.Json;
using Community.Blazor.MapLibre.Models.Event;
using Community.Blazor.MapLibre.Models.Feature;
using Community.Blazor.MapLibre.Models.LayerFeatures;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests;

/// <summary>
/// Regression test for https://github.com/Yet-another-solution/Blazor.MapLibre/issues/160: deserializing a
/// MapMouseEvent (as sent by CallbackHandler.Invoke from a "click" event) threw
/// DeserializationMustSpecifyTypeDiscriminator on .NET 10. Root cause: IGeometry/IFeature used to declare an
/// explicit `Type` member mapped to the JSON property "type", which is also the interfaces'
/// [JsonPolymorphic] discriminator property name - .NET 10's stricter polymorphism checks reject a member
/// colliding with its own type's discriminator (see dotnet/runtime#118061). That member was already removed
/// in commit 559f754 ("fix: serialization"), which fixes this on main; this test locks that behavior in so
/// it can't silently regress.
/// </summary>
public class LayerFeatureDeserializationTests
{
    // Same options CallbackHandler.Invoke uses to deserialize inbound JS event payloads.
    private static readonly JsonSerializerOptions Options = new()
    {
        AllowOutOfOrderMetadataProperties = true,
    };

    private const string ClickEventJson = """
        {
            "point": { "x": 100, "y": 200 },
            "lngLat": { "lng": -122.4194, "lat": 37.7749 },
            "type": "click",
            "features": [
                {
                    "type": "Feature",
                    "id": 42,
                    "source": "resources",
                    "state": {},
                    "geometry": {
                        "type": "Point",
                        "coordinates": [-122.4194, 37.7749]
                    },
                    "properties": {
                        "title": "San Francisco"
                    }
                }
            ]
        }
        """;

    [Fact]
    public void MapMouseEvent_With_Nested_Feature_Geometry_Should_Deserialize()
    {
        var mapMouseEvent = JsonSerializer.Deserialize<MapMouseEvent>(ClickEventJson, Options);

        mapMouseEvent.Should().NotBeNull();
        mapMouseEvent!.Type.Should().Be(EventType.Click);
        mapMouseEvent.Features.Should().HaveCount(1);

        var feature = mapMouseEvent.Features[0].Should().BeOfType<LayerFeatureFeature>().Subject;
        feature.Geometry.Should().BeOfType<PointGeometry>();
        ((PointGeometry)feature.Geometry).Coordinates.Should().Equal(-122.4194, 37.7749);
    }
}
