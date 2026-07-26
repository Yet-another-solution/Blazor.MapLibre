import { test, expect } from '@playwright/test';
import { expectMapCanvasVisible, trackPageErrors } from './helpers';

test('add-listener page loads geojson, adds a layer, and registers a click listener', async ({ page }) => {
  const errors = trackPageErrors(page);
  await page.goto('/examples/add-listener');
  await expectMapCanvasVisible(page);

  // Registering the layer-scoped click listener (AddSource + AddLayer + OnClick) must not throw -
  // reliably clicking the exact polygon feature would require computing the map projection, which
  // is out of scope for this smoke test; the textarea being present and the page staying error-free
  // is what's verified here.
  await expect(page.locator('textarea')).toBeVisible();

  expect(errors).toEqual([]);
});
