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
  const html = buildSnapshotHtml({
    readme: {
      title: 'Raccord Architecture',
      description: 'A guided view.',
    },
    sections,
    stackName: 'Raccord',
    revision: '1234567890',
  });
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

test('bounds diagram width and keeps wide SVGs scrollable inside the article', () => {
  const html = buildSnapshotHtml({
    readme: {
      title: 'Raccord Architecture',
      description: 'A guided view.',
    },
    sections,
    stackName: 'Raccord',
    revision: '1234567890',
  });
  assert.match(html, /figure\.diagram[^}]*overflow-x:\s*auto/);
  assert.match(html, /figure\.diagram svg[^}]*max-width:\s*min\(100%,\s*72rem\)/);
  assert.match(html, /figure\.diagram[^}]*overscroll-behavior-x:\s*contain/);
});

test('embeds the supplied canonical tokens in its self-contained stylesheet', () => {
  const html = buildSnapshotHtml({
    readme: {
      title: 'Raccord Architecture',
      description: 'A guided view.',
    },
    sections,
    stackName: 'Raccord',
    revision: '1234567890',
    designTokens: ':root { --raccord-ink: #0b1020; }',
  });
  assert.match(html, /--raccord-ink: #0b1020/);
});
