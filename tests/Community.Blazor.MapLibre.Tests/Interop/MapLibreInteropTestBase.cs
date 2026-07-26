using Bunit;
using Microsoft.JSInterop;

namespace Community.Blazor.MapLibre.Tests.Interop;

/// <summary>
/// Shared harness for JS-interop contract tests: verifies a <see cref="MapLibre"/> method invokes the
/// correct JS function with correctly-shaped arguments. This does NOT verify MapLibre GL JS itself does
/// the right thing (there's no real browser here) - that's covered by the Playwright E2E project instead.
/// </summary>
/// <remarks>
/// bUnit's <see cref="TestContext.JSInterop"/> defaults to <see cref="JSRuntimeMode.Strict"/>, which throws
/// on any unconfigured invocation. Setting it to <see cref="JSRuntimeMode.Loose"/> makes unconfigured
/// <c>InvokeAsync&lt;IJSObjectReference&gt;</c> calls (i.e. the two module imports in
/// <c>OnAfterRenderAsync</c>, and <c>initializeMap</c>) return a loose-mode <see cref="IJSObjectReference"/>
/// that proxies back to the SAME <see cref="TestContext.JSInterop"/> instance - so every JS call the
/// component makes, however many "modules" it thinks it imported, ends up recorded in one place:
/// <c>JSInterop.Invocations</c>, verifiable via <c>JSInterop.VerifyInvoke("functionName")</c>.
/// </remarks>
public abstract class MapLibreInteropTestBase : TestContext
{
    protected MapLibreInteropTestBase()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    /// <summary>
    /// Renders a <see cref="MapLibre"/> component (running its <c>OnAfterRenderAsync</c> JS-interop setup)
    /// ready for interop contract tests against <see cref="TestContext.JSInterop"/>.
    /// </summary>
    protected IRenderedComponent<global::Community.Blazor.MapLibre.MapLibre> RenderMapLibre(
        Action<ComponentParameterCollectionBuilder<global::Community.Blazor.MapLibre.MapLibre>>? parameterBuilder = null) =>
        parameterBuilder is null
            ? RenderComponent<global::Community.Blazor.MapLibre.MapLibre>()
            : RenderComponent<global::Community.Blazor.MapLibre.MapLibre>(parameterBuilder);
}
