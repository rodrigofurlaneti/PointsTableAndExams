import { type Page, type Locator, expect } from '@playwright/test';

export class LoginPage {
  readonly page: Page;
  readonly usernameOrEmailInput: Locator;
  readonly passwordInput: Locator;
  readonly submitButton: Locator;
  readonly errorBanner: Locator;

  constructor(page: Page) {
    this.page = page;
    // Use CSS id selectors derived from label text (Input component: id = label.toLowerCase().replace(/\s+/g, '-'))
    this.usernameOrEmailInput = page.locator('#username-or-email');
    this.passwordInput        = page.locator('#password');
    this.submitButton         = page.locator('button[type="submit"]');
    this.errorBanner          = page.getByRole('alert');
  }

  async goto() {
    await this.page.goto('/login', { waitUntil: 'networkidle' });
    // Wait for form to be rendered by React
    await this.usernameOrEmailInput.waitFor({ state: 'visible' });
  }

  async login(usernameOrEmail: string, password: string) {
    await this.usernameOrEmailInput.fill(usernameOrEmail);
    await this.passwordInput.fill(password);
    await this.submitButton.click();
  }

  async expectError(message: string | RegExp) {
    await expect(this.errorBanner).toContainText(message);
  }

  async expectRedirectedToDashboard() {
    await expect(this.page).not.toHaveURL(/\/login/, { timeout: 15_000 });
  }
}
