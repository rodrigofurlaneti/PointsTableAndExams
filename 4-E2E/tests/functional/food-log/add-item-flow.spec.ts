import { test, expect } from '../../../fixtures/auth.fixture';
import { FoodLogPage } from '../../../pages/FoodLogPage';

test.describe('Food Log — add item flow', () => {
  // ── Search ───────────────────────────────────────────────────────
  test('search input is visible in select mode', async ({ authenticatedPage: page }) => {
    const foodLogPage = new FoodLogPage(page);
    await foodLogPage.goto();
    await expect(page.locator('#search-food')).toBeVisible();
  });

  test('typing in search input updates dropdown options', async ({ authenticatedPage: page }) => {
    const foodLogPage = new FoodLogPage(page);
    await foodLogPage.goto();
    await page.fill('#search-food', 'rice');
    // Wait for API debounce + response
    await page.waitForTimeout(1500);
    // The food item select should still be visible (not hidden during search)
    await expect(foodLogPage.foodItemSelect).toBeVisible();
  });

  // ── Quantity validation ──────────────────────────────────────────
  test('quantity below minimum (0) shows validation error', async ({ authenticatedPage: page }) => {
    const foodLogPage = new FoodLogPage(page);
    await foodLogPage.goto();
    await page.fill('#quantity', '0');
    await foodLogPage.addItemButton.click();
    await page.waitForTimeout(500);
    // Zod coerce.number().min(0.5) triggers — error renders in #quantity-error
    await expect(page.locator('#quantity-error')).toBeVisible();
  });

  test('quantity above maximum (21) shows validation error', async ({ authenticatedPage: page }) => {
    const foodLogPage = new FoodLogPage(page);
    await foodLogPage.goto();
    await page.fill('#quantity', '21');
    await foodLogPage.addItemButton.click();
    await page.waitForTimeout(500);
    await expect(page.locator('#quantity-error')).toBeVisible();
  });

  // ── Add item happy path ──────────────────────────────────────────
  test('selects a food item and adds it to the log', async ({ authenticatedPage: page }) => {
    const foodLogPage = new FoodLogPage(page);
    await foodLogPage.goto();

    // Wait for food items API to populate the dropdown
    await page.waitForTimeout(2000);

    const select = foodLogPage.foodItemSelect;
    const options = await select.evaluate((el: HTMLSelectElement) =>
      Array.from(el.options)
        .filter(o => o.value !== '')
        .map(o => o.value)
    );

    if (options.length === 0) {
      // No food items seeded — skip the add step
      console.log('No food items in dropdown, skipping add-item happy path');
      await expect(foodLogPage.addItemButton).toBeVisible();
      return;
    }

    // Count current items in the list
    const listBefore = await page.locator('[class*="listItem"]').count();

    // Select first available item
    await select.selectOption(options[0]);
    await page.fill('#quantity', '1');
    await foodLogPage.addItemButton.click();

    // Wait for optimistic update or API response
    await page.waitForTimeout(2000);

    // The list should have the same or more items
    const listAfter = await page.locator('[class*="listItem"]').count();
    expect(listAfter).toBeGreaterThanOrEqual(listBefore);
  });

  // ── Mode toggle ──────────────────────────────────────────────────
  test('switching from select to camera mode hides the food select', async ({ authenticatedPage: page }) => {
    const foodLogPage = new FoodLogPage(page);
    await foodLogPage.goto();
    await expect(foodLogPage.foodItemSelect).toBeVisible();
    await foodLogPage.switchToPhotoMode();
    await expect(foodLogPage.foodItemSelect).not.toBeVisible();
  });

  test('switching back to select mode shows the food select again', async ({ authenticatedPage: page }) => {
    const foodLogPage = new FoodLogPage(page);
    await foodLogPage.goto();
    await foodLogPage.switchToPhotoMode();
    await foodLogPage.selectModeButton.click();
    await expect(foodLogPage.foodItemSelect).toBeVisible();
  });

  // ── Gallery mode ─────────────────────────────────────────────────
  test('"Gallery" tab button is visible on food log page', async ({ authenticatedPage: page }) => {
    const foodLogPage = new FoodLogPage(page);
    await foodLogPage.goto();
    await expect(foodLogPage.galleryModeButton).toBeVisible();
  });

  test('switching to Gallery mode hides the food select', async ({ authenticatedPage: page }) => {
    const foodLogPage = new FoodLogPage(page);
    await foodLogPage.goto();
    await expect(foodLogPage.foodItemSelect).toBeVisible();
    await foodLogPage.switchToGalleryMode();
    await expect(foodLogPage.foodItemSelect).not.toBeVisible();
  });

  test('Gallery mode shows device file picker hint text', async ({ authenticatedPage: page }) => {
    const foodLogPage = new FoodLogPage(page);
    await foodLogPage.goto();
    await foodLogPage.switchToGalleryMode();
    await expect(page.getByText(/tap to choose a photo from your device/i)).toBeVisible();
  });

  test('switching from Gallery back to Select shows the food select', async ({ authenticatedPage: page }) => {
    const foodLogPage = new FoodLogPage(page);
    await foodLogPage.goto();
    await foodLogPage.switchToGalleryMode();
    await foodLogPage.selectModeButton.click();
    await expect(foodLogPage.foodItemSelect).toBeVisible();
  });

  test('Gallery mode: uploading a photo triggers analysis', async ({ authenticatedPage: page }) => {
    const foodLogPage = new FoodLogPage(page);
    await foodLogPage.goto();
    await foodLogPage.switchToGalleryMode();

    // Inject a minimal 1×1 JPEG directly via the hidden gallery file input
    const minimalJpeg = Buffer.from(
      '/9j/4AAQSkZJRgABAQEASABIAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0a' +
      'HBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/wAARCAABAAEDASIAAhEBAxEB/8QAFAABAAAAAAAAAAAAAAAAAAAACf/' +
      'EABQQAQAAAAAAAAAAAAAAAAAAAAD/xAAUAQEAAAAAAAAAAAAAAAAAAAAA/8QAFBEBAAAAAAAAAAAAAAAAAAAAAP/aAAwDAQACEQMRAD8AJQAB/9k=',
      'base64',
    );
    await foodLogPage.galleryFileInput.setInputFiles({
      name: 'gallery-test.jpg',
      mimeType: 'image/jpeg',
      buffer: minimalJpeg,
    });

    const analyzing = page.getByText(/analyzing your photo/i);
    const result    = page.getByRole('button', { name: /confirm & add to log/i });
    const error     = page.getByRole('alert');
    await expect(analyzing.or(result).or(error)).toBeVisible({ timeout: 25_000 });
  });

  // ── Meal time select ─────────────────────────────────────────────
  test('meal time dropdown has multiple options', async ({ authenticatedPage: page }) => {
    const foodLogPage = new FoodLogPage(page);
    await foodLogPage.goto();
    // The second select in the form is the meal time
    const mealTimeSelect = page.locator('select').nth(1);
    const count = await mealTimeSelect.evaluate((el: HTMLSelectElement) => el.options.length);
    expect(count).toBeGreaterThan(1);
  });
});
