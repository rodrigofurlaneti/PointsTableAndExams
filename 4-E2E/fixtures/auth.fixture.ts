import { test as base, type Page } from '@playwright/test';

const EMAIL    = process.env.TEST_USER_EMAIL    ?? 'emailusuario@teste.com';
const PASSWORD = process.env.TEST_USER_PASSWORD ?? 'Password123';

/**
 * Performs login via the UI and stores auth state in the browser context.
 */
async function loginViaUI(page: Page) {
  await page.goto('/login', { waitUntil: 'networkidle' });

  // Wait for the form to be rendered by React lazy loader
  const emailField    = page.locator('#username-or-email');
  const passwordField = page.locator('#password');
  await emailField.waitFor({ state: 'visible' });

  await emailField.fill(EMAIL);
  await passwordField.fill(PASSWORD);
  await page.locator('button[type="submit"]').click();

  // Wait until redirected away from /login.
  // Timeout is 60 s to tolerate Azure App Service cold starts (~30-60 s).
  await page.waitForURL((url) => !url.pathname.includes('/login'), { timeout: 60_000 });
}

type AuthFixtures = {
  authenticatedPage: Page;
};

export const test = base.extend<AuthFixtures>({
  authenticatedPage: async ({ page }, use) => {
    await loginViaUI(page);
    await use(page);
  },
});

export { expect } from '@playwright/test';
