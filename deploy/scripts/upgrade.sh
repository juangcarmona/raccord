#!/usr/bin/env bash
# Upgrades a Docker Compose installation only after a verified backup and migration.
# Usage: deploy/scripts/upgrade.sh [backup-directory] (run from the deploy/ folder)
set -euo pipefail

backup_directory="${1:-./backups}"
health_url="${RACCORD_HEALTH_URL:-http://localhost:${RACCORD_HTTP_PORT:-8080}/health/ready}"

echo "Checking the selected release image..."
docker compose config --quiet
docker compose pull migrate web

echo "Creating and verifying the pre-upgrade backup..."
backup_result="$(bash scripts/backup.sh "$backup_directory")"
printf '%s\n' "$backup_result"
backup_stamp="$(sed -n 's/^Backup stamp: //p' <<< "$backup_result")"
if [[ ! "$backup_stamp" =~ ^[0-9]{8}T[0-9]{6}Z$ ]]; then
  echo "Could not determine the pre-upgrade backup stamp." >&2
  exit 1
fi
( cd "$backup_directory" && sha256sum --check "raccord-$backup_stamp.sha256" )

echo "Applying database migrations..."
if ! docker compose run --rm migrate; then
  echo "Migration failed. Restore the pre-upgrade backup before retrying:" >&2
  echo "  bash scripts/restore.sh '$backup_directory' '$backup_stamp'" >&2
  exit 1
fi

echo "Starting the migrated application..."
docker compose up -d --no-deps web

for attempt in {1..30}; do
  if curl --fail --silent --show-error "$health_url" >/dev/null; then
    echo "Upgrade complete: $health_url"
    exit 0
  fi
  sleep 2
done

echo "Upgrade failed readiness verification: $health_url" >&2
echo "Restore the pre-upgrade database, documents, and application image before retrying:" >&2
echo "  bash scripts/restore.sh '$backup_directory' '$backup_stamp'" >&2
exit 1
