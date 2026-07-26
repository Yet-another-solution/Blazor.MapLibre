using Bunit;
using Community.Blazor.MapLibre.Models;
using Community.Blazor.MapLibre.Models.Control;
using Community.Blazor.MapLibre.Models.Feature;
using Community.Blazor.MapLibre.Models.Image;
using Community.Blazor.MapLibre.Models.Sources;
using Community.Blazor.MapLibre.Models.Sprite;
using FluentAssertions;
using Xunit;

namespace Community.Blazor.MapLibre.Tests.Interop;

/// <summary>
/// Contract tests for the "add"/"remove" family of <see cref="global::Community.Blazor.MapLibre.MapLibre"/>
/// methods: confirms each invokes the correctly-named JS function with correctly-shaped/ordered arguments,
/// both on the direct path and when queued through a bulk transaction (<see cref="BulkTransaction"/>) and
/// flushed via "executeTransaction".
/// </summary>
public class AddRemoveInteropTests : MapLibreInteropTestBase
{
    private const string MapId = "test-map";

    private IRenderedComponent<global::Community.Blazor.MapLibre.MapLibre> RenderMap() =>
        RenderMapLibre(parameters => parameters.Add(p => p.MapId, MapId));

    // ---- AddControl ----------------------------------------------------

    [Fact]
    public async Task AddControl_Should_Invoke_AddControl_With_MapId_ControlTypeString_Position_And_Options()
    {
        var cut = RenderMap();
        var options = new { showCompass = false };

        var control = await cut.Instance.AddControl(ControlType.NavigationControl, ControlPosition.TopLeft, options);

        control.Should().NotBeNull();
        var invocation = JSInterop.VerifyInvoke("addControl");
        invocation.Arguments.Should().HaveCount(4);
        invocation.Arguments[0].Should().Be(MapId);
        invocation.Arguments[1].Should().Be("NavigationControl");
        invocation.Arguments[2].Should().Be(ControlPosition.TopLeft);
        invocation.Arguments[3].Should().BeSameAs(options);
    }

    [Fact]
    public async Task AddControl_During_BulkTransaction_Should_Defer_To_ExecuteTransaction_And_Return_Null()
    {
        var cut = RenderMap();
        cut.Instance.StartTransaction();

        var control = await cut.Instance.AddControl(ControlType.NavigationControl, ControlPosition.TopLeft);

        // No control exists yet while queued in a transaction, so there's nothing to return a handle to.
        control.Should().BeNull();
        JSInterop.VerifyNotInvoke("addControl");

        await cut.Instance.Commit();

        var invocation = JSInterop.VerifyInvoke("executeTransaction");
        invocation.Arguments[0].Should().Be(MapId);
        var transactions = (List<Transaction>)invocation.Arguments[1]!;
        var transaction = transactions.Should().ContainSingle(t => t.Event == "addControl").Subject;
        transaction.Data.Should().Equal("NavigationControl", ControlPosition.TopLeft, null);
    }

    // ---- AddImage --------------------------------------------------------

    [Fact]
    public async Task AddImage_Should_Invoke_AddImage_With_MapId_Id_Url_And_Options()
    {
        var cut = RenderMap();
        var options = new StyleImageMetadata { PixelRatio = 2 };

        await cut.Instance.AddImage("icon-1", "https://example.com/icon.png", options);

        var invocation = JSInterop.VerifyInvoke("addImage");
        invocation.Arguments.Should().HaveCount(4);
        invocation.Arguments[0].Should().Be(MapId);
        invocation.Arguments[1].Should().Be("icon-1");
        invocation.Arguments[2].Should().Be("https://example.com/icon.png");
        invocation.Arguments[3].Should().BeSameAs(options);
    }

    [Fact]
    public async Task AddImage_During_BulkTransaction_Should_Defer_To_ExecuteTransaction()
    {
        var cut = RenderMap();
        var options = new StyleImageMetadata { PixelRatio = 2 };
        cut.Instance.StartTransaction();

        await cut.Instance.AddImage("icon-1", "https://example.com/icon.png", options);

        JSInterop.VerifyNotInvoke("addImage");

        await cut.Instance.Commit();

        var invocation = JSInterop.VerifyInvoke("executeTransaction");
        var transactions = (List<Transaction>)invocation.Arguments[1]!;
        var transaction = transactions.Should().ContainSingle(t => t.Event == "addImage").Subject;
        transaction.Data.Should().Equal("icon-1", "https://example.com/icon.png", options);
    }

