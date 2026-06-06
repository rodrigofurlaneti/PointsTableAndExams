import { test, expect } from '@playwright/test';

// Routes that actually exist in AppRouter and are behind ProtectedRoute
const PROTECTED_ROUTES = [
  '/food-log',
  '/dashboard',
  '/exams',
  '/food-log/history',
];

test.describe('Protected routes — unauthenticated', () => {
  for (const route of PROTECTED_ROUTES) {
    test(`redirects ${route} to /login when not authenticated`, async ({ page }) => {
      await page.goto(route, { waitUntil: 'networkidle' });
      await expect(page).toHaveURL(/\/login/, { timeout: 10_000 });
    });
  }

  test('home / redirects unauthenticated user to login', async ({ page }) => {
    await page.goto('/', { waitUntil: 'networkidle' });
    await expect(page).toHaveURL(/\/login/);
  });
});

test.describe('Public routes — always accessible', () => {
  test('/login page loads and shows the form', async ({ page }) => {
    await page.goto('/login', { waitUntil: 'networkidle' });
    await expect(page.locator('#username-or-email')).toBeVisible();
    await expect(page.locator('#password')).toBeVisible();
  });

  test('/register page loads and shows the form', async ({ page }) => {
    await page.goto('/register', { waitUntil: 'networkidle' });
    await expect(page.locator('#full-name')).toBeVisible();
  });
});
