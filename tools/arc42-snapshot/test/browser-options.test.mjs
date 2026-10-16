import test from 'node:test';
import assert from 'node:assert/strict';
import { getPuppeteerLaunchOptions } from '../browser-options.mjs';

test('disables Chromium sandbox only on GitHub-hosted action runners', () => {
  assert.deepEqual(getPuppeteerLaunchOptions({ GITHUB_ACTIONS: 'true' }), {
    args: ['--no-sandbox', '--disable-setuid-sandbox'],
  });
  assert.deepEqual(getPuppeteerLaunchOptions({}), { args: [] });
});
