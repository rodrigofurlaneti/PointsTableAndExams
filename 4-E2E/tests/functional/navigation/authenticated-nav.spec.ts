import { test, expect } from '../../../fixtures/auth.fixture';

test.describe('Navigation — authenticated', () => {
  // Scope all nav queries to the global nav to avoid strict-mode violations
  // caused by the footer repeating the same links.

  // ── Global nav presence ──────────────────────────────────────────
  test('global nav is visible after login', async ({ authenticatedPage: page }) => {
    await expect(page.locator('nav[aria-label="Global navigation"]')).toBeVisible();
  });

  test('"Sign Out" button is visible in nav', async ({ authenticatedPage: page }) => {
    const nav = page.locator('nav[aria-label="Global navigation"]');
    await expect(nav.getByRole('button', { name: /sign out/i })).toBeVisible();
  });

  test('user first name span is rendered in the nav bar', async ({ authenticatedPage: page }) => {
    // GlobalNav renders user?.fullName?.split(' ')[0] in the only <span> inside the nav.
    // Visibility check — actual text depends on the test account's profile data.
    const nav = page.locator('nav[aria-label="Global navigation"]');
    await expect(nav.locator('span').first()).toBeVisible();
  });

  // ── Nav links ────────────────────────────────────────────────────
  test('"Dashboard" nav link is visible', async ({ authenticatedPage: page }) => {
    const nav = page.locator('nav[aria-label="Global navigation"]');
    await expect(nav.getByRole('link', { name: 'Dashboard' })).toBeVisible();
  });

  test('"Food Log" nav link is visible', async ({ authenticatedPage: page }) => {
    const nav = page.locator('nav[aria-label="Global navigation"]');
    await expect(nav.getByRole('link', { name: 'Food Log' })).toBeVisible();
  });

  test('"Exams" nav link is visible', async ({ authenticatedPage: page }) => {
    const nav = page.locator('nav[aria-label="Global navigation"]');
    await expect(nav.getByRole('link', { name: 'Exams' })).toBeVisible();
  });

  // ── Nav link navigation ──────────────────────────────────────────
  test('"Dashboard" nav link navigates to /dashboard', async ({ authenticatedPage: page }) => {
    const nav = page.locator('nav[aria-label="Global navigation"]');
    await page.goto('/food-log', { waitUntil: 'networkidle' });
    await nav.getByRole('link', { name: 'Dashboard' }).click();
    await expect(page).toHaveURL(/\/(dashboard|$)/);
  });

  test('"Food Log" nav link navigates to /food-log', async ({ authenticatedPage: page }) => {
    const nav = page.locator('nav[aria-label="Global navigation"]');
    await page.goto('/dashboard', { waitUntil: 'networkidle' });
    await nav.getByRole('link', { name: 'Food Log' }).click();
    await expect(page).toHaveURL(/\/food-log/);
  });

  test('"Exams" nav link navigates to /exams', async ({ authenticatedPage: page }) => {
    const nav = page.locator('nav[aria-label="Global navigation"]');
    await page.goto('/dashboard', { waitUntil: 'networkidle' });
    await nav.getByRole('link', { name: 'Exams' }).click();
    await expect(page).toHaveURL(/\/exams/);
  });

  // ── Logo ─────────────────────────────────────────────────────────
  test('PT&E logo navigates to root (dashboard)', async ({ authenticatedPage: page }) => {
    await page.goto('/food-log', { waitUntil: 'networkidle' });
    await page.getByRole('link', { name: /pointstable home/i }).click();
    await expect(page).toHaveURL(/\/(dashboard|)$/);
  });
});
