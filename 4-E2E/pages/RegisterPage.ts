import { type Page, type Locator, expect } from '@playwright/test';

export interface RegisterData {
  fullName: string;
  email: string;
  phoneNumber: string;
  username: string;
  password: string;
  birthDate: string; // YYYY-MM-DD
  gender: 'F' | 'M' | 'O';
}

export class RegisterPage {
  readonly page: Page;
  readonly fullNameInput: Locator;
  readonly emailInput: Locator;
  readonly phoneInput: Locator;
  readonly usernameInput: Locator;
  readonly passwordInput: Locator;
  readonly birthDateInput: Locator;
  readonly genderSelect: Locator;
  readonly submitButton: Locator;
  readonly errorBanner: Locator;

  constructor(page: Page) {
    this.page           = page;
    // Input component generates id = label.toLowerCase().replace(/\s+/g, '-')
    this.fullNameInput  = page.locator('#full-name');
    this.emailInput     = page.locator('#email');
    this.phoneInput     = page.locator('#phone-number');
    this.usernameInput  = page.locator('#username');
    this.passwordInput  = page.locator('#password');
    this.birthDateInput = page.locator('#birth-date');
    // Gender select has no htmlFor — select the only <select> on the page
    this.genderSelect   = page.locator('select');
    this.submitButton   = page.getByRole('button', { name: /create account/i });
    this.errorBanner    = page.getByRole('alert').first();
  }

  async goto() {
    await this.page.goto('/register', { waitUntil: 'networkidle' });
    // Wait for form to be rendered by React lazy loader
    await this.fullNameInput.waitFor({ state: 'visible' });
  }

  async fill(data: RegisterData) {
    await this.fullNameInput.fill(data.fullName);
    await this.emailInput.fill(data.email);
    await this.phoneInput.fill(data.phoneNumber);
    await this.usernameInput.fill(data.username);
    await this.passwordInput.fill(data.password);
    await this.birthDateInput.fill(data.birthDate);
    await this.genderSelect.selectOption(data.gender);
  }

  async submit() {
    await this.submitButton.click();
  }

  async register(data: RegisterData) {
    await this.fill(data);
    await this.submit();
  }

  async expectApiError(message: string | RegExp) {
    await expect(this.errorBanner).toContainText(message);
  }

  async expectSuccess() {
    // After registration the app redirects away from /register
    await expect(this.page).not.toHaveURL(/\/register/, { timeout: 15_000 });
  }
}
