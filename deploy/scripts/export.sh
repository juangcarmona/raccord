#!/usr/bin/env bash
# Creates a host-operator migration bundle containing the database and documents.
# Usage: deploy/scripts/export.sh [output-directory] (run from the deploy/ folder)
set -euo pipefail

target="${1:-./exports}"
stamp="$(date -u +%Y%m%dT%H%M%SZ)"
mkdir -p "$target"
target="$(cd "$target" && pwd)"
work="${target}/.raccord-export-${stamp}-$$"
mkdir "$work"
trap 'rm -rf "$work"' EXIT

RACCORD_BACKUP_RETENTION_DAYS=999999999 bash scripts/backup.sh "$work"
archive="$target/raccord-export-$stamp.tgz"
tar czf "$archive" -C "$work" .
( cd "$target" && sha256sum "$(basename "$archive")" > "$(basename "$archive").sha256" )
echo "Export written to $archive"
