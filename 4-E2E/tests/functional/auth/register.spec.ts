import { test, expect } from '@playwright/test';
import { RegisterPage } from '../../../pages/RegisterPage';

const validUser = () => ({
  fullName:    'Test User',
  email:       `testuser${Date.now()}@example.com`,
  phoneNumber: '11999999999',
  username:    `testuser${Date.now()}`,
  password:    'Test1234!',
  birthDate:   '1995-06-15',
  gender:      'M' as const,
});

test.describe('Register — functional', () => {
  let registerPage: RegisterPage;

  test.beforeEach(async ({ page }) => {
    registerPage = new RegisterPage(page);
    await registerPage.goto();
  });

  // ── Happy path ──────────────────────────────────────────────────
  test('registers a new user successfully and redirects', async () => {
    await registerPage.register(validUser());
    await registerPage.expectSuccess();
  });

  // ── Client-side validation ───────────────────────────────────────
  test('shows error when full name is too short', async ({ page }) => {
    const data = validUser();
    await registerPage.fill({ ...data, fullName: 'A' });
    await registerPage.submit();
    // Error renders in a <p role="alert"> next to the field
    await expect(page.locator('#full-name-error')).toBeVisible();
    await expect(page.locator('#full-name-error')).toContainText(/full name is required/i);
  });

  test('shows error for invalid email', async ({ page }) => {
    await registerPage.fill({ ...validUser(), email: 'notanemail' });
    await registerPage.submit();
    await expect(page.locator('#email-error')).toBeVisible();
    await expect(page.locator('#email-error')).toContainText(/invalid email/i);
  });

  test('shows error when username has uppercase letters', async ({ page }) => {
    await registerPage.fill({ ...validUser(), username: 'UserUpper' });
    await registerPage.submit();
    await expect(page.locator('#username-error')).toBeVisible();
    await expect(page.locator('#username-error')).toContainText(/lowercase/i);
  });

  test('shows error when username has special characters', async ({ page }) => {
    await registerPage.fill({ ...validUser(), username: 'user@name' });
    await registerPage.submit();
    await expect(page.locator('#username-error')).toBeVisible();
    await expect(page.locator('#username-error')).toContainText(/lowercase/i);
  });

  test('shows error when password has no uppercase letter', async ({ page }) => {
    await registerPage.fill({ ...validUser(), password: 'test1234!' });
    await registerPage.submit();
    await expect(page.locator('#password-error')).toBeVisible();
    await expect(page.locator('#password-error')).toContainText(/uppercase/i);
  });

  test('shows error when password has no digit', async ({ page }) => {
    await registerPage.fill({ ...validUser(), password: 'TestAbcd!' });
    await registerPage.submit();
    await expect(page.locator('#password-error')).toBeVisible();
    await expect(page.locator('#password-error')).toContainText(/digit/i);
  });

  test('shows error when password is too short', async ({ page }) => {
    await registerPage.fill({ ...validUser(), password: 'T1!' });
    await registerPage.submit();
    await expect(page.locator('#password-error')).toBeVisible();
    await expect(page.locator('#password-error')).toContainText(/min 8/i);
  });

  test('shows error when gender is not selected', async ({ page }) => {
    await registerPage.fill(validUser());
    // Force-reset gender to empty
    await page.locator('select').selectOption('');
    await registerPage.submit();
    // Zod enum error renders in a <p> next to the select
    await expect(page.locator('select + p, select ~ p').first()).toBeVisible();
  });

  // ── API-level duplicate email ────────────────────────────────────
  test('shows error when email is already registered', async () => {
    const existing = {
      ...validUser(),
      email:    process.env.TEST_USER_EMAIL    ?? 'emailusuario@teste.com',
      username: `dup${Date.now()}`,
    };
    await registerPage.register(existing);
    await registerPage.expectApiError(/.+/);
  });
});
