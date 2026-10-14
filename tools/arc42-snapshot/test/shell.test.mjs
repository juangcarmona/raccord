import test from 'node:test';
import assert from 'node:assert/strict';
import { buildSnapshotHtml } from '../shell.mjs';

const sections = Array.from({ length: 12 }, (_, i) => ({
  number: String(i + 1).padStart(2, '0'),
  title: `Section ${i + 1}`,
  description: `Description ${i + 1}`,
  html: '',
}));

test('builds a semantic icon sidebar without numeric section labels', () => {
  const html = buildSnapshotHtml({ readme: { title: 'Raccord Architecture', description: 'A guided view.' }, sections, stackName: 'Raccord', revision: '1234567890' });
  assert.match(html, /data-category="quality"/);
  assert.match(html, /class="nav-icon"/);
  assert.match(html, /class="nav-code">GOAL/);
  assert.doesNotMatch(html, /class="num"/);
  assert.match(html, /mobile-nav-toggle/);
  assert.match(html, /nav-backdrop/);
  assert.match(html, /function closeMobileNav/);
  assert.match(html, /el\('div', 'section-cards'\)/);
  assert.match(html, /target\.title/);
  assert.doesNotMatch(html, /target\.number \+ ' · '/);
});
