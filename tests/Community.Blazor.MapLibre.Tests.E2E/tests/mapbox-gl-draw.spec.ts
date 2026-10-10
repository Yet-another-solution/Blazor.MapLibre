import { test, expect } from '@playwright/test';
import { expectMapCanvasVisible, trackPageErrors } from './helpers';

// BUG found by this test (not fixed here - it's in the vendored third-party mapbox-gl-draw.js
// asset under examples/Community.Blazor.MapLibre.Examples.MapboxGlPlugin, not this library):
// two of its style layers ("gl-draw-lines" cold/hot) set `"line-dasharray": [2, 2]` as a bare
// number array. MapLibre GL JS's style validator (since 5.12.0, still the case in 6.13.0) requires numeric literal arrays to be
// wrapped as `["literal", [2, 2]]` expressions and rejects the bare form with
// "Expression name must be a string, but found number instead." The control still loads (this is
// exactly why the plugin's own "Control added successfully" log is checked below, not just "no
// errors") but those two layers' dash styling silently fails to apply - a real, if cosmetic,
// compatibility break between the vendored plugin and the MapLibre version this library ships.
const KNOWN_LINE_DASHARRAY_VALIDATION_ERROR =
  /Expression name must be a string, but found number instead/;

test('mapbox-gl-draw page loads the plugin and adds a draw control without error', async ({ page }) => {
  const errors = trackPageErrors(page); // already filters the known base-href preload-race 404s
  const consoleMessages: string[] = [];
  page.on('console', (msg) => consoleMessages.push(msg.text()));

  await page.goto('/examples/mapbox-gl-draw');
  await expectMapCanvasVisible(page);

  // IMapLibrePlugin.Initialize + AddControl + AddFeature all succeeded only if this exact message
  // was logged (the plugin's catch block logs "Error adding control:" on any failure instead).
  await expect
    .poll(() => consoleMessages.some((m) => m.includes('Control added successfully')), {
      timeout: 15_000,
    })
    .toBe(true);

  const unexpectedErrors = errors.filter((e) => !KNOWN_LINE_DASHARRAY_VALIDATION_ERROR.test(e));
  expect(unexpectedErrors).toEqual([]);
});
