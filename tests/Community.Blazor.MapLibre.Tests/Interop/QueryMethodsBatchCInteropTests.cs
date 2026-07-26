using Bunit;
using Community.Blazor.MapLibre.Models;
using Community.Blazor.MapLibre.Models.Control;
using Community.Blazor.MapLibre.Models.Sources;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests.Interop;

/// <summary>
/// JS-interop contract tests (batch C) for the query/getter/state-check methods on
/// <see cref="global::Community.Blazor.MapLibre.MapLibre"/>: verifies each method invokes the correct JS
/// function with correctly-shaped, correctly-ordered arguments, and that simple return values round-trip.
/// </summary>
public class QueryMethodsBatchCInteropTests : MapLibreInteropTestBase
{
    [Fact]
    public async Task GetSprite_Should_Invoke_GetSprite_With_MapId()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        await cut.Instance.GetSprite();

        var invocation = JSInterop.VerifyInvoke("getSprite");
        invocation.Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetStyle_Should_Invoke_GetStyle_With_MapId()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        await cut.Instance.GetStyle();

        var invocation = JSInterop.VerifyInvoke("getStyle");
        invocation.Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetTerrain_Should_Invoke_GetTerrain_With_MapId()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        await cut.Instance.GetTerrain();

        var invocation = JSInterop.VerifyInvoke("getTerrain");
        invocation.Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task GetVerticalFieldOfView_Should_Invoke_GetVerticalFieldOfView_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<double>("getVerticalFieldOfView", "test-map").SetResult(36.5);

        var result = await cut.Instance.GetVerticalFieldOfView();

        result.Should().Be(36.5);
    }

    [Fact]
    public async Task GetZoom_Should_Invoke_GetZoom_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<double>("getZoom", "test-map").SetResult(8.25);

        var result = await cut.Instance.GetZoom();

        result.Should().Be(8.25);
    }

    [Fact]
    public async Task HasControl_Should_Invoke_HasControl_With_MapId_And_Control_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        // Realistic usage: the control handle passed to HasControl is whatever AddControl returned.
        var control = (await cut.Instance.AddControl(ControlType.NavigationControl))!;
        JSInterop.Setup<bool>("hasControl", "test-map", control).SetResult(true);

        var result = await cut.Instance.HasControl(control);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task HasImage_Should_Invoke_HasImage_With_MapId_And_Id_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<bool>("hasImage", "test-map", "icon-1").SetResult(true);

        var result = await cut.Instance.HasImage("icon-1");

        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsMoving_Should_Invoke_IsMoving_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<bool>("isMoving", "test-map").SetResult(true);

        var result = await cut.Instance.IsMoving();

        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsRotating_Should_Invoke_IsRotating_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<bool>("isRotating", "test-map").SetResult(true);

        var result = await cut.Instance.IsRotating();

        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsSourceLoaded_Should_Invoke_IsSourceLoaded_With_MapId_And_Id_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<bool>("isSourceLoaded", "test-map", "source-1").SetResult(true);

        var result = await cut.Instance.IsSourceLoaded("source-1");

        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsStyleLoaded_Should_Invoke_IsStyleLoaded_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<bool>("isStyleLoaded", "test-map").SetResult(true);

        var result = await cut.Instance.IsStyleLoaded();

        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsZooming_Should_Invoke_IsZooming_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<bool>("isZooming", "test-map").SetResult(true);

        var result = await cut.Instance.IsZooming();

        result.Should().BeTrue();
    }

    [Fact]
    public async Task Listens_Should_Invoke_Listens_With_MapId_And_Type_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<bool>("listens", "test-map", "click").SetResult(true);

        var result = await cut.Instance.Listens("click");

        result.Should().BeTrue();
    }

    [Fact]
    public async Task ListImages_Should_Invoke_ListImages_With_MapId()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        await cut.Instance.ListImages();

        var invocation = JSInterop.VerifyInvoke("listImages");
        invocation.Arguments.Should().ContainSingle().Which.Should().Be("test-map");
    }

    [Fact]
    public async Task Loaded_Should_Invoke_Loaded_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        JSInterop.Setup<bool>("loaded", "test-map").SetResult(true);

        var result = await cut.Instance.Loaded();

        result.Should().BeTrue();
    }

    [Fact]
    public async Task LoadImage_Should_Invoke_LoadImage_With_MapId_And_Url()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        await cut.Instance.LoadImage("https://example.com/icon.png");

        var invocation = JSInterop.VerifyInvoke("loadImage");
        invocation.Arguments.Should().HaveCount(2);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be("https://example.com/icon.png");
    }

    [Fact]
    public async Task QueryTerrainElevation_Should_Invoke_QueryTerrainElevation_With_MapId_And_LngLat_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var lngLat = new LngLat(12.3, 45.6);
        JSInterop.Setup<double>("queryTerrainElevation", "test-map", lngLat).SetResult(123.4);

        var result = await cut.Instance.QueryTerrainElevation(lngLat);

        result.Should().Be(123.4);
    }

    [Fact]
    public async Task QueryRenderedFeatures_Should_Invoke_QueryRenderedFeatures_With_MapId_Query_And_Options()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var query = new { layers = new[] { "layer-1" } };
        var options = new { validate = false };

        await cut.Instance.QueryRenderedFeatures(query, options);

        var invocation = JSInterop.VerifyInvoke("queryRenderedFeatures");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(query);
        invocation.Arguments[2].Should().BeSameAs(options);
    }

    [Fact]
    public async Task QueryRenderedFeatures_With_No_Options_Should_Pass_Null()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var query = new { layers = new[] { "layer-1" } };

        await cut.Instance.QueryRenderedFeatures(query);

        var invocation = JSInterop.VerifyInvoke("queryRenderedFeatures");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[1].Should().BeSameAs(query);
        invocation.Arguments[2].Should().BeNull();
    }

    [Fact]
    public async Task QuerySourceFeatures_Should_Invoke_QuerySourceFeatures_With_MapId_SourceId_And_Parameters()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var options = new QuerySourceFeatureOptions { SourceLayer = "buildings" };

        await cut.Instance.QuerySourceFeatures("source-1", options);

        var invocation = JSInterop.VerifyInvoke("querySourceFeatures");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be("source-1");
        invocation.Arguments[2].Should().BeSameAs(options);
    }

    [Fact]
    public async Task QuerySourceFeatures_Should_Not_Swap_SourceId_And_Parameters()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var options = new QuerySourceFeatureOptions { SourceLayer = "source-1", Filter = "source-1" };

        await cut.Instance.QuerySourceFeatures("source-1", options);

        var invocation = JSInterop.VerifyInvoke("querySourceFeatures");
        invocation.Arguments[1].Should().BeOfType<string>();
        invocation.Arguments[1].Should().Be("source-1");
        invocation.Arguments[2].Should().BeSameAs(options);
    }
}
