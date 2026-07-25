import { defineConfig, devices } from '@playwright/test';

const PORT = 5048;
const BASE_URL = `http://localhost:${PORT}`;

export default defineConfig({
  testDir: './tests',
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 0,
  reporter: 'list',
  use: {
    baseURL: BASE_URL,
    trace: 'on-first-retry',
  },
  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },
  ],
  webServer: {
    // Real browser against the real Examples app (Blazor WASM) and its real MapLibre GL JS -
    // this is what actually verifies the library works, as opposed to the bUnit contract tests
    // in Community.Blazor.MapLibre.Tests, which only check that the right JS function gets called.
    command: 'dotnet run --project ../../examples/Community.Blazor.MapLibre.Examples --urls ' + BASE_URL,
    url: BASE_URL,
    reuseExistingServer: !process.env.CI,
    timeout: 120_000,
    stdout: 'pipe',
    stderr: 'pipe',
  },
});
