import { type Page, type Locator, expect } from '@playwright/test';

export class FoodLogPage {
  readonly page: Page;
  readonly title: Locator;
  readonly selectModeButton: Locator;
  readonly photoModeButton: Locator;
  readonly foodItemSelect: Locator;
  readonly quantityInput: Locator;
  readonly addItemButton: Locator;
  readonly photoUploadArea: Locator;
  readonly fileInput: Locator;
  readonly analyzingIndicator: Locator;
  readonly analysisCard: Locator;
  readonly confirmPhotoButton: Locator;
  readonly todaysItemsHeader: Locator;

  constructor(page: Page) {
    this.page               = page;
    this.title              = page.getByRole('heading', { name: /daily food log/i });
    // Mode toggle buttons (text includes emoji)
    this.selectModeButton   = page.getByRole('button', { name: /select/i });
    this.photoModeButton    = page.getByRole('button', { name: /photo/i });
    // Form fields in select mode
    this.foodItemSelect     = page.locator('select').first();
    this.quantityInput      = page.locator('#quantity');
    this.addItemButton      = page.getByRole('button', { name: /^add item$/i });
    // Photo mode
    this.photoUploadArea    = page.locator('input[type="file"]');
    this.fileInput          = page.locator('input[type="file"]');
    this.analyzingIndicator = page.getByText(/analyzing your photo/i);
    this.analysisCard       = page.getByText(/confirm & add to log/i);
    this.confirmPhotoButton = page.getByRole('button', { name: /confirm & add to log/i });
    // List
    this.todaysItemsHeader  = page.getByText(/today's items/i);
  }

  async goto() {
    await this.page.goto('/food-log', { waitUntil: 'networkidle' });
    await this.title.waitFor({ state: 'visible' });
  }

  async expectPageLoaded() {
    await expect(this.title).toBeVisible();
    await expect(this.page).toHaveURL(/\/food-log/);
  }

  async switchToPhotoMode() {
    await this.photoModeButton.click();
  }
}
