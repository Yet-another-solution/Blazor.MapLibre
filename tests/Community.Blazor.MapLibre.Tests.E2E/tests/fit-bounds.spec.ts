import { test, expect } from '@playwright/test';
import { expectCanvasHasRenderedContent, trackPageErrors } from './helpers';

test('fit-bounds page fits the camera to the given bounds without error', async ({ page }) => {
  const errors = trackPageErrors(page);
  await page.goto('/examples/fit-bounds');
  await expectCanvasHasRenderedContent(page);

  expect(errors).toEqual([]);
});