    // ---- AddSource -------------------------------------------------------

    [Fact]
    public async Task AddSource_Should_Invoke_AddSource_With_MapId_Id_And_Source()
    {
        var cut = RenderMap();
        var source = new GeoJsonSource { Data = new FeatureCollection() };

        await cut.Instance.AddSource("source-1", source);

        var invocation = JSInterop.VerifyInvoke("addSource");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be(MapId);
        invocation.Arguments[1].Should().Be("source-1");
        invocation.Arguments[2].Should().BeSameAs(source);
    }

    [Fact]
    public async Task AddSource_During_BulkTransaction_Should_Defer_To_ExecuteTransaction()
    {
        var cut = RenderMap();
        var source = new GeoJsonSource { Data = new FeatureCollection() };
        cut.Instance.StartTransaction();

        await cut.Instance.AddSource("source-1", source);

        JSInterop.VerifyNotInvoke("addSource");

        await cut.Instance.Commit();

        var invocation = JSInterop.VerifyInvoke("executeTransaction");
        var transactions = (List<Transaction>)invocation.Arguments[1]!;
        var transaction = transactions.Should().ContainSingle(t => t.Event == "addSource").Subject;
        transaction.Data.Should().Equal("source-1", source);
    }

    // ---- SetSourceData -----------------------------------------------------
    // SetSourceData unwraps the OneOf<IFeature, string> on GeoJsonSource.Data before calling JS: the JS
    // function receives the resolved feature/string directly, NOT the GeoJsonSource wrapper.

    [Fact]
    public async Task SetSourceData_With_Feature_Data_Should_Invoke_SetSourceData_With_MapId_Id_And_Feature()
    {
        var cut = RenderMap();
        var features = new FeatureCollection();
        var source = new GeoJsonSource { Data = features };

        await cut.Instance.SetSourceData("source-1", source);

        var invocation = JSInterop.VerifyInvoke("setSourceData");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be(MapId);
        invocation.Arguments[1].Should().Be("source-1");
        invocation.Arguments[2].Should().BeSameAs(features);
    }

    [Fact]
    public async Task SetSourceData_With_Url_Data_Should_Invoke_SetSourceData_With_MapId_Id_And_Url()
    {
        var cut = RenderMap();
        var source = new GeoJsonSource { Data = "https://example.com/data.geojson" };

        await cut.Instance.SetSourceData("source-1", source);

        var invocation = JSInterop.VerifyInvoke("setSourceData");
        invocation.Arguments[2].Should().Be("https://example.com/data.geojson");
    }

    [Fact]
    public async Task SetSourceData_During_BulkTransaction_Should_Defer_To_ExecuteTransaction()
    {
        var cut = RenderMap();
        var features = new FeatureCollection();
        var source = new GeoJsonSource { Data = features };
        cut.Instance.StartTransaction();

        await cut.Instance.SetSourceData("source-1", source);

        JSInterop.VerifyNotInvoke("setSourceData");

        await cut.Instance.Commit();

        var invocation = JSInterop.VerifyInvoke("executeTransaction");
        var transactions = (List<Transaction>)invocation.Arguments[1]!;
        var transaction = transactions.Should().ContainSingle(t => t.Event == "setSourceData").Subject;
        transaction.Data.Should().Equal("source-1", features);
    }

    // ---- AddSprite -------------------------------------------------------

    [Fact]
    public async Task AddSprite_Should_Invoke_AddSprite_With_MapId_Id_Url_And_Options()
    {
        var cut = RenderMap();
        var options = new StyleSetterOptions { Validate = false };

        await cut.Instance.AddSprite("sprite-1", "https://example.com/sprite.json", options);

        var invocation = JSInterop.VerifyInvoke("addSprite");
        invocation.Arguments.Should().HaveCount(4);
        invocation.Arguments[0].Should().Be(MapId);
        invocation.Arguments[1].Should().Be("sprite-1");
        invocation.Arguments[2].Should().Be("https://example.com/sprite.json");
        invocation.Arguments[3].Should().BeSameAs(options);
    }

