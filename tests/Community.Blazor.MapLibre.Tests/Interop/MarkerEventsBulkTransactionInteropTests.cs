using Bunit;
using Community.Blazor.MapLibre;
using Community.Blazor.MapLibre.Models;
using Community.Blazor.MapLibre.Models.Control;
using Community.Blazor.MapLibre.Models.Event;
using Community.Blazor.MapLibre.Models.Marker;
using FluentAssertions;
using Microsoft.JSInterop;
using Xunit;

namespace Community.Blazor.MapLibre.Tests.Interop;

/// <summary>
/// Contract tests for the Marker, Events, and Bulk Transaction regions of
/// <see cref="global::Community.Blazor.MapLibre.MapLibre"/>.
/// </summary>
public class MarkerEventsBulkTransactionInteropTests : MapLibreInteropTestBase
{
    #region Marker

    [Fact]
    public async Task AddMarker_Without_MarkerId_Should_Invoke_CreateMarker_With_MapId_GeneratedId_Options_Position_And_Return_Id()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var options = new MarkerOptions { Color = "#ff0000" };
        var position = new LngLat(12.34, 56.78);

        var id = await cut.Instance.AddMarker(options, position);

        // createMarker now also receives options.Element as a 5th argument (added in #167) so
        // Blazor can marshal a custom DOM element through to the JS side.
        var invocation = JSInterop.VerifyInvoke("createMarker");
        invocation.Arguments.Should().HaveCount(5);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be(id);
        invocation.Arguments[2].Should().BeSameAs(options);
        invocation.Arguments[3].Should().BeSameAs(position);
        invocation.Arguments[4].Should().Be(options.Element);
    }

    [Fact]
    public async Task AddMarker_With_Explicit_MarkerId_Should_Use_And_Return_That_Id()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var options = new MarkerOptions();
        var position = new LngLat(1, 2);
        var explicitId = Guid.NewGuid();

        var id = await cut.Instance.AddMarker(options, position, explicitId);

        id.Should().Be(explicitId);
        var invocation = JSInterop.VerifyInvoke("createMarker");
        invocation.Arguments[1].Should().Be(explicitId);
    }

    [Fact]
    public async Task RemoveMarker_Should_Invoke_RemoveMarker_With_MarkerId_Only()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var markerId = Guid.NewGuid();

        await cut.Instance.RemoveMarker(markerId);

        // Note: unlike most other JS calls, removeMarker does NOT receive MapId - the JS side
        // looks the marker up by its own id-keyed instance cache.
        var invocation = JSInterop.VerifyInvoke("removeMarker");
        invocation.Arguments.Should().HaveCount(1);
        invocation.Arguments[0].Should().Be(markerId);
    }

    [Fact]
    public async Task MoveMarker_Should_Invoke_MoveMarker_With_MarkerId_And_Position_But_No_MapId()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        var markerId = Guid.NewGuid();
        var position = new LngLat(3, 4);

        await cut.Instance.MoveMarker(markerId, position);

        var invocation = JSInterop.VerifyInvoke("moveMarker");
        invocation.Arguments.Should().HaveCount(2);
        invocation.Arguments[0].Should().Be(markerId);
        invocation.Arguments[1].Should().BeSameAs(position);
    }

    #endregion

    #region Events

    [Fact]
    public async Task AddListener_Should_Invoke_On_With_MapId_EventName_DotNetReference_And_Layer()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        Action<MapMouseEvent> handler = _ => { };

        await cut.Instance.AddListener("customevent", handler, "layer-1");

        var invocation = JSInterop.VerifyInvoke("on");
        invocation.Arguments.Should().HaveCount(4);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be("customevent");
        invocation.Arguments[2].Should().Be("layer-1");
        invocation.Arguments[3].Should().BeAssignableTo<DotNetObjectReference<CallbackHandler>>();
    }

    [Fact]
    public async Task AddListener_With_No_Layer_Should_Pass_Null_Layer()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        Action<MapMouseEvent> handler = _ => { };

        await cut.Instance.AddListener("customevent", handler);

        var invocation = JSInterop.VerifyInvoke("on");
        invocation.Arguments[2].Should().BeNull();
    }

    [Fact]
    public async Task AddAsyncListener_Should_Invoke_On_With_MapId_EventName_DotNetReference_And_Layer()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        Func<MapMouseEvent, Task> handler = _ => Task.CompletedTask;

        await cut.Instance.AddAsyncListener("customevent", handler, "layer-2");

        var invocation = JSInterop.VerifyInvoke("on");
        invocation.Arguments.Should().HaveCount(4);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be("customevent");
        invocation.Arguments[2].Should().Be("layer-2");
        invocation.Arguments[3].Should().BeAssignableTo<DotNetObjectReference<CallbackHandler>>();
    }

    [Fact]
    public async Task OnClick_Sync_Should_Invoke_On_With_Click_EventName_And_LayerId()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        Action<MapMouseEvent> handler = _ => { };

        await cut.Instance.OnClick("some-layer", handler);

        var invocation = JSInterop.VerifyInvoke("on");
        invocation.Arguments.Should().HaveCount(4);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be("click");
        invocation.Arguments[2].Should().Be("some-layer");
        invocation.Arguments[3].Should().BeAssignableTo<DotNetObjectReference<CallbackHandler>>();
    }

    [Fact]
    public async Task OnClick_Async_Should_Invoke_On_With_Click_EventName_And_LayerId()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));
        Func<MapMouseEvent, Task> handler = _ => Task.CompletedTask;

        await cut.Instance.OnClick("some-layer", handler);

        var invocation = JSInterop.VerifyInvoke("on");
        invocation.Arguments.Should().HaveCount(4);
        invocation.Arguments[0].Should().Be("test-map");
        invocation.Arguments[1].Should().Be("click");
        invocation.Arguments[2].Should().Be("some-layer");
        invocation.Arguments[3].Should().BeAssignableTo<DotNetObjectReference<CallbackHandler>>();
    }

    #endregion

    #region Bulk Transaction

    [Fact]
    public void StartTransaction_Called_Twice_Without_Commit_Should_Throw()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        cut.Instance.StartTransaction();

        var act = () => cut.Instance.StartTransaction();

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public async Task Commit_Without_StartTransaction_Should_Throw()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        var act = async () => await cut.Instance.Commit();

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Commit_Should_Invoke_ExecuteTransaction_With_MapId_And_Enqueued_Transactions()
    {
        var cut = RenderMapLibre(parameters => parameters.Add(p => p.MapId, "test-map"));

        cut.Instance.StartTransaction();

        // While a transaction is in progress, other API methods enqueue into the transaction
        // instead of calling JS directly - confirmed by the "addControl" invocation NOT appearing.
        await cut.Instance.AddControl(ControlType.NavigationControl);

        await cut.Instance.Commit();

        JSInterop.VerifyNotInvoke("addControl");
        var invocation = JSInterop.VerifyInvoke("executeTransaction");
        invocation.Arguments.Should().HaveCount(2);
        invocation.Arguments[0].Should().Be("test-map");
        var transactions = invocation.Arguments[1].Should().BeAssignableTo<List<Transaction>>().Subject;
        transactions.Should().HaveCount(1);
        transactions[0].Event.Should().Be("addControl");
    }

    #endregion
}
