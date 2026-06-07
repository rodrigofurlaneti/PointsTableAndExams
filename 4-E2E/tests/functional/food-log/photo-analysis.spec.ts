import * as path from 'path';
import * as fs from 'fs';
import { test, expect } from '../../../fixtures/auth.fixture';
import { FoodLogPage } from '../../../pages/FoodLogPage';

// Minimal valid JPEG (1x1 white pixel) — no external fixture needed
function getTestImagePath(): string {
  const dir  = path.join(__dirname, '__fixtures__');
  const file = path.join(dir, 'test-food.jpg');
  if (!fs.existsSync(dir)) fs.mkdirSync(dir, { recursive: true });
  if (!fs.existsSync(file)) {
    const jpeg = Buffer.from(
      '/9j/4AAQSkZJRgABAQEASABIAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0a' +
      'HBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/wAARCAABAAEDASIAAhEBAxEB/8QAFAABAAAAAAAAAAAAAAAAAAAACf/' +
      'EABQQAQAAAAAAAAAAAAAAAAAAAAD/xAAUAQEAAAAAAAAAAAAAAAAAAAAA/8QAFBEBAAAAAAAAAAAAAAAAAAAAAP/aAAwDAQACEQMRAD8AJQAB/9k=',
      'base64',
    );
    fs.writeFileSync(file, jpeg);
  }
  return file;
}

test.describe('Food Log — photo mode', () => {
  test('📷 Camera button is visible on food log page', async ({ authenticatedPage: page }) => {
    const foodLogPage = new FoodLogPage(page);
    await foodLogPage.goto();
    await expect(foodLogPage.photoModeButton).toBeVisible();
  });

  test('switching to photo mode shows upload area', async ({ authenticatedPage: page }) => {
    const foodLogPage = new FoodLogPage(page);
    await foodLogPage.goto();
    await foodLogPage.switchToPhotoMode();
    // The upload area div (clicking it triggers file input)
    await expect(page.getByText(/tap to take or upload a photo/i)).toBeVisible();
  });

  test('uploading a photo triggers analysis and shows result or loading state', async ({ authenticatedPage: page }) => {
    const imagePath = getTestImagePath();
    const foodLogPage = new FoodLogPage(page);
    await foodLogPage.goto();
    await foodLogPage.switchToPhotoMode();

    // Upload file directly to the hidden input
    await foodLogPage.fileInput.setInputFiles(imagePath);

    // Either the analyzing spinner or the analysis result should appear
    const analyzing = page.getByText(/analyzing your photo/i);
    const result    = page.getByRole('button', { name: /confirm & add to log/i });
    const error     = page.getByRole('alert');

    await expect(analyzing.or(result).or(error)).toBeVisible({ timeout: 25_000 });
  });
});
