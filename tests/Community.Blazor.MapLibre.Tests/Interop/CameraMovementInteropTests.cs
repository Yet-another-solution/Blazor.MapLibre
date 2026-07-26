using Bunit;
using Community.Blazor.MapLibre.Models;
using Community.Blazor.MapLibre.Models.Camera;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests.Interop;

/// <summary>
/// JS-interop contract tests for the camera-movement methods on <see cref="global::Community.Blazor.MapLibre.MapLibre"/>:
/// CalculateCameraOptionsFromCameraLngLatAltRotation, CalculateCameraOptionsFromTo, CameraForBounds, EaseTo,
/// FitBounds, SetMaxBounds, FitScreenCoordinates, FlyTo, JumpTo, PanBy, PanTo, Project, ResetNorth, ResetNorthPitch.
/// </summary>
/// <remarks>
/// Note on FitBounds/SetMaxBounds: the corresponding JS functions in MapLibre.razor.js reshape the
/// <see cref="LngLatBounds"/> into a nested <c>[[swLng, swLat], [neLng, neLat]]</c> array before calling into
/// MapLibre GL JS - but that reshaping happens INSIDE the JS function body, which bUnit's JSInterop mock never
/// executes. What crosses the C#-to-JS boundary (and what these contract tests can observe) is the
/// <see cref="LngLatBounds"/> object passed through unmodified by the C# method. So the assertions below verify
/// the bounds object is passed as-is, not the reshaped array - the reshaping itself is out of scope for a
/// contract test and would need to be covered by the separate Playwright/real-browser project.
/// </remarks>
public class CameraMovementInteropTests : MapLibreInteropTestBase
{
    [Fact]
    public async Task CalculateCameraOptionsFromCameraLngLatAltRotation_Should_Invoke_With_All_Args_In_Order()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var cameraLngLat = new LngLat(10, 20);
        var expected = new CameraOptions { Center = new LngLat(1, 2), Zoom = 5 };
        JSInterop.Setup<CameraOptions>("calculateCameraOptionsFromCameraLngLatAltRotation",
            "test-map", cameraLngLat, 100d, 45d, 30d, 5d).SetResult(expected);

        var result = await cut.Instance.CalculateCameraOptionsFromCameraLngLatAltRotation(
            cameraLngLat, 100, 45, 30, 5);

