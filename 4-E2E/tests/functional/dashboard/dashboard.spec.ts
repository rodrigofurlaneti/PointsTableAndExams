import { test, expect } from '../../../fixtures/auth.fixture';

test.describe('Dashboard — functional', () => {
  test.beforeEach(async ({ authenticatedPage: page }) => {
    await page.goto('/dashboard', { waitUntil: 'networkidle' });
    await page.waitForSelector('h1');
  });

  // ── Headings & copy ─────────────────────────────────────────────
  test('shows "Your Health Dashboard" heading', async ({ authenticatedPage: page }) => {
    await expect(page.getByRole('heading', { name: /your health dashboard/i })).toBeVisible();
  });

  test('shows personalised greeting with user first name', async ({ authenticatedPage: page }) => {
    await expect(page.getByText(/good day,/i)).toBeVisible();
  });

  test('shows tagline about tracking food and exams', async ({ authenticatedPage: page }) => {
    await expect(page.getByText(/track what you eat/i)).toBeVisible();
  });

  // ── Stats cards ─────────────────────────────────────────────────
  test("shows \"Today's Points\" stat card", async ({ authenticatedPage: page }) => {
    await expect(page.getByText("Today's Points")).toBeVisible();
  });

  test('shows "Food items today" stat card', async ({ authenticatedPage: page }) => {
    await expect(page.getByText('Food items today')).toBeVisible();
  });

  test('shows "Pending exams" stat card', async ({ authenticatedPage: page }) => {
    await expect(page.getByText('Pending exams')).toBeVisible();
  });

  // ── Quick actions ────────────────────────────────────────────────
  test('shows "What would you like to do?" section', async ({ authenticatedPage: page }) => {
    await expect(page.getByText(/what would you like to do/i)).toBeVisible();
  });

  test('"Open Food Log" card is visible', async ({ authenticatedPage: page }) => {
    await expect(page.getByRole('button', { name: /open food log/i })).toBeVisible();
  });

  test('"Open Food Log" navigates to /food-log', async ({ authenticatedPage: page }) => {
    await page.getByRole('button', { name: /open food log/i }).click();
    await expect(page).toHaveURL(/\/food-log/);
  });

  test('"Go to Exams" navigates to /exams', async ({ authenticatedPage: page }) => {
    await page.getByRole('button', { name: /go to exams/i }).click();
    await expect(page).toHaveURL(/\/exams/);
  });

  test('"View History" navigates to /food-log/history', async ({ authenticatedPage: page }) => {
    await page.getByRole('button', { name: /view history/i }).click();
    await expect(page).toHaveURL(/\/food-log\/history/);
  });
});
