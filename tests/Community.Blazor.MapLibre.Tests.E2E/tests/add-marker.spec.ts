import { test, expect } from '@playwright/test';
import { expectMapCanvasVisible, trackPageErrors } from './helpers';

test('add-marker page renders a marker on button click', async ({ page }) => {
  const errors = trackPageErrors(page);
  await page.goto('/examples/add-marker');
  await expectMapCanvasVisible(page);

  await expect(page.locator('.maplibregl-marker')).toHaveCount(0);
  await page.getByRole('button', { name: 'Add a random marker' }).click();
  await expect(page.locator('.maplibregl-marker')).toHaveCount(1);

  expect(errors).toEqual([]);
});
