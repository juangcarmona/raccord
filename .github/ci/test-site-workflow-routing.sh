#!/usr/bin/env bash
set -euo pipefail

public_workflow=".github/workflows/public-site.yml"
engineering_workflow=".github/workflows/engineering-site.yml"
ci_workflow=".github/workflows/ci.yml"
post_merge_workflow=".github/workflows/post-merge.yml"

require_path() {
  local workflow="$1"
  local path="$2"
  if ! grep -Fq -- "$path" "$workflow"; then
    printf 'Expected %s to watch or ignore %s\n' "$workflow" "$path" >&2
    exit 1
  fi
  printf 'PASS %s contains %s\n' "$workflow" "$path"
}

require_path "$public_workflow" 'src/public-site/**'
require_path "$public_workflow" '.github/workflows/public-site.yml'
require_path "$engineering_workflow" 'src/**'
require_path "$engineering_workflow" '!src/public-site/**'
require_path "$engineering_workflow" '.github/workflows/engineering-site.yml'
if grep -Fq -- 'openspec/changes/nightly-e2e-and-documentation-site/**' "$engineering_workflow"; then
  printf 'OpenSpec-only changes must not trigger the engineering-site workflow\n' >&2
  exit 1
fi
require_path "$post_merge_workflow" 'openspec/**'
require_path "$post_merge_workflow" 'src/public-site/**'
require_path "$post_merge_workflow" '.github/workflows/public-site.yml'
app_job_conditions="$(grep -Fc "if: needs.changes.outputs.non_app_only != 'true'" "$ci_workflow")"
if [ "$app_job_conditions" -ne 6 ]; then
  printf 'Expected six application jobs gated for documentation/public-site-only diffs, found %s\n' "$app_job_conditions" >&2
  exit 1
fi
printf 'PASS engineering workflow excludes public-site paths and main CI gates six app jobs\n'
bash .github/ci/test-classify-changes.sh
