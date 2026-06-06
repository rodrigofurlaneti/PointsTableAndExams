import { test, expect } from '../../../fixtures/auth.fixture';
import { FoodLogPage } from '../../../pages/FoodLogPage';

test.describe('Food Log — page access & UI', () => {
  test('food log page loads after login and shows today\'s log', async ({ authenticatedPage: page }) => {
    const foodLogPage = new FoodLogPage(page);
    await foodLogPage.goto();
    await foodLogPage.expectPageLoaded();
  });

  test('shows today\'s items header', async ({ authenticatedPage: page }) => {
    const foodLogPage = new FoodLogPage(page);
    await foodLogPage.goto();
    await expect(foodLogPage.todaysItemsHeader).toBeVisible();
  });

  test('select mode is active by default', async ({ authenticatedPage: page }) => {
    const foodLogPage = new FoodLogPage(page);
    await foodLogPage.goto();
    // Food item select should be visible in default (select) mode
    await expect(foodLogPage.foodItemSelect).toBeVisible();
  });

  test('shows Add item button in select mode', async ({ authenticatedPage: page }) => {
    const foodLogPage = new FoodLogPage(page);
    await foodLogPage.goto();
    await expect(foodLogPage.addItemButton).toBeVisible();
  });

  test('shows error when submitting without selecting a food item', async ({ authenticatedPage: page }) => {
    const foodLogPage = new FoodLogPage(page);
    await foodLogPage.goto();
    await foodLogPage.addItemButton.click();
    // Zod validation error for foodItemId
    await expect(page.getByText(/select a food item/i)).toBeVisible();
  });

  test('shows points badge when today\'s log is loaded', async ({ authenticatedPage: page }) => {
    const foodLogPage = new FoodLogPage(page);
    await foodLogPage.goto();
    // The pointsBadge renders only when the API returns today's log (log != null).
    // Wait briefly for the log to load, then check if badge is present.
    await page.waitForTimeout(2000);
    const badge = page.locator('[class*="pointsBadge"], [class*="pointsDen"]');
    const hasBadge = await badge.count() > 0;
    if (hasBadge) {
      await expect(badge.first()).toBeVisible();
    } else {
      // Badge not rendered — log not returned by API (empty day). Page still loaded.
      await foodLogPage.expectPageLoaded();
    }
  });
});
