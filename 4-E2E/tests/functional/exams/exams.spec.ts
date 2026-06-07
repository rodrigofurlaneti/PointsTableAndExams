import { test, expect } from '../../../fixtures/auth.fixture';

test.describe('Exams — functional', () => {
  test.beforeEach(async ({ authenticatedPage: page }) => {
    await page.goto('/exams', { waitUntil: 'networkidle' });
    await page.waitForSelector('h1');
  });

  // ── Page load ────────────────────────────────────────────────────
  test('loads with "Exam Requests" heading', async ({ authenticatedPage: page }) => {
    await expect(page.getByRole('heading', { name: /exam requests/i })).toBeVisible();
  });

  test('shows pending count or "All exams completed" subtitle', async ({ authenticatedPage: page }) => {
    await page.waitForTimeout(2000);
    const subtitle = page.getByText(/pending exam|all exams completed/i);
    await expect(subtitle).toBeVisible();
  });

  test('shows "My requests" section heading', async ({ authenticatedPage: page }) => {
    await expect(page.getByRole('heading', { name: /my requests/i })).toBeVisible();
  });

  // ── New request button ───────────────────────────────────────────
  test('shows "+ New request" button', async ({ authenticatedPage: page }) => {
    await expect(page.getByRole('button', { name: /new request/i })).toBeVisible();
  });

  test('"+ New request" button navigates to /exams/requests', async ({ authenticatedPage: page }) => {
    await page.getByRole('button', { name: /new request/i }).click();
    await expect(page).toHaveURL(/\/exams\/requests/);
  });

  // ── SubNav ───────────────────────────────────────────────────────
  // ExamsPage (/exams) has SubNav: [My requests → /exams] [New request → /exams/requests]
  // ExamRequestsPage (/exams/requests) has NO SubNav — it is a standalone form page.
  test('SubNav "New request" link navigates to /exams/requests', async ({ authenticatedPage: page }) => {
    const subNav = page.getByRole('navigation', { name: /exams sub-navigation/i });
    await subNav.getByRole('link', { name: /new request/i }).click();
    await expect(page).toHaveURL(/\/exams\/requests/);
  });

  test('SubNav "My requests" link is active on /exams', async ({ authenticatedPage: page }) => {
    // Already on /exams from beforeEach — click "My requests" in the SubNav
    const subNav = page.getByRole('navigation', { name: /exams sub-navigation/i });
    await subNav.getByRole('link', { name: /my requests/i }).click();
    await expect(page).toHaveURL(/\/exams$/);
  });
});