    [Fact]
    public async Task AddSprite_During_BulkTransaction_Should_Defer_To_ExecuteTransaction()
    {
        var cut = RenderMap();
        var options = new StyleSetterOptions { Validate = false };
        cut.Instance.StartTransaction();

        await cut.Instance.AddSprite("sprite-1", "https://example.com/sprite.json", options);

        JSInterop.VerifyNotInvoke("addSprite");

        await cut.Instance.Commit();

        var invocation = JSInterop.VerifyInvoke("executeTransaction");
        var transactions = (List<Transaction>)invocation.Arguments[1]!;
        var transaction = transactions.Should().ContainSingle(t => t.Event == "addSprite").Subject;
        transaction.Data.Should().Equal("sprite-1", "https://example.com/sprite.json", options);
    }

    // ---- RemoveControl -----------------------------------------------------

    [Fact]
    public async Task RemoveControl_Should_Invoke_RemoveControl_With_MapId_And_Control()
    {
        var cut = RenderMap();
        // Realistic usage: the control handle passed to RemoveControl is whatever AddControl returned.
        var control = (await cut.Instance.AddControl(ControlType.NavigationControl))!;

        await cut.Instance.RemoveControl(control);

        var invocation = JSInterop.VerifyInvoke("removeControl");
        invocation.Arguments.Should().HaveCount(2);
        invocation.Arguments[0].Should().Be(MapId);
        invocation.Arguments[1].Should().BeSameAs(control);
    }

    [Fact]
    public async Task RemoveControl_During_BulkTransaction_Should_Defer_To_ExecuteTransaction()
    {
        var cut = RenderMap();
        var control = (await cut.Instance.AddControl(ControlType.NavigationControl))!;
        cut.Instance.StartTransaction();

        await cut.Instance.RemoveControl(control);

        JSInterop.VerifyNotInvoke("removeControl");

        await cut.Instance.Commit();

        var invocation = JSInterop.VerifyInvoke("executeTransaction");
        var transactions = (List<Transaction>)invocation.Arguments[1]!;
        var transaction = transactions.Should().ContainSingle(t => t.Event == "removeControl").Subject;
        transaction.Data.Should().Equal(control);
    }

    // ---- RemoveFeatureState ------------------------------------------------

    [Fact]
    public async Task RemoveFeatureState_Should_Invoke_RemoveFeatureState_With_MapId_Target_And_Key()
    {
        var cut = RenderMap();
        var target = new FeatureIdentifier { Source = "source-1", Id = "feature-1" };

        await cut.Instance.RemoveFeatureState(target, "hover");

        var invocation = JSInterop.VerifyInvoke("removeFeatureState");
        invocation.Arguments.Should().HaveCount(3);
        invocation.Arguments[0].Should().Be(MapId);
        invocation.Arguments[1].Should().BeSameAs(target);
        invocation.Arguments[2].Should().Be("hover");
    }

    [Fact]
    public async Task RemoveFeatureState_During_BulkTransaction_Should_Defer_To_ExecuteTransaction()
    {
        var cut = RenderMap();
        var target = new FeatureIdentifier { Source = "source-1", Id = "feature-1" };
        cut.Instance.StartTransaction();

        await cut.Instance.RemoveFeatureState(target, "hover");

        JSInterop.VerifyNotInvoke("removeFeatureState");

        await cut.Instance.Commit();

        var invocation = JSInterop.VerifyInvoke("executeTransaction");
        var transactions = (List<Transaction>)invocation.Arguments[1]!;
        var transaction = transactions.Should().ContainSingle(t => t.Event == "removeFeatureState").Subject;
        transaction.Data.Should().Equal(target, "hover");
    }

    // ---- RemoveImage -------------------------------------------------------

    [Fact]
    public async Task RemoveImage_Should_Invoke_RemoveImage_With_MapId_And_Id()
    {
        var cut = RenderMap();

        await cut.Instance.RemoveImage("icon-1");

        var invocation = JSInterop.VerifyInvoke("removeImage");
        invocation.Arguments.Should().HaveCount(2);
        invocation.Arguments[0].Should().Be(MapId);
        invocation.Arguments[1].Should().Be("icon-1");
    }

