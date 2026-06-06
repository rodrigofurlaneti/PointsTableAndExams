import { test, expect } from '@playwright/test';

/**
 * Non-functional: Performance
 * Thresholds based on Google Core Web Vitals (good tier):
 *   FCP  < 1800 ms
 *   LCP  < 2500 ms
 *   TTI  < 3800 ms  (approximated via domContentLoaded)
 */

interface PerfMetrics {
  fcp: number;
  lcp: number;
  domContentLoaded: number;
}

async function collectMetrics(page: import('@playwright/test').Page, url: string): Promise<PerfMetrics> {
  await page.goto(url, { waitUntil: 'networkidle' });

  const metrics = await page.evaluate((): PerfMetrics => {
    const nav = performance.getEntriesByType('navigation')[0] as PerformanceNavigationTiming;
    const fcpEntry = performance.getEntriesByName('first-contentful-paint')[0];
    const lcpEntries = performance.getEntriesByType('largest-contentful-paint');

    return {
      fcp: fcpEntry?.startTime ?? 0,
      lcp: lcpEntries.length > 0
        ? (lcpEntries[lcpEntries.length - 1] as PerformancePaintTiming).startTime
        : 0,
      domContentLoaded: nav.domContentLoadedEventEnd - nav.startTime,
    };
  });

  return metrics;
}

test.describe('Performance — Core Web Vitals', () => {
  test('login page: FCP < 1800ms and LCP < 2500ms', async ({ page }) => {
    const m = await collectMetrics(page, '/login');
    console.log('Login FCP:', m.fcp, 'LCP:', m.lcp, 'DCL:', m.domContentLoaded);
    expect(m.fcp, `FCP was ${m.fcp}ms (limit 1800ms)`).toBeLessThan(1800);
    if (m.lcp > 0) {
      expect(m.lcp, `LCP was ${m.lcp}ms (limit 2500ms)`).toBeLessThan(2500);
    }
  });

  test('register page: FCP < 1800ms', async ({ page }) => {
    const m = await collectMetrics(page, '/register');
    console.log('Register FCP:', m.fcp, 'DCL:', m.domContentLoaded);
    expect(m.fcp, `FCP was ${m.fcp}ms (limit 1800ms)`).toBeLessThan(1800);
  });

  test('domContentLoaded < 3000ms on public pages', async ({ page }) => {
    for (const route of ['/login', '/register']) {
      const m = await collectMetrics(page, route);
      expect(m.domContentLoaded, `DCL on ${route} was ${m.domContentLoaded}ms`).toBeLessThan(3000);
    }
  });

  test('no unhandled JS errors on login page', async ({ page }) => {
    const errors: string[] = [];
    page.on('pageerror', (err) => errors.push(err.message));
    await page.goto('/login', { waitUntil: 'networkidle' });
    expect(errors, `JS errors: ${errors.join(', ')}`).toHaveLength(0);
  });

  test('no unhandled JS errors on register page', async ({ page }) => {
    const errors: string[] = [];
    page.on('pageerror', (err) => errors.push(err.message));
    await page.goto('/register', { waitUntil: 'networkidle' });
    expect(errors, `JS errors: ${errors.join(', ')}`).toHaveLength(0);
  });
});
