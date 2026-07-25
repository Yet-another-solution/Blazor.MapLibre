import { test, expect } from '@playwright/test';
import { expectCanvasHasRenderedContent, trackPageErrors } from './helpers';

test('render-globe page sets globe projection on style load without error', async ({ page }) => {
  const errors = trackPageErrors(page);
  await page.goto('/examples/render-globe');
  await expectCanvasHasRenderedContent(page);

  expect(errors).toEqual([]);
});
