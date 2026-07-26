using Bunit;
using Community.Blazor.MapLibre.Models;
using Community.Blazor.MapLibre.Models.Feature;
using Community.Blazor.MapLibre.Models.Sprite;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests.Interop;

/// <summary>
/// JS-interop contract tests for style/feature/set-style related <see cref="global::Community.Blazor.MapLibre.MapLibre"/>
/// methods, plus <c>Remove</c> and component disposal. Verifies each method invokes the correct JS function with
/// correctly-shaped, correctly-ordered arguments - not that MapLibre GL JS itself behaves correctly.
/// </summary>
public class StyleFeatureSetInteropTests : MapLibreInteropTestBase
{
    [Fact]
    public async Task SetFeatureState_Should_Invoke_SetFeatureState_With_MapId_Feature_And_State()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var feature = new FeatureIdentifier { Source = "source-1", Id = "feature-1" };
        var state = new { hover = true };

        await cut.Instance.SetFeatureState(feature, state);

        var invocation = JSInterop.VerifyInvoke("setFeatureState");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(feature);
        invocation.Arguments[2].Should().BeSameAs(state);
    }

    [Fact]
    public async Task SetFilter_Should_Invoke_SetFilter_With_MapId_LayerId_Filter_And_Options()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var filter = new object[] { "==", "class", "street" };
        var options = new StyleSetterOptions { Validate = false };

        await cut.Instance.SetFilter("layer-1", filter, options);

        var invocation = JSInterop.VerifyInvoke("setFilter");
        invocation.Arguments.Should().HaveCount(4);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be("layer-1");
        invocation.Arguments[2].Should().BeSameAs(filter);
        invocation.Arguments[3].Should().BeSameAs(options);
    }

    [Fact]
    public async Task SetLayoutProperty_Should_Invoke_SetLayoutProperty_With_MapId_LayerId_Name_Value_And_Options()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var options = new StyleSetterOptions { Validate = true };

        await cut.Instance.SetLayoutProperty("layer-1", "visibility", "none", options);

        var invocation = JSInterop.VerifyInvoke("setLayoutProperty");
        invocation.Arguments.Should().HaveCount(5);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be("layer-1");
        invocation.Arguments[2].Should().Be("visibility");
        invocation.Arguments[3].Should().Be("none");
        invocation.Arguments[4].Should().BeSameAs(options);
    }

    [Fact]
    public async Task SetProjection_Should_Invoke_SetProjection_With_MapId_And_Projection()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var projection = new ProjectionSpecification { Type = "globe" };

        await cut.Instance.SetProjection(projection);

        var invocation = JSInterop.VerifyInvoke("setProjection");
        invocation.Arguments.Should().HaveCount(2);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(projection);
    }

    [Fact]
    public async Task SetStyle_Should_Invoke_SetStyle_With_MapId_Style_And_Options()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var style = "mapbox://styles/mapbox/streets-v11";
        var options = new { diff = false };

        await cut.Instance.SetStyle(style, options);

        var invocation = JSInterop.VerifyInvoke("setStyle");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(style);
        invocation.Arguments[2].Should().BeSameAs(options);
    }

    [Fact]
    public async Task SetStyle_With_No_Options_Should_Pass_Null()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var style = new { version = 8 };

        await cut.Instance.SetStyle(style);

        var invocation = JSInterop.VerifyInvoke("setStyle");
        invocation.Arguments[2].Should().BeNull();
    }

    [Fact]
    public async Task Unproject_Should_Invoke_Unproject_With_MapId_And_Point()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var point = new PointLike(100, 200);

        await cut.Instance.Unproject(point);

        var invocation = JSInterop.VerifyInvoke("unproject");
        invocation.Arguments.Should().HaveCount(2);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(point);
    }

    [Fact]
    public async Task UpdateImage_Should_Invoke_UpdateImage_With_MapId_Id_And_Image()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var image = new { url = "https://example.com/image.png" };

        await cut.Instance.UpdateImage("image-1", image);

        var invocation = JSInterop.VerifyInvoke("updateImage");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be("image-1");
        invocation.Arguments[2].Should().BeSameAs(image);
    }

    [Fact]
    public async Task CreatePopup_Should_Invoke_CreatePopup_With_MapId_Popup_And_Options()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var popup = new Popup { Content = "Hello", Coordinates = new LngLat { Longitude = 1, Latitude = 2 } };
        var options = new PopupOptions { ClassName = "my-popup" };

        await cut.Instance.CreatePopup(popup, options);

        var invocation = JSInterop.VerifyInvoke("createPopup");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().BeSameAs(popup);
        invocation.Arguments[2].Should().BeSameAs(options);
    }

    [Fact]
    public async Task Remove_Should_Invoke_Remove_With_MapId()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        await cut.Instance.Remove();

        var invocation = JSInterop.VerifyInvoke("remove");
        invocation.Arguments.Should().HaveCount(1);
        invocation.Arguments[0].Should().Be("test-map");
    }

    [Fact]
    public async Task DisposeAsync_Should_Invoke_Remove_With_MapId()
    {
        // DisposeAsync's body is: await Remove(); await _jsModule.DisposeAsync(); (swallowing JSDisconnectedException).
        // Calling the component's DisposeAsync directly exercises that path deterministically, rather than relying
        // on bUnit's TestContext.Dispose() to cascade into the component's async disposal.
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        await cut.Instance.DisposeAsync();

        var invocation = JSInterop.VerifyInvoke("remove");
        invocation.Arguments.Should().HaveCount(1);
        invocation.Arguments[0].Should().Be("test-map");
    }
}
