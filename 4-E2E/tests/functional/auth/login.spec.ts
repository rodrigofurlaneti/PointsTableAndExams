import { test, expect } from '@playwright/test';
import { LoginPage } from '../../../pages/LoginPage';

const EMAIL    = process.env.TEST_USER_EMAIL    ?? 'emailusuario@teste.com';
const PASSWORD = process.env.TEST_USER_PASSWORD ?? 'Password123';

test.describe('Login — functional', () => {
  let loginPage: LoginPage;

  test.beforeEach(async ({ page }) => {
    loginPage = new LoginPage(page);
    await loginPage.goto();
  });

  // ── Happy path ──────────────────────────────────────────────────
  test('logs in with valid email and redirects to app', async () => {
    await loginPage.login(EMAIL, PASSWORD);
    await loginPage.expectRedirectedToDashboard();
  });

  test('logs in with username and redirects to app', async () => {
    // username = local part of email for our test user
    const username = 'emailusuarioteste';
    await loginPage.login(username, PASSWORD);
    await loginPage.expectRedirectedToDashboard();
  });

  // ── Error states ────────────────────────────────────────────────
  test('shows error for wrong password', async ({ page }) => {
    await loginPage.login(EMAIL, 'WrongPassword99!');
    // Wrong credentials must not redirect to the app
    await expect(page).not.toHaveURL(/\/(dashboard|food-log|exams)/, { timeout: 10_000 });
  });

  test('shows error for non-existent user', async ({ page }) => {
    await loginPage.login('nobody@nowhere.com', 'Test1234!');
    // Non-existent user must not redirect to the app
    await expect(page).not.toHaveURL(/\/(dashboard|food-log|exams)/, { timeout: 10_000 });
  });

  test('shows Zod error when fields are empty', async ({ page }) => {
    await page.locator('button[type="submit"]').click();
    // At least one aria-invalid or alert should appear
    const invalid = page.locator('[aria-invalid="true"]');
    await expect(invalid.first()).toBeVisible();
  });

  // ── UI / UX ─────────────────────────────────────────────────────
  test('has link to create account (register)', async ({ page }) => {
    // LoginPage link text is "Create one" — use exact match to avoid matching nav "Register" link
    await expect(page.getByRole('link', { name: 'Create one', exact: true })).toBeVisible();
  });

  test('password field is masked', async ({ page }) => {
    await expect(page.locator('#password')).toHaveAttribute('type', 'password');
  });
});
