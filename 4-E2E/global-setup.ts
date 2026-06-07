/**
 * Playwright Global Setup — Backend Warmup
 *
 * Azure App Service (Free/Basic tier) hibernates after inactivity.
 * Cold starts take 30-60 s. This setup pings the health endpoint
 * before any test runs so the first login attempt doesn't time out.
 */

import { chromium } from '@playwright/test';

const API_BASE_URL =
  process.env.API_BASE_URL ??
  process.env.VITE_API_URL?.replace('/api', '') ??
  null;

const FRONTEND_URL =
  process.env.BASE_URL ?? 'https://gentle-field-0c117480f.7.azurestaticapps.net';

const WARMUP_TIMEOUT_MS = 90_000; // 90 s max to wait for backend cold start
const POLL_INTERVAL_MS  =  3_000;

async function pingHealth(url: string): Promise<boolean> {
  try {
    const res = await fetch(url, { signal: AbortSignal.timeout(5_000) });
    return res.status < 500;
  } catch {
    return false;
  }
}

export default async function globalSetup() {
  const start = Date.now();

  // ── 1. Warm up backend via /health (if API_BASE_URL is set) ──────────
  if (API_BASE_URL) {
    const healthUrl = `${API_BASE_URL}/api/health`;
    console.log(`\n[warmup] Pinging backend: ${healthUrl}`);

    let ready = false;
    while (!ready && Date.now() - start < WARMUP_TIMEOUT_MS) {
      ready = await pingHealth(healthUrl);
      if (!ready) {
        process.stdout.write('.');
        await new Promise(r => setTimeout(r, POLL_INTERVAL_MS));
      }
    }
    if (ready) {
      console.log(`\n[warmup] Backend ready in ${((Date.now() - start) / 1000).toFixed(1)}s`);
    } else {
      console.warn(`\n[warmup] Backend did not respond within ${WARMUP_TIMEOUT_MS / 1000}s — tests may be slow`);
    }
    return;
  }

  // ── 2. Fallback: browser-based warmup via the login page ────────────
  // Navigating to /login triggers the SPA which immediately calls the API
  // (auth store check), waking up the backend without needing its URL.
  console.log(`\n[warmup] No API_BASE_URL set — using browser warmup on ${FRONTEND_URL}/login`);
  const browser = await chromium.launch({ headless: true });
  const page    = await browser.newPage();

  try {
    // Navigate to login (this wakes up the backend via the auth check)
    await page.goto(`${FRONTEND_URL}/login`, { waitUntil: 'networkidle', timeout: 30_000 });

    // Try an actual login to fully warm up the auth endpoint
    const email    = process.env.TEST_USER_EMAIL    ?? 'emailusuario@teste.com';
    const password = process.env.TEST_USER_PASSWORD ?? 'Password123';

    await page.fill('#username-or-email', email);
    await page.fill('#password', password);
    await page.click('button[type="submit"]');

    // Wait up to 60 s for the backend to respond (cold start window)
    const success = await page.waitForFunction(
      () => !window.location.pathname.includes('/login'),
      { timeout: 60_000 }
    ).then(() => true).catch(() => false);

    if (success) {
      console.log(`[warmup] Backend warmed up in ${((Date.now() - start) / 1000).toFixed(1)}s ✅`);
    } else {
      console.warn(`[warmup] Login warmup timed out — backend may still be cold. Tests will retry.`);
    }
  } finally {
    await browser.close();
  }
}
