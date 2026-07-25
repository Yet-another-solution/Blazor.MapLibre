using System.Text.Json;
using Community.Blazor.MapLibre.Models.Marker;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests;

/// <summary>
/// AddMarker sends MarkerOptions to JS via JsonSerializer. None of these properties are serializable
/// (they're C# delegates, not data), so a missing [JsonIgnore] would either throw on serialize or leak
/// a garbage value into the payload sent to maplibregl.Marker. These tests lock that in.
/// </summary>
public class MarkerOptionsSerializationTests
{
    [Fact]
    public void MarkerOptions_With_All_Event_Handlers_Set_Should_Serialize_Without_Throwing()
    {
        var options = new MarkerOptions
        {
            OnClick = _ => { },
            OnClickAsync = _ => Task.CompletedTask,
            OnDragStart = _ => { },
            OnDragStartAsync = _ => Task.CompletedTask,
            OnDrag = _ => { },
            OnDragAsync = _ => Task.CompletedTask,
            OnDragEnd = _ => { },
            OnDragEndAsync = _ => Task.CompletedTask,
        };

        var act = () => JsonSerializer.Serialize(options);

        act.Should().NotThrow();
    }

    [Fact]
    public void MarkerOptions_Serialized_Json_Should_Not_Contain_Event_Handler_Properties()
    {
        var options = new MarkerOptions
        {
            Draggable = true,
            OnClick = _ => { },
            OnClickAsync = _ => Task.CompletedTask,
            OnDragStart = _ => { },
            OnDragStartAsync = _ => Task.CompletedTask,
            OnDrag = _ => { },
            OnDragAsync = _ => Task.CompletedTask,
            OnDragEnd = _ => { },
            OnDragEndAsync = _ => Task.CompletedTask,
        };

        var json = JsonSerializer.Serialize(options);

        json.Should().NotContainAny(
            "OnClick", "OnClickAsync",
            "OnDragStart", "OnDragStartAsync",
            "OnDrag", "OnDragAsync",
            "OnDragEnd", "OnDragEndAsync",
            "Element");
        json.Should().Contain("\"draggable\":true");
    }
}
