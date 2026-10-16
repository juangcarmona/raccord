#!/usr/bin/env bash
set -euo pipefail

has_path=false
while IFS= read -r -d '' path; do
  [ -n "$path" ] || continue
  has_path=true
  case "$path" in
    openspec/*|src/public-site/*|.github/workflows/public-site.yml) ;;
    *) printf 'false\n'; exit 0 ;;
  esac
done

if [ "$has_path" = true ]; then
  printf 'true\n'
else
  printf 'false\n'
fi