        result.Should().BeSameAs(expected);
        var invocation = JSInterop.VerifyInvoke("calculateCameraOptionsFromCameraLngLatAltRotation");
        invocation.Arguments.Should().HaveCount(6);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(cameraLngLat);
        invocation.Arguments[2].Should().Be(100d);
        invocation.Arguments[3].Should().Be(45d);
        invocation.Arguments[4].Should().Be(30d);
        invocation.Arguments[5].Should().Be(5d);
    }

    [Fact]
    public async Task CalculateCameraOptionsFromCameraLngLatAltRotation_With_No_Roll_Should_Pass_Null()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var cameraLngLat = new LngLat(10, 20);
        JSInterop.Setup<CameraOptions>("calculateCameraOptionsFromCameraLngLatAltRotation",
            "test-map", cameraLngLat, 100d, 45d, 30d, null).SetResult(new CameraOptions());

        await cut.Instance.CalculateCameraOptionsFromCameraLngLatAltRotation(
            cameraLngLat, 100, 45, 30);

        var invocation = JSInterop.VerifyInvoke("calculateCameraOptionsFromCameraLngLatAltRotation");
        invocation.Arguments[5].Should().BeNull();
    }

    [Fact]
    public async Task CalculateCameraOptionsFromTo_Should_Invoke_With_From_AltitudeFrom_To_AltitudeTo_In_Order()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var from = new LngLat(1, 2);
        var to = new LngLat(3, 4);
        var expected = new CameraOptions { Zoom = 7 };
        JSInterop.Setup<CameraOptions>("calculateCameraOptionsFromTo", "test-map", from, 10d, to, 20d)
            .SetResult(expected);

        var result = await cut.Instance.CalculateCameraOptionsFromTo(from, 10, to, 20);

        result.Should().BeSameAs(expected);
        var invocation = JSInterop.VerifyInvoke("calculateCameraOptionsFromTo");
        invocation.Arguments.Should().HaveCount(5);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(from);
        invocation.Arguments[2].Should().Be(10d);
        invocation.Arguments[3].Should().BeSameAs(to);
        invocation.Arguments[4].Should().Be(20d);
    }

    [Fact]
    public async Task CalculateCameraOptionsFromTo_With_No_AltitudeTo_Should_Pass_Null()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var from = new LngLat(1, 2);
        var to = new LngLat(3, 4);
        JSInterop.Setup<CameraOptions>("calculateCameraOptionsFromTo", "test-map", from, 10d, to, null)
            .SetResult(new CameraOptions());

        await cut.Instance.CalculateCameraOptionsFromTo(from, 10, to);

        var invocation = JSInterop.VerifyInvoke("calculateCameraOptionsFromTo");
        invocation.Arguments[4].Should().BeNull();
    }

    [Fact]
    public async Task CameraForBounds_Should_Invoke_With_MapId_Bounds_And_Options_And_Return_Value()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var bounds = new LngLatBounds { Southwest = new LngLat(-10, -20), Northeast = new LngLat(10, 20) };
        var options = new CameraForBoundsOptions { MaxZoom = 12 };
        var expected = new CenterZoomBearing { Center = new LngLat(0, 0), Zoom = 8, Bearing = 0 };
        JSInterop.Setup<CenterZoomBearing>("cameraForBounds", "test-map", bounds, options).SetResult(expected);

        var result = await cut.Instance.CameraForBounds(bounds, options);

        result.Should().BeSameAs(expected);
        var invocation = JSInterop.VerifyInvoke("cameraForBounds");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(bounds);
        invocation.Arguments[2].Should().BeSameAs(options);
    }

    [Fact]
    public async Task EaseTo_Should_Invoke_EaseTo_With_MapId_Options_And_EventData()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var options = new EaseToOptions { Zoom = 10, Duration = 400 };
        var eventData = new { source = "test" };

        await cut.Instance.EaseTo(options, eventData);

        var invocation = JSInterop.VerifyInvoke("easeTo");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(options);
        invocation.Arguments[2].Should().BeSameAs(eventData);
    }

    [Fact]
    public async Task FitBounds_Should_Invoke_FitBounds_With_MapId_Bounds_Options_And_EventData()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var bounds = new LngLatBounds { Southwest = new LngLat(-10, -20), Northeast = new LngLat(10, 20) };
        var options = new FitBoundOptions { MaxZoom = 15 };
        var eventData = new { source = "test" };

        await cut.Instance.FitBounds(bounds, options, eventData);

        // The [[sw],[ne]] reshaping happens inside the real "fitBounds" JS function body, which this mock
        // never executes - so what we observe here is the bounds object passed through as-is. See class remarks.
        var invocation = JSInterop.VerifyInvoke("fitBounds");
        invocation.Arguments.Should().HaveCount(4);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(bounds);
        invocation.Arguments[2].Should().BeSameAs(options);
        invocation.Arguments[3].Should().BeSameAs(eventData);
    }

    [Fact]
    public async Task FitBounds_With_No_Options_Or_EventData_Should_Pass_Null()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var bounds = new LngLatBounds { Southwest = new LngLat(-1, -2), Northeast = new LngLat(1, 2) };

        await cut.Instance.FitBounds(bounds);

        var invocation = JSInterop.VerifyInvoke("fitBounds");
        invocation.Arguments[2].Should().BeNull();
        invocation.Arguments[3].Should().BeNull();
    }

    [Fact]
    public async Task SetMaxBounds_Should_Invoke_SetMaxBounds_With_MapId_And_Bounds()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var bounds = new LngLatBounds { Southwest = new LngLat(-5, -6), Northeast = new LngLat(5, 6) };

        await cut.Instance.SetMaxBounds(bounds);

        // As with FitBounds, the JS-side reshaping into a nested array happens inside "setMaxBounds" itself
        // and is invisible to this contract test - only the direct C#-to-JS argument (the bounds object) is checked.
        var invocation = JSInterop.VerifyInvoke("setMaxBounds");
        invocation.Arguments.Should().HaveCount(2);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(bounds);
    }

    [Fact]
    public async Task FitScreenCoordinates_Should_Invoke_With_P0_P1_Bearing_Options_And_EventData_In_Order()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var p0 = new PointLike(220, 400);
        var p1 = new PointLike(500, 900);
        var options = new FitBoundOptions { Linear = true };
        var eventData = new { source = "test" };

        await cut.Instance.FitScreenCoordinates(p0, p1, 30, options, eventData);

        var invocation = JSInterop.VerifyInvoke("fitScreenCoordinates");
        invocation.Arguments.Should().HaveCount(6);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(p0);
        invocation.Arguments[2].Should().BeSameAs(p1);
        invocation.Arguments[3].Should().Be(30d);
        invocation.Arguments[4].Should().BeSameAs(options);
        invocation.Arguments[5].Should().BeSameAs(eventData);
    }

    [Fact]
    public async Task FlyTo_Should_Invoke_FlyTo_With_MapId_Options_And_EventData()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var options = new FlyToOptions { Center = new LngLat(0, 0), Zoom = 9 };
        var eventData = new { source = "test" };

        await cut.Instance.FlyTo(options, eventData);

        var invocation = JSInterop.VerifyInvoke("flyTo");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(options);
        invocation.Arguments[2].Should().BeSameAs(eventData);
    }

    [Fact]
    public async Task JumpTo_Should_Invoke_JumpTo_With_MapId_Options_And_EventData()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var options = new JumpToOptions { Center = new LngLat(1, 1), Zoom = 3 };
        var eventData = new { source = "test" };

        await cut.Instance.JumpTo(options, eventData);

        var invocation = JSInterop.VerifyInvoke("jumpTo");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(options);
        invocation.Arguments[2].Should().BeSameAs(eventData);
    }

    [Fact]
    public async Task PanBy_Should_Invoke_PanBy_With_MapId_Offset_Options_And_EventData()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var offset = new PointLike(50, -25);
        var options = new EaseToOptions { Duration = 250 };
        var eventData = new { source = "test" };

        await cut.Instance.PanBy(offset, options, eventData);

        var invocation = JSInterop.VerifyInvoke("panBy");
        invocation.Arguments.Should().HaveCount(4);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(offset);
        invocation.Arguments[2].Should().BeSameAs(options);
        invocation.Arguments[3].Should().BeSameAs(eventData);
    }

    [Fact]
    public async Task PanBy_With_No_Options_Or_EventData_Should_Pass_Null()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var offset = new PointLike(10, 10);

        await cut.Instance.PanBy(offset);

        var invocation = JSInterop.VerifyInvoke("panBy");
        invocation.Arguments[2].Should().BeNull();
        invocation.Arguments[3].Should().BeNull();
    }

    [Fact]
    public async Task PanTo_Should_Invoke_PanTo_With_MapId_LngLat_Options_And_EventData()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var lngLat = new LngLat(12, 34);
        var options = new EaseToOptions { Duration = 300 };
        var eventData = new { source = "test" };

        await cut.Instance.PanTo(lngLat, options, eventData);

        var invocation = JSInterop.VerifyInvoke("panTo");
        invocation.Arguments.Should().HaveCount(4);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(lngLat);
        invocation.Arguments[2].Should().BeSameAs(options);
        invocation.Arguments[3].Should().BeSameAs(eventData);
    }

    [Fact]
    public async Task Project_Should_Invoke_Project_With_MapId_And_LngLat_And_Convert_Array_Result_To_PointLike()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var lngLat = new LngLat(45, 90);
        JSInterop.Setup<double[]>("project", "test-map", lngLat).SetResult([123.4, 456.7]);

        var result = await cut.Instance.Project(lngLat);

        // Project post-processes the raw JS array result into a PointLike in C# - confirm that conversion.
        result.X.Should().Be(123.4);
        result.Y.Should().Be(456.7);
        var invocation = JSInterop.VerifyInvoke("project");
        invocation.Arguments.Should().HaveCount(2);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(lngLat);
    }

    [Fact]
    public async Task ResetNorth_Should_Invoke_ResetNorth_With_MapId_Options_And_EventData()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var options = new AnimationOptions { Duration = 200 };
        var eventData = new { source = "test" };

        await cut.Instance.ResetNorth(options, eventData);

        var invocation = JSInterop.VerifyInvoke("resetNorth");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(options);
        invocation.Arguments[2].Should().BeSameAs(eventData);
    }

    [Fact]
    public async Task ResetNorth_With_No_Args_Should_Pass_Null_Options_And_EventData()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        await cut.Instance.ResetNorth();

        var invocation = JSInterop.VerifyInvoke("resetNorth");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeNull();
        invocation.Arguments[2].Should().BeNull();
    }

    [Fact]
    public async Task ResetNorthPitch_Should_Invoke_ResetNorthPitch_With_MapId_Options_And_EventData()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var options = new AnimationOptions { Duration = 250 };
        var eventData = new { source = "test" };

        await cut.Instance.ResetNorthPitch(options, eventData);

        var invocation = JSInterop.VerifyInvoke("resetNorthPitch");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(options);
        invocation.Arguments[2].Should().BeSameAs(eventData);
    }

    [Fact]
    public async Task ResetNorthPitch_With_No_Args_Should_Pass_Null_Options_And_EventData()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        await cut.Instance.ResetNorthPitch();

        var invocation = JSInterop.VerifyInvoke("resetNorthPitch");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeNull();
        invocation.Arguments[2].Should().BeNull();
    }
}
