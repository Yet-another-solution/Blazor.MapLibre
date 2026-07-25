import { test, expect } from '@playwright/test';
import { expectCanvasHasRenderedContent, trackPageErrors } from './helpers';

test('load-geojson page adds a GeoJSON source and fill layer without error', async ({ page }) => {
  const errors = trackPageErrors(page);
  await page.goto('/examples/load-geojson');
  await expectCanvasHasRenderedContent(page);

  expect(errors).toEqual([]);
});
