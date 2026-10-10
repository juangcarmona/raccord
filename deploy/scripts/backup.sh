#!/usr/bin/env bash
# Backs up a Docker Compose installation: a PostgreSQL custom-format dump plus an archive of stored documents.
# Usage: deploy/scripts/backup.sh [backup-directory]   (run from the deploy/ folder, with the stack running)
set -euo pipefail

project="${COMPOSE_PROJECT_NAME:-raccord}"
target="${1:-./backups}"
stamp="$(date -u +%Y%m%dT%H%M%SZ)"
mkdir -p "$target"
target="$(cd "$target" && pwd)"

echo "Dumping database..."
docker compose -p "$project" exec -T postgres sh -c 'pg_dump -U "$POSTGRES_USER" -d "$POSTGRES_DB" --format=custom' \
  > "$target/raccord-db-$stamp.dump"

echo "Archiving documents..."
docker run --rm -v "${project}_documents:/data:ro" -v "$target:/backup" alpine:3 \
  tar czf "/backup/raccord-documents-$stamp.tgz" -C /data .

( cd "$target" && sha256sum "raccord-db-$stamp.dump" "raccord-documents-$stamp.tgz" > "raccord-$stamp.sha256" )
echo "Backup written to $target (stamp $stamp)"
