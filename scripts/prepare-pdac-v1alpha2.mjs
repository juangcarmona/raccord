import { readFile, readdir, mkdir, writeFile } from 'node:fs/promises';
import { join, relative, dirname } from 'node:path';
import { parseArtifactDocument } from '@prodshape/core';

const model = 'docs/product/model';
const proposed = 'docs/product/changes/active/chg-pdac-v1alpha2-001/proposed';

async function files(directory) {
  const entries = await readdir(directory, { withFileTypes: true });
  const nested = await Promise.all(entries.map(entry => {
    const path = join(directory, entry.name);
    return entry.isDirectory() ? files(path) : [path];
  }));
  return nested.flat();
}

function parse(source, path) {
  const result = parseArtifactDocument(source, path);
  if (result.diagnostics.length || !result.artifact) throw Error(`Cannot parse ${path}`);
  return result.artifact;
}

function checkChange(before, after, path, field, expected) {
  const original = parse(before, path);
  const changed = parse(after, path);
  if (original.body !== changed.body) throw Error(`Body changed: ${path}`);
  const metadata = { ...changed.frontmatter };
  if (JSON.stringify(metadata[field]) !== JSON.stringify(expected)) throw Error(`Unexpected ${field}: ${path}`);
  metadata[field] = original.frontmatter[field];
  if (JSON.stringify(metadata) !== JSON.stringify(original.frontmatter)) throw Error(`Other metadata changed: ${path}`);
}

const originals = new Map();
for (const path of await files(model)) {
  if (!path.endsWith('.md')) continue;
  const source = await readFile(path, 'utf8');
  originals.set(path, { source, artifact: parse(source, path) });
}

const additions = new Map();
const modified = new Map();
let pairs = 0;
for (const [path, { source, artifact }] of originals) {
  if (artifact.frontmatter.type !== 'business-rule') continue;
  const selectors = artifact.frontmatter['applies-to'] ?? [];
  const useCases = selectors.filter(id => id.startsWith('UC-'));
  if (!useCases.length) continue;
  pairs += useCases.length;
  for (const id of useCases) {
    const ids = additions.get(id) ?? new Set();
    ids.add(artifact.frontmatter.id);
    additions.set(id, ids);
  }
  const lines = source.split('\n');
  const start = lines.findIndex(line => line === 'applies-to:');
  if (start < 0) throw Error(`Unsupported selector layout: ${path}`);
  let end = start + 1;
  while (lines[end]?.startsWith('  - ')) end++;
  const remaining = selectors.filter(id => !id.startsWith('UC-'));
  const replacement = remaining.length ? ['applies-to:', ...remaining.map(id => `  - "${id}"`)] : ['applies-to: []'];
  lines.splice(start, end - start, ...replacement);
  const after = lines.join('\n');
  checkChange(source, after, path, 'applies-to', remaining);
  modified.set(path, after);
}

for (const [id, rules] of additions) {
  const found = [...originals].find(([, value]) => value.artifact.frontmatter.id === id);
  if (!found || found[1].artifact.frontmatter.type !== 'use-case') throw Error(`Missing use case: ${id}`);
  const [path, { source, artifact }] = found;
  const current = artifact.frontmatter['governed-by'] ?? [];
  const next = [...new Set([...current, ...rules])];
  const lines = source.split('\n');
  const start = lines.findIndex(line => line === 'governed-by:' || line === 'governed-by: []');
  if (start < 0) throw Error(`Missing governed-by: ${path}`);
  let end = start + 1;
  while (lines[end]?.startsWith('  - ')) end++;
  lines.splice(start, end - start, 'governed-by:', ...next.map(rule => `  - "${rule}"`));
  const after = lines.join('\n');
  checkChange(source, after, path, 'governed-by', next);
  modified.set(path, after);
}

for (const [path, content] of modified) {
  const destination = join(proposed, relative(model, path));
  await mkdir(dirname(destination), { recursive: true });
  await writeFile(destination, content);
}

const originalPairs = new Set();
const proposedPairs = new Set();
for (const [path, { artifact }] of originals) {
  if (artifact.frontmatter.type === 'business-rule') {
    for (const id of artifact.frontmatter['applies-to'] ?? []) if (id.startsWith('UC-')) originalPairs.add(`${id}:${artifact.frontmatter.id}`);
  }
  if (artifact.frontmatter.type !== 'use-case') continue;
  for (const rule of artifact.frontmatter['governed-by'] ?? []) originalPairs.add(`${artifact.frontmatter.id}:${rule}`);
  const next = modified.has(path) ? parse(modified.get(path), path) : artifact;
  for (const rule of next.frontmatter['governed-by'] ?? []) proposedPairs.add(`${artifact.frontmatter.id}:${rule}`);
}
if (originalPairs.size !== proposedPairs.size || [...originalPairs].some(pair => !proposedPairs.has(pair))) throw Error('Rule governance pairs changed');

console.log(JSON.stringify({ legacyRuleSelectors: pairs, governancePairs: proposedPairs.size, businessRules: [...modified.keys()].filter(path => path.includes('/business-rules/')).length, useCases: additions.size, modified: modified.size, ids: [...modified].map(([, source]) => parse(source, 'proposed').frontmatter.id).sort() }));
