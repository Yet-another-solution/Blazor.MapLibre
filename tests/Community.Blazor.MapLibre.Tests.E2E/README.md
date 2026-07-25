# Community.Blazor.MapLibre.Tests.E2E

Playwright end-to-end tests that run `examples/Community.Blazor.MapLibre.Examples` (a real Blazor
WASM app) in a real Chromium browser against real MapLibre GL JS.

This is a different layer of testing than `tests/Community.Blazor.MapLibre.Tests`:

- `Community.Blazor.MapLibre.Tests` (xUnit + bUnit) verifies C# behavior in isolation - model
  serialization, and (via the `Interop/` folder) that each `MapLibre.razor.cs` method invokes the
  correct JS function with correctly-shaped arguments. It never runs real JavaScript.
- This project verifies the whole stack actually works together in a browser: that MapLibre GL JS
  accepts what the wrapper sends it, tiles render, markers/popups appear, etc.

## Running locally

```bash
npm install
npx playwright install chromium
npm test
```

`npm test` starts `dotnet run` for the Examples project automatically (see `playwright.config.ts`)
against a fixed port and tears it down after the run. Use `npm run test:ui` for Playwright's
interactive UI mode, or `npm run test:headed` to watch the browser.

## Scope

One smoke test per example page (`tests/*.spec.ts`), each asserting the page loads, the map canvas
actually renders (screenshot-based pixel-variance check, not WebGL `readPixels` - see the comment
in `tests/helpers.ts` for why), and no unexpected console/page errors occur. This deliberately
isn't 1:1 coverage of every `MapLibre.razor.cs` method in a browser - most of them share the exact
same JS interop mechanism already exercised here, so that would be redundant rather than more
correct.

Not currently wired into CI (`.github/workflows/`) - that's a separate decision left to the
maintainers, since it changes shared pipeline infrastructure.

## Known, already-diagnosed issues these tests work around

- **`index.html`'s dynamic `<base href>`** (set via `document.write` based on
  `window.location.hostname`) races against Chrome's preload scanner on any deep-linked
  `/examples/*` URL, producing 3 benign 404 console errors per page load before a second,
  correctly-based request succeeds. Filtered in `tests/helpers.ts` (`trackPageErrors`) rather than
  fixed here - it's a real bug in the Examples app worth fixing separately, not a library issue.
- **`mapbox-gl-draw.spec.ts`**: the vendored third-party `mapbox-gl-draw.js` sets
  `"line-dasharray": [2, 2]` as a bare array on two style layers, which MapLibre GL JS 5.12.0's
  stricter style validator rejects (it wants `["literal", [2, 2]]`). The control still loads, but
  those two layers' dash styling silently fails to apply. Documented and filtered in that spec file
  rather than patched in the vendored asset.
