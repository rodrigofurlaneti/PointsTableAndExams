import { test, expect } from '../../../fixtures/auth.fixture';

test.describe('Navigation — authenticated', () => {
  // All nav queries are scoped to nav[aria-label="Global navigation"] to avoid
  // strict-mode violations caused by the footer repeating the same links.

  // ── Global nav presence ──────────────────────────────────────────
  test('global nav is visible after login', async ({ authenticatedPage: page }) => {
    await expect(page.locator('nav[aria-label="Global navigation"]')).toBeVisible();
  });

  test('"Sign Out" button is visible in nav', async ({ authenticatedPage: page }) => {
    const nav = page.locator('nav[aria-label="Global navigation"]');
    await expect(nav.getByRole('button', { name: /sign out/i })).toBeVisible();
  });

  test('user name span is present in the nav (shows first name when profile has fullName)', async ({ authenticatedPage: page }) => {
    // GlobalNav renders user?.fullName?.split(' ')[0] in a <span style="color:rgba(255,255,255,0.6)">
    // An empty span has 0×0 px → toBeAttached() checks DOM presence without requiring visibility.
    const nav = page.locator('nav[aria-label="Global navigation"]');
    await expect(nav.locator('span[style*="rgba"]')).toBeAttached();
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
