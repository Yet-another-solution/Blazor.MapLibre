using Bunit;
using Community.Blazor.MapLibre.Models;
using Community.Blazor.MapLibre.Models.Padding;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests.Interop;

/// <summary>
/// Contract tests for a batch of read-only "get"/"has" query methods on <see cref="global::Community.Blazor.MapLibre.MapLibre"/>:
/// confirms each invokes the correctly-named JS function with the map's id (and any extra parameters) in the
/// correct order, and that simple return values round-trip back through <c>InvokeAsync</c> correctly.
/// </summary>
public class QueryMethodsBatchBInteropTests : MapLibreInteropTestBase
{
    [Fact]
    public async Task GetLayoutProperty_Should_Invoke_GetLayoutProperty_With_MapId_LayerId_And_Name()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<object>("getLayoutProperty", "test-map", "layer-1", "visibility").SetResult("visible");

        var result = await cut.Instance.GetLayoutProperty("layer-1", "visibility");

        result.Should().Be("visible");
        var invocation = JSInterop.VerifyInvoke("getLayoutProperty");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be("layer-1");
        invocation.Arguments[2].Should().Be("visibility");
    }

    [Fact]
    public async Task GetLayoutProperty_Should_Preserve_LayerId_Name_Order()
    {
        // Regression guard: layerId and name must not be swapped positionally.
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<object>("getLayoutProperty", "test-map", "buildings", "icon-size").SetResult(1.0);

        await cut.Instance.GetLayoutProperty("buildings", "icon-size");

        var invocation = JSInterop.VerifyInvoke("getLayoutProperty");
        invocation.Arguments[1].Should().Be("buildings");
        invocation.Arguments[2].Should().Be("icon-size");
    }

    [Fact]
    public async Task GetLight_Should_Invoke_GetLight_With_MapId()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        await cut.Instance.GetLight();

        JSInterop.VerifyInvoke("getLight").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetMaxBounds_Should_Invoke_GetMaxBounds_And_Return_Bounds()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var bounds = new LngLatBounds
        {
            Southwest = new LngLat(-10, -20),
            Northeast = new LngLat(10, 20)
        };
        JSInterop.Setup<LngLatBounds?>("getMaxBounds", "test-map").SetResult(bounds);

        var result = await cut.Instance.GetMaxBounds();

        result.Should().BeSameAs(bounds);
        JSInterop.VerifyInvoke("getMaxBounds").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetMaxPitch_Should_Invoke_GetMaxPitch_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<double>("getMaxPitch", "test-map").SetResult(85.0);

        var result = await cut.Instance.GetMaxPitch();

        result.Should().Be(85.0);
        JSInterop.VerifyInvoke("getMaxPitch").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetMaxZoom_Should_Invoke_GetMaxZoom_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<double>("getMaxZoom", "test-map").SetResult(22.0);

        var result = await cut.Instance.GetMaxZoom();

        result.Should().Be(22.0);
        JSInterop.VerifyInvoke("getMaxZoom").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetMinPitch_Should_Invoke_GetMinPitch_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<double>("getMinPitch", "test-map").SetResult(0.0);

        var result = await cut.Instance.GetMinPitch();

        result.Should().Be(0.0);
        JSInterop.VerifyInvoke("getMinPitch").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetMinZoom_Should_Invoke_GetMinZoom_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<double>("getMinZoom", "test-map").SetResult(1.0);

        var result = await cut.Instance.GetMinZoom();

        result.Should().Be(1.0);
        JSInterop.VerifyInvoke("getMinZoom").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetPadding_Should_Invoke_GetPadding_And_Return_Options()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var padding = new PaddingOptions { Top = 1, Bottom = 2, Left = 3, Right = 4 };
        JSInterop.Setup<PaddingOptions>("getPadding", "test-map").SetResult(padding);

        var result = await cut.Instance.GetPadding();

        result.Should().BeSameAs(padding);
        JSInterop.VerifyInvoke("getPadding").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetPaintProperty_Should_Invoke_GetPaintProperty_With_MapId_LayerId_And_Name()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<object?>("getPaintProperty", "test-map", "layer-1", "fill-color").SetResult("#ff0000");

        var result = await cut.Instance.GetPaintProperty("layer-1", "fill-color");

        result.Should().Be("#ff0000");
        var invocation = JSInterop.VerifyInvoke("getPaintProperty");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be("layer-1");
        invocation.Arguments[2].Should().Be("fill-color");
    }

    [Fact]
    public async Task GetPaintProperty_Should_Preserve_LayerId_Name_Order()
    {
        // Regression guard: layerId and name must not be swapped positionally.
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<object?>("getPaintProperty", "test-map", "roads", "line-width").SetResult(2.0);

        await cut.Instance.GetPaintProperty("roads", "line-width");

        var invocation = JSInterop.VerifyInvoke("getPaintProperty");
        invocation.Arguments[1].Should().Be("roads");
        invocation.Arguments[2].Should().Be("line-width");
    }

    [Fact]
    public async Task GetPitch_Should_Invoke_GetPitch_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<double>("getPitch", "test-map").SetResult(30.0);

        var result = await cut.Instance.GetPitch();

        result.Should().Be(30.0);
        JSInterop.VerifyInvoke("getPitch").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetPixelRatio_Should_Invoke_GetPixelRatio_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<double>("getPixelRatio", "test-map").SetResult(2.0);

        var result = await cut.Instance.GetPixelRatio();

        result.Should().Be(2.0);
        JSInterop.VerifyInvoke("getPixelRatio").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetProjection_Should_Invoke_GetProjection_With_MapId()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        await cut.Instance.GetProjection();

        JSInterop.VerifyInvoke("getProjection").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetRenderWorldCopies_Should_Invoke_GetRenderWorldCopies_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<bool>("getRenderWorldCopies", "test-map").SetResult(true);

        var result = await cut.Instance.GetRenderWorldCopies();

        result.Should().BeTrue();
        JSInterop.VerifyInvoke("getRenderWorldCopies").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetRoll_Should_Invoke_GetRoll_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<double>("getRoll", "test-map").SetResult(5.0);

        var result = await cut.Instance.GetRoll();

        result.Should().Be(5.0);
        JSInterop.VerifyInvoke("getRoll").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetSky_Should_Invoke_GetSky_With_MapId()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        await cut.Instance.GetSky();

        JSInterop.VerifyInvoke("getSky").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetSource_Should_Invoke_GetSource_With_MapId_And_Id()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        await cut.Instance.GetSource("source-1");

        var invocation = JSInterop.VerifyInvoke("getSource");
        invocation.Arguments.Should().HaveCount(2);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be("source-1");
    }

    [Fact]
    public async Task HasSource_Should_Invoke_HasSource_With_MapId_And_Id_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<bool>("hasSource", "test-map", "source-1").SetResult(true);

        var result = await cut.Instance.HasSource("source-1");

        result.Should().BeTrue();
        var invocation = JSInterop.VerifyInvoke("hasSource");
        invocation.Arguments.Should().HaveCount(2);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be("source-1");
    }
}
