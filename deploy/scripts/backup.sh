#!/usr/bin/env bash
# Backs up a Docker Compose installation: a PostgreSQL custom-format dump plus an archive of stored documents.
# Usage: deploy/scripts/backup.sh [backup-directory]   (run from the deploy/ folder, with the stack running)
set -euo pipefail

project="${COMPOSE_PROJECT_NAME:-raccord}"
target="${1:-./backups}"
stamp="$(date -u +%Y%m%dT%H%M%SZ)"
retention_days="${RACCORD_BACKUP_RETENTION_DAYS:-30}"

if [[ ! "$retention_days" =~ ^[0-9]+$ ]]; then
  echo "RACCORD_BACKUP_RETENTION_DAYS must be a non-negative integer" >&2
  exit 2
fi

mkdir -p "$target"
target="$(cd "$target" && pwd)"
work="${target}/.raccord-backup-${stamp}-$$"
mkdir "$work"
trap 'rm -rf "$work"' EXIT

web_container="$(docker compose -p "$project" ps -q web)"
if [[ -n "$web_container" ]]; then
  docker inspect --format '{{.Config.Image}}' "$web_container" > "$work/raccord-image-$stamp.txt"
fi

echo "Dumping database..."
docker compose -p "$project" exec -T postgres sh -c 'pg_dump -U "$POSTGRES_USER" -d "$POSTGRES_DB" --format=custom' \
  > "$work/raccord-db-$stamp.dump"

echo "Archiving documents..."
docker run --rm -v "${project}_documents:/data:ro" -v "$target:/backup" alpine:3 \
  tar czf "/backup/.raccord-backup-${stamp}-$$/raccord-documents-$stamp.tgz" -C /data .

backup_files=("raccord-db-$stamp.dump" "raccord-documents-$stamp.tgz")
if [[ -f "$work/raccord-image-$stamp.txt" ]]; then
  backup_files+=("raccord-image-$stamp.txt")
fi
( cd "$work" && sha256sum "${backup_files[@]}" > "raccord-$stamp.sha256" )
mv "$work"/raccord-* "$target/"
find "$target" -maxdepth 1 -type f -name 'raccord-*.sha256' -mtime "+$retention_days" -print0 |
  while IFS= read -r -d '' manifest; do
    old_stamp="${manifest##*/raccord-}"
    old_stamp="${old_stamp%.sha256}"
    rm -f "$manifest" "$target/raccord-db-$old_stamp.dump" \
      "$target/raccord-documents-$old_stamp.tgz" "$target/raccord-image-$old_stamp.txt"
  done
echo "Backup written to $target"
echo "Backup stamp: $stamp"
