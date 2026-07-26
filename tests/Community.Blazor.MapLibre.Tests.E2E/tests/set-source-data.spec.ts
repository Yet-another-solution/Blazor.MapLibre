import { test, expect } from '@playwright/test';
import { expectMapCanvasVisible, trackPageErrors } from './helpers';

test('set-source-data page updates GeoJSON source data on button click', async ({ page }) => {
  const errors = trackPageErrors(page);
  await page.goto('/examples/set-source-data');
  await expectMapCanvasVisible(page);

  await expect(page.getByText('Map loaded with initial data')).toBeVisible();
  await page.getByRole('button', { name: 'Update Source Data' }).click();
  await expect(page.getByText('Source data updated 1 time(s)')).toBeVisible();

  expect(errors).toEqual([]);
});
