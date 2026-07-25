using Bunit;
using Community.Blazor.MapLibre.Models;
using Community.Blazor.MapLibre.Models.Camera;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests.Interop;

/// <summary>
/// JS-interop contract tests for the miscellaneous camera/layer/render methods on
/// <see cref="global::Community.Blazor.MapLibre.MapLibre"/>: RotateTo, ZoomIn, ZoomOut, ZoomTo, SetBearing,
/// SetCenter, SetCenterClampedToGround, SetCenterElevation, SetZoom, MoveLayer, Redraw, Resize, Stop.
/// </summary>
public class ZoomRotateMiscInteropTests : MapLibreInteropTestBase
{
    [Fact]
    public async Task RotateTo_Should_Invoke_RotateTo_With_MapId_Bearing_Options_And_EventData()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var options = new EaseToOptions { Duration = 500 };
        var eventData = new { source = "test" };

        await cut.Instance.RotateTo(180, options, eventData);

        var invocation = JSInterop.VerifyInvoke("rotateTo");
        invocation.Arguments.Should().HaveCount(4);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be(180d);
        invocation.Arguments[2].Should().BeSameAs(options);
        invocation.Arguments[3].Should().BeSameAs(eventData);
    }

    [Fact]
    public async Task ZoomIn_Should_Invoke_ZoomIn_With_MapId_Options_And_EventData()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var options = new AnimationOptions { Duration = 300 };
        var eventData = new { source = "test" };

        await cut.Instance.ZoomIn(options, eventData);

        var invocation = JSInterop.VerifyInvoke("zoomIn");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(options);
        invocation.Arguments[2].Should().BeSameAs(eventData);
    }

    [Fact]
    public async Task ZoomOut_Should_Invoke_ZoomOut_With_MapId_Options_And_EventData()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var options = new AnimationOptions { Duration = 300 };
        var eventData = new { source = "test" };

        await cut.Instance.ZoomOut(options, eventData);

        var invocation = JSInterop.VerifyInvoke("zoomOut");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(options);
        invocation.Arguments[2].Should().BeSameAs(eventData);
    }

    [Fact]
    public async Task ZoomTo_Should_Invoke_ZoomTo_With_MapId_Zoom_Options_And_EventData()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var options = new EaseToOptions { Duration = 500 };
        var eventData = new { source = "test" };

        await cut.Instance.ZoomTo(12.5, options, eventData);

        var invocation = JSInterop.VerifyInvoke("zoomTo");
        invocation.Arguments.Should().HaveCount(4);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be(12.5);
        invocation.Arguments[2].Should().BeSameAs(options);
        invocation.Arguments[3].Should().BeSameAs(eventData);
    }

    [Fact]
    public async Task SetBearing_Should_Invoke_SetBearing_With_MapId_Bearing_And_EventData()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var eventData = new { source = "test" };

        await cut.Instance.SetBearing(90, eventData);

        var invocation = JSInterop.VerifyInvoke("setBearing");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be(90d);
        invocation.Arguments[2].Should().BeSameAs(eventData);
    }

    [Fact]
    public async Task SetCenter_Should_Invoke_SetCenter_With_MapId_Center_And_EventData()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var center = new LngLat(1.23, 4.56);
        var eventData = new { source = "test" };

        await cut.Instance.SetCenter(center, eventData);

        var invocation = JSInterop.VerifyInvoke("setCenter");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(center);
        invocation.Arguments[2].Should().BeSameAs(eventData);
    }

    [Fact]
    public async Task SetCenterClampedToGround_Should_Invoke_SetCenterClampedToGround_With_MapId_And_Flag()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        await cut.Instance.SetCenterClampedToGround(false);

        var invocation = JSInterop.VerifyInvoke("setCenterClampedToGround");
        invocation.Arguments.Should().HaveCount(2);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be(false);
    }

    [Fact]
    public async Task SetCenterElevation_Should_Invoke_SetCenterElevation_With_MapId_Elevation_And_EventData()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var eventData = new { source = "test" };

        await cut.Instance.SetCenterElevation(42.5, eventData);

        var invocation = JSInterop.VerifyInvoke("setCenterElevation");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be(42.5);
        invocation.Arguments[2].Should().BeSameAs(eventData);
    }

    [Fact]
    public async Task SetZoom_Should_Invoke_SetZoom_With_MapId_Zoom_And_EventData()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var eventData = new { source = "test" };

        await cut.Instance.SetZoom(8, eventData);

        var invocation = JSInterop.VerifyInvoke("setZoom");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be(8d);
        invocation.Arguments[2].Should().BeSameAs(eventData);
    }

    [Fact]
    public async Task MoveLayer_Should_Invoke_MoveLayer_With_MapId_Id_And_BeforeId()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        await cut.Instance.MoveLayer("layer-1", "layer-2");

        var invocation = JSInterop.VerifyInvoke("moveLayer");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be("layer-1");
        invocation.Arguments[2].Should().Be("layer-2");
    }

    [Fact]
    public async Task Redraw_Should_Invoke_Redraw_With_MapId()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        await cut.Instance.Redraw();

        var invocation = JSInterop.VerifyInvoke("redraw");
        invocation.Arguments.Should().HaveCount(1);
        invocation.Arguments[0].Should().Be("test-map");
    }

    [Fact]
    public async Task Resize_With_No_Args_Should_Invoke_Resize_With_Null_EventData_And_Default_ConstrainTransform_True()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        await cut.Instance.Resize();

        var invocation = JSInterop.VerifyInvoke("resize");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeNull();
        invocation.Arguments[2].Should().Be(true);
    }

    [Fact]
    public async Task Resize_With_Explicit_Args_Should_Invoke_Resize_With_EventData_And_ConstrainTransform_False()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var eventData = new { source = "test" };

        await cut.Instance.Resize(eventData, false);

        var invocation = JSInterop.VerifyInvoke("resize");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(eventData);
        invocation.Arguments[2].Should().Be(false);
    }

    [Fact]
    public async Task Stop_Should_Invoke_Stop_With_MapId()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        await cut.Instance.Stop();

        var invocation = JSInterop.VerifyInvoke("stop");
        invocation.Arguments.Should().HaveCount(1);
        invocation.Arguments[0].Should().Be("test-map");
    }
}
