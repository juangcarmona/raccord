#!/usr/bin/env bash
set -euo pipefail

classifier="$(dirname "$0")/classify-changes.sh"

assert_route() {
  local expected="$1"
  local name="$2"
  local actual
  shift 2
  actual="$(printf '%s\0' "$@" | bash "$classifier")"
  if [ "$actual" != "$expected" ]; then
    printf 'Expected %s for %s, got %s\n' "$expected" "$name" "$actual" >&2
    exit 1
  fi
  printf 'PASS %s\n' "$name"
}

assert_route true 'OpenSpec-only paths' 'openspec/changes/example/proposal.md' 'openspec/specs/example/spec.md'
assert_route false 'mixed OpenSpec and source paths' 'openspec/changes/example/tasks.md' 'src/Raccord.Web/Program.cs'
assert_route false 'public-site-only paths' 'src/public-site/index.html'
assert_route false 'application-only paths' 'src/Raccord.Web/Program.cs'
assert_route false 'combined application and public-site paths' 'src/public-site/index.html' 'src/Raccord.Web/Program.cs'
assert_route false 'non-OpenSpec documentation paths' 'docs/architecture/overview.md'
assert_route false 'empty diff'
