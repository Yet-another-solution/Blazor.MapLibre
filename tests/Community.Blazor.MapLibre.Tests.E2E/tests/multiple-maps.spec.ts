import { test, expect } from '@playwright/test';
import { expectMapCanvasVisible, trackPageErrors } from './helpers';

test('multiple-maps page renders additional map instances on demand', async ({ page }) => {
  const errors = trackPageErrors(page);
  await page.goto('/examples/multiple-maps');
  await expectMapCanvasVisible(page, 0);
  await expect(page.locator('canvas.maplibregl-canvas')).toHaveCount(1);

  await page.getByRole('button', { name: '+' }).click();
  await expectMapCanvasVisible(page, 1);
  await expect(page.locator('canvas.maplibregl-canvas')).toHaveCount(2);

  expect(errors).toEqual([]);
});
