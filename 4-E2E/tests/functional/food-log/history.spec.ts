import { test, expect } from '../../../fixtures/auth.fixture';

test.describe('Food Log History — functional', () => {
  test.beforeEach(async ({ authenticatedPage: page }) => {
    await page.goto('/food-log/history', { waitUntil: 'networkidle' });
    await page.waitForSelector('h1');
  });

  // ── Page load ────────────────────────────────────────────────────
  test('loads with "Points History" heading', async ({ authenticatedPage: page }) => {
    await expect(page.getByRole('heading', { name: /points history/i })).toBeVisible();
  });

  test('shows subtitle about daily point consumption', async ({ authenticatedPage: page }) => {
    await expect(page.getByText(/daily point consumption/i)).toBeVisible();
  });

  test('shows history list or "No history yet" empty state', async ({ authenticatedPage: page }) => {
    await page.waitForTimeout(2000);
    const hasList  = await page.getByText(/all logged days/i).isVisible().catch(() => false);
    const hasEmpty = await page.getByText(/no history yet/i).isVisible().catch(() => false);
    expect(hasList || hasEmpty, 'Expected "All logged days" header or "No history yet" text').toBe(true);
  });

  // ── SubNav navigation ────────────────────────────────────────────
  test('SubNav "Today" link navigates back to /food-log', async ({ authenticatedPage: page }) => {
    await page.getByRole('link', { name: /^today$/i }).click();
    await expect(page).toHaveURL(/\/food-log$/);
  });

  test('SubNav "History" link is active (stays on /food-log/history)', async ({ authenticatedPage: page }) => {
    await page.getByRole('link', { name: /^history$/i }).click();
    await expect(page).toHaveURL(/\/food-log\/history/);
  });
});
