using Bunit;
using Community.Blazor.MapLibre.Models.Layers;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests.Interop;

/// <summary>
/// Proof-of-concept for the JS-interop contract test harness (<see cref="MapLibreInteropTestBase"/>):
/// confirms <see cref="global::Community.Blazor.MapLibre.MapLibre.AddLayer"/> invokes the JS "addLayer"
/// function with the map's id, the layer, and beforeId, in that order.
/// </summary>
public class AddLayerInteropTests : MapLibreInteropTestBase
{
    [Fact]
    public async Task AddLayer_Should_Invoke_AddLayer_With_MapId_Layer_And_BeforeId()
    {
        // Arrange
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var layer = new CircleLayer { Id = "circle-1", Source = "source-1" };

        // Act
        await cut.Instance.AddLayer(layer, beforeId: "existing-layer");

        // Assert
        var invocation = JSInterop.VerifyInvoke("addLayer");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(layer);
        invocation.Arguments[2].Should().Be("existing-layer");
    }

    [Fact]
    public async Task AddLayer_With_No_BeforeId_Should_Pass_Null()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var layer = new CircleLayer { Id = "circle-2", Source = "source-1" };

        await cut.Instance.AddLayer(layer);

        var invocation = JSInterop.VerifyInvoke("addLayer");
        invocation.Arguments[2].Should().BeNull();
    }
}