    [Fact]
    public async Task RemoveImage_During_BulkTransaction_Should_Defer_To_ExecuteTransaction()
    {
        var cut = RenderMap();
        cut.Instance.StartTransaction();

        await cut.Instance.RemoveImage("icon-1");

        JSInterop.VerifyNotInvoke("removeImage");

        await cut.Instance.Commit();

        var invocation = JSInterop.VerifyInvoke("executeTransaction");
        var transactions = (List<Transaction>)invocation.Arguments[1]!;
        var transaction = transactions.Should().ContainSingle(t => t.Event == "removeImage").Subject;
        transaction.Data.Should().Equal("icon-1");
    }

    // ---- RemoveLayer -------------------------------------------------------

    [Fact]
    public async Task RemoveLayer_Should_Invoke_RemoveLayer_With_MapId_And_Id()
    {
        var cut = RenderMap();

        await cut.Instance.RemoveLayer("layer-1");

        var invocation = JSInterop.VerifyInvoke("removeLayer");
        invocation.Arguments.Should().HaveCount(2);
        invocation.Arguments[0].Should().Be(MapId);
        invocation.Arguments[1].Should().Be("layer-1");
    }

    [Fact]
    public async Task RemoveLayer_During_BulkTransaction_Should_Defer_To_ExecuteTransaction()
    {
        var cut = RenderMap();
        cut.Instance.StartTransaction();

        await cut.Instance.RemoveLayer("layer-1");

        JSInterop.VerifyNotInvoke("removeLayer");

        await cut.Instance.Commit();

        var invocation = JSInterop.VerifyInvoke("executeTransaction");
        var transactions = (List<Transaction>)invocation.Arguments[1]!;
        var transaction = transactions.Should().ContainSingle(t => t.Event == "removeLayer").Subject;
        transaction.Data.Should().Equal("layer-1");
    }

    // ---- RemoveSource ------------------------------------------------------

    [Fact]
    public async Task RemoveSource_Should_Invoke_RemoveSource_With_MapId_And_Id()
    {
        var cut = RenderMap();

        await cut.Instance.RemoveSource("source-1");

        var invocation = JSInterop.VerifyInvoke("removeSource");
        invocation.Arguments.Should().HaveCount(2);
        invocation.Arguments[0].Should().Be(MapId);
        invocation.Arguments[1].Should().Be("source-1");
    }

    [Fact]
    public async Task RemoveSource_During_BulkTransaction_Should_Defer_To_ExecuteTransaction()
    {
        var cut = RenderMap();
        cut.Instance.StartTransaction();

        await cut.Instance.RemoveSource("source-1");

        JSInterop.VerifyNotInvoke("removeSource");

        await cut.Instance.Commit();

        var invocation = JSInterop.VerifyInvoke("executeTransaction");
        var transactions = (List<Transaction>)invocation.Arguments[1]!;
        var transaction = transactions.Should().ContainSingle(t => t.Event == "removeSource").Subject;
        transaction.Data.Should().Equal("source-1");
    }

    // ---- RemoveSprite ------------------------------------------------------

    [Fact]
    public async Task RemoveSprite_Should_Invoke_RemoveSprite_With_MapId_And_Id()
    {
        var cut = RenderMap();

        await cut.Instance.RemoveSprite("sprite-1");

        var invocation = JSInterop.VerifyInvoke("removeSprite");
        invocation.Arguments.Should().HaveCount(2);
        invocation.Arguments[0].Should().Be(MapId);
        invocation.Arguments[1].Should().Be("sprite-1");
    }

    [Fact]
    public async Task RemoveSprite_During_BulkTransaction_Should_Defer_To_ExecuteTransaction()
    {
        var cut = RenderMap();
        cut.Instance.StartTransaction();

        await cut.Instance.RemoveSprite("sprite-1");

        JSInterop.VerifyNotInvoke("removeSprite");

        await cut.Instance.Commit();

        var invocation = JSInterop.VerifyInvoke("executeTransaction");
        var transactions = (List<Transaction>)invocation.Arguments[1]!;
        var transaction = transactions.Should().ContainSingle(t => t.Event == "removeSprite").Subject;
        transaction.Data.Should().Equal("sprite-1");
    }
}
