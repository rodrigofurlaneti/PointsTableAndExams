import { defineConfig, devices } from '@playwright/test';

// Playwright automatically loads .env in the project root
const BASE_URL = process.env.BASE_URL ?? 'https://gentle-field-0c117480f.7.azurestaticapps.net';

export default defineConfig({
  testDir: './tests',
  globalSetup: './global-setup.ts',
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 1,   // 1 retry locally handles transient cold-start failures
  workers: process.env.CI ? 2 : undefined,

  reporter: [
    ['list'],
    ['html', { outputFolder: 'playwright-report', open: 'never' }],
    ['json', { outputFile: 'test-results/results.json' }],
  ],

  use: {
    baseURL: BASE_URL,
    trace: 'on-first-retry',
    screenshot: 'only-on-failure',
    video: 'on-first-retry',
    actionTimeout:     20_000,
    navigationTimeout: 45_000,
  },

  expect: {
    timeout: 10_000,
  },

  projects: [
    // ── Functional — Chromium only (faster feedback loop) ─────────
    {
      name: 'chromium',
      testMatch: '**/functional/**/*.spec.ts',
      use: { ...devices['Desktop Chrome'] },
    },
    {
      name: 'firefox',
      testMatch: '**/functional/**/*.spec.ts',
      use: { ...devices['Desktop Firefox'] },
    },
    {
      name: 'webkit',
      testMatch: '**/functional/**/*.spec.ts',
      use: { ...devices['Desktop Safari'] },
    },
    // ── Mobile ────────────────────────────────────────────────────
    {
      name: 'mobile-chrome',
      testMatch: '**/functional/**/*.spec.ts',
      use: { ...devices['Pixel 7'] },
    },
    // ── Non-functional — Chromium only ────────────────────────────
    {
      name: 'non-functional',
      testMatch: '**/non-functional/**/*.spec.ts',
      use: { ...devices['Desktop Chrome'] },
    },
  ],

  outputDir: 'test-results',
});
