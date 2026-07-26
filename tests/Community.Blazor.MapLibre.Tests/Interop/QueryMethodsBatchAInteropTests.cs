using Bunit;
using Community.Blazor.MapLibre.Models;
using Community.Blazor.MapLibre.Models.Feature;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests.Interop;

/// <summary>
/// JS-interop contract tests for the "query" style getters on <see cref="global::Community.Blazor.MapLibre.MapLibre"/>:
/// each test confirms the C# method invokes the correct JS function, with the correct arguments in the
/// correct order, and (where the return type isn't an opaque <see cref="Microsoft.JSInterop.IJSObjectReference"/>)
/// that the JS return value round-trips back through the method unchanged.
/// </summary>
public class QueryMethodsBatchAInteropTests : MapLibreInteropTestBase
{
    [Fact]
    public async Task AreTilesLoaded_Should_Invoke_AreTilesLoaded_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<bool>("areTilesLoaded", "test-map").SetResult(true);

        var result = await cut.Instance.AreTilesLoaded();

        result.Should().BeTrue();
        JSInterop.VerifyInvoke("areTilesLoaded").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetBearing_Should_Invoke_GetBearing_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<double>("getBearing", "test-map").SetResult(45.0);

        var result = await cut.Instance.GetBearing();

        result.Should().Be(45.0);
        JSInterop.VerifyInvoke("getBearing").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetBounds_Should_Invoke_GetBounds_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var bounds = new LngLatBounds { Southwest = new LngLat(-10, -20), Northeast = new LngLat(10, 20) };
        JSInterop.Setup<LngLatBounds>("getBounds", "test-map").SetResult(bounds);

        var result = await cut.Instance.GetBounds();

        result.Should().BeSameAs(bounds);
        JSInterop.VerifyInvoke("getBounds").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetCameraTargetElevation_Should_Invoke_GetCameraTargetElevation_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<double>("getCameraTargetElevation", "test-map").SetResult(123.4);

        var result = await cut.Instance.GetCameraTargetElevation();

        result.Should().Be(123.4);
        JSInterop.VerifyInvoke("getCameraTargetElevation").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetCanvas_Should_Invoke_GetCanvas_With_MapId()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        await cut.Instance.GetCanvas();

        JSInterop.VerifyInvoke("getCanvas").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetCanvasContainer_Should_Invoke_GetCanvasContainer_With_MapId()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        await cut.Instance.GetCanvasContainer();

        JSInterop.VerifyInvoke("getCanvasContainer").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetCenter_Should_Invoke_GetCenter_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var center = new LngLat(1.5, 2.5);
        JSInterop.Setup<LngLat>("getCenter", "test-map").SetResult(center);

        var result = await cut.Instance.GetCenter();

        result.Should().BeSameAs(center);
        JSInterop.VerifyInvoke("getCenter").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetCenterClampedToGround_Should_Invoke_GetCenterClampedToGround_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<bool>("getCenterClampedToGround", "test-map").SetResult(false);

        var result = await cut.Instance.GetCenterClampedToGround();

        result.Should().BeFalse();
        JSInterop.VerifyInvoke("getCenterClampedToGround").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetCenterElevation_Should_Invoke_GetCenterElevation_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<double>("getCenterElevation", "test-map").SetResult(99.9);

        var result = await cut.Instance.GetCenterElevation();

        result.Should().Be(99.9);
        JSInterop.VerifyInvoke("getCenterElevation").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetContainer_Should_Invoke_GetContainer_With_MapId()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        await cut.Instance.GetContainer();

        JSInterop.VerifyInvoke("getContainer").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetFeatureState_Should_Invoke_GetFeatureState_With_MapId_And_Feature()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var feature = new FeatureIdentifier { Id = "feature-1", Source = "source-1", SourceLayer = "layer-1" };
        object state = new { hover = true };
        JSInterop.Setup<object>("getFeatureState", "test-map", feature).SetResult(state);

        var result = await cut.Instance.GetFeatureState(feature);

        result.Should().BeSameAs(state);
        var invocation = JSInterop.VerifyInvoke("getFeatureState");
        invocation.Arguments.Should().HaveCount(2);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(feature);
    }

    [Fact]
    public async Task GetFeatureState_Should_Pass_Through_All_FeatureIdentifier_Properties()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var feature = new FeatureIdentifier { Id = "feature-42", Source = "source-42", SourceLayer = "layer-42" };
        JSInterop.Setup<object>("getFeatureState", "test-map", feature).SetResult(new object());

        await cut.Instance.GetFeatureState(feature);

        var invocation = JSInterop.VerifyInvoke("getFeatureState");
        var passedFeature = invocation.Arguments[1].Should().BeOfType<FeatureIdentifier>().Subject;
        passedFeature.Id.Should().Be("feature-42");
        passedFeature.Source.Should().Be("source-42");
        passedFeature.SourceLayer.Should().Be("layer-42");
    }

    [Fact]
    public async Task GetFilter_Should_Invoke_GetFilter_With_MapId_And_LayerId_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<string>("getFilter", "test-map", "layer-1").SetResult("[\"==\",\"class\",\"river\"]");

        var result = await cut.Instance.GetFilter("layer-1");

        result.Should().Be("[\"==\",\"class\",\"river\"]");
        var invocation = JSInterop.VerifyInvoke("getFilter");
        invocation.Arguments.Should().HaveCount(2);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be("layer-1");
    }

    [Fact]
    public async Task GetGlyphs_Should_Invoke_GetGlyphs_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<string>("getGlyphs", "test-map").SetResult("https://example.com/glyphs/{fontstack}/{range}.pbf");

        var result = await cut.Instance.GetGlyphs();

        result.Should().Be("https://example.com/glyphs/{fontstack}/{range}.pbf");
        JSInterop.VerifyInvoke("getGlyphs").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetImage_Should_Invoke_GetImage_With_MapId_And_Id_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<string>("getImage", "test-map", "image-1").SetResult("data:image/png;base64,abc");

        var result = await cut.Instance.GetImage("image-1");

        result.Should().Be("data:image/png;base64,abc");
        var invocation = JSInterop.VerifyInvoke("getImage");
        invocation.Arguments.Should().HaveCount(2);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be("image-1");
    }

    [Fact]
    public async Task GetLayer_Should_Invoke_GetLayer_With_MapId_And_Id_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        object layer = new { id = "layer-1", type = "circle" };
        JSInterop.Setup<object>("getLayer", "test-map", "layer-1").SetResult(layer);

        var result = await cut.Instance.GetLayer("layer-1");

        result.Should().BeSameAs(layer);
        var invocation = JSInterop.VerifyInvoke("getLayer");
        invocation.Arguments.Should().HaveCount(2);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be("layer-1");
    }

    [Fact]
    public async Task HasLayer_Should_Invoke_HasLayer_With_MapId_And_Id_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<bool>("hasLayer", "test-map", "layer-1").SetResult(true);

        var result = await cut.Instance.HasLayer("layer-1");

        result.Should().BeTrue();
        var invocation = JSInterop.VerifyInvoke("hasLayer");
        invocation.Arguments.Should().HaveCount(2);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be("layer-1");
    }

    [Fact]
    public async Task GetLayersOrder_Should_Invoke_GetLayersOrder_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var layerIds = new[] { "layer-a", "layer-b", "layer-c" };
        JSInterop.Setup<string[]>("getLayersOrder", "test-map").SetResult(layerIds);

        var result = await cut.Instance.GetLayersOrder();

        result.Should().BeSameAs(layerIds);
        JSInterop.VerifyInvoke("getLayersOrder").Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }
}
