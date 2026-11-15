import assert from 'node:assert/strict';
import { spawnSync } from 'node:child_process';
import { test } from 'node:test';

function run(command) {
  return spawnSync(process.execPath, ['scripts/guard-git-hooks.mjs'], {
    input: JSON.stringify({ tool_input: { command } }),
    encoding: 'utf8',
  });
}

for (const command of [
  'git commit --no-verify -m "message"',
  'git -C repo commit -n -m "message"',
  'git push origin main --no-verify',
  'HUSKY=0 git commit -m "message"',
  'export HUSKY="0"; git push',
]) {
  test(`guard_Should_Block_${command}`, () => {
    const result = run(command);
    assert.equal(result.status, 2);
    assert.match(result.stderr, /Git hooks must run/u);
  });
}

for (const command of [
  'git commit -m "message"',
  'git push origin main',
  'git status --short',
  'git commit -am "message"',
  'echo ready',
]) {
  test(`guard_Should_Allow_${command}`, () => {
    assert.equal(run(command).status, 0);
  });
}

test('guard_Should_Allow_Invalid_Input', () => {
  const result = spawnSync(process.execPath, ['scripts/guard-git-hooks.mjs'], {
    input: '{invalid',
    encoding: 'utf8',
  });
  assert.equal(result.status, 0);
});
