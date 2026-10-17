import test from 'node:test';
import assert from 'node:assert/strict';
import {
  getDiagramAccessibleTitle,
  MERMAID_CONFIG,
  normalizeMermaidSvg,
  renderMarkdown,
  stripDiagramAccessibilityTitle,
  stripDuplicateTitle,
  validateDiagramTitles,
} from '../render.mjs';

const fileToRoute = new Map();

test('removes a leading section H1 that repeats frontmatter title', () => {
  const source = '<!-- markdownlint-disable MD025 -->\n\n# 12. Glossary\n\nTerms follow.\n';
  const cleaned = stripDuplicateTitle(source, 'Glossary');
  assert.doesNotMatch(cleaned, /^\s*# 12\. Glossary/m);
  assert.match(cleaned, /Terms follow\./);
});

test('does not remove a non-matching H1', () => {
  const source = '# Different heading\n\nContent.\n';
  assert.equal(stripDuplicateTitle(source, 'Glossary'), source);
});

test('validates a descriptive accessibility title for each diagram', () => {
  const sources = [
    'flowchart LR\naccTitle: Invoice data flow\na --> b',
    'sequenceDiagram\naccTitle: Sign-in sequence\nUser->>Web: Sign in',
  ];
  assert.deepEqual(validateDiagramTitles(sources), []);
  assert.equal(getDiagramAccessibleTitle(sources[0]), 'Invoice data flow');
  assert.deepEqual(validateDiagramTitles([...sources, 'flowchart TB\na --> b']), [
    'Diagram 3 is missing an accTitle accessibility name',
  ]);
});

test('removes the source accessibility directive before Mermaid parsing', () => {
  assert.equal(
    stripDiagramAccessibilityTitle('mindmap\naccTitle: Quality tree\n  root((Quality))'),
    'mindmap\n  root((Quality))',
  );
});

test('uses a shared high-contrast base theme with strict SVG security', () => {
  assert.equal(MERMAID_CONFIG.securityLevel, 'strict');
  assert.equal(MERMAID_CONFIG.htmlLabels, false);
  assert.equal(MERMAID_CONFIG.theme, 'base');
  assert.equal(MERMAID_CONFIG.themeVariables.primaryColor, '#edf6fc');
  assert.equal(MERMAID_CONFIG.themeVariables.secondaryColor, '#e3f5ee');
  assert.equal(MERMAID_CONFIG.themeVariables.tertiaryColor, '#fff3d6');
});

test('renders the authored title as an accessible figure name', async () => {
  const source = 'flowchart LR\naccTitle: Current module boundary\na --> b';
  const svg = '<svg xmlns="http://www.w3.org/2000/svg" width="20" height="10" viewBox="0 0 20 10"><text>diagram</text></svg>';
  const { html } = await renderMarkdown(`\`\`\`mermaid\n${source}\n\`\`\``, {
    fileToRoute,
    svgs: [svg],
  });
  assert.match(html, /<figure class="diagram" role="img" aria-label="Current module boundary">/);
});

test('refuses to render an inaccessible diagram', async () => {
  await assert.rejects(
    renderMarkdown('```mermaid\nflowchart LR\na --> b\n```', { fileToRoute, svgs: ['<svg></svg>'] }),
    /Diagram 1 is missing an accTitle/,
  );
});

test('sanitizes SVG content and rewrites internal references to stable IDs', () => {
  const source = '<svg xmlns="http://www.w3.org/2000/svg" id="root" width="20px" height="10px" style="max-width: 20px"><defs><linearGradient id="gradient"><stop offset="0%" /></linearGradient></defs><rect id="shape" fill="url(#gradient)" onclick="alert(1)" /></svg>';
  const normalized = normalizeMermaidSvg(source, 2);
  assert.match(normalized, /id="mmd-2"/);
  assert.match(normalized, /id="mmd-2-x1"/);
  assert.match(normalized, /fill="url\(#mmd-2-x1\)"/);
  assert.doesNotMatch(normalized, /onclick/);
  assert.match(normalized, /max-width: 20px/);
});

test('suppresses HTML comment directives while preserving fenced examples', async () => {
  const source = [
    '<!-- markdownlint-disable MD025 -->',
    '',
    '```html',
    '<!-- this is an example -->',
    '```',
  ].join('\n');
  const { html } = await renderMarkdown(source, { fileToRoute });
  assert.doesNotMatch(html, /markdownlint-disable/);
  assert.match(html, /&lt;!-- this is an example --&gt;/);
});

test('renders section title once, hides source directives, and keeps the description', async () => {
  const source = '<!-- markdownlint-disable MD025 -->\n\n# 12. Glossary\n\n| Term | Meaning |\n| --- | --- |\n| Raccord | Example |';
  const { html } = await renderMarkdown(source, { fileToRoute, title: 'Glossary' });
  assert.doesNotMatch(html, /markdownlint-disable/);
  assert.doesNotMatch(html, /<h1[^>]*>12\. Glossary<\/h1>/);
  assert.match(html, /<td>Raccord<\/td>/);
});
