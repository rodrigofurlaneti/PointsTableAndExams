import { test, expect } from '../../../fixtures/auth.fixture';

test.describe('Logout — functional', () => {
  test('logs out and redirects to login page', async ({ authenticatedPage: page }) => {
    // GlobalNav button text is "Sign Out"
    await page.getByRole('button', { name: /sign out/i }).click();
    await expect(page).toHaveURL(/\/login/, { timeout: 10_000 });
  });

  test('cannot access protected route after logout', async ({ authenticatedPage: page }) => {
    await page.getByRole('button', { name: /sign out/i }).click();
    await expect(page).toHaveURL(/\/login/, { timeout: 10_000 });

    await page.goto('/food-log');
    await expect(page).toHaveURL(/\/login/);
  });
});
