import { test, expect } from '@playwright/test';
import { expectMapCanvasVisible, trackPageErrors } from './helpers';

test('add-popup page opens a popup on map click', async ({ page }) => {
  const errors = trackPageErrors(page);
  await page.goto('/examples/add-popup');
  const canvas = await expectMapCanvasVisible(page);

  // OnClick(null, ...) is only registered once the map's async "load" event fires (inside
  // OnLoad in AddPopup.razor); retry the click until the listener has actually attached.
  await expect(async () => {
    await canvas.click({ position: { x: 100, y: 100 } });
    await expect(page.locator('.maplibregl-popup')).toBeVisible({ timeout: 1000 });
  }).toPass({ timeout: 15_000 });
  await expect(page.locator('.maplibregl-popup')).toContainText('Map clicked');

  expect(errors).toEqual([]);
});
