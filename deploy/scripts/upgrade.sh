#!/usr/bin/env bash
# Upgrades a Docker Compose installation only after a verified backup and migration.
# Usage: deploy/scripts/upgrade.sh [backup-directory] (run from the deploy/ folder)
set -euo pipefail

backup_directory="${1:-./backups}"
health_url="${RACCORD_HEALTH_URL:-http://localhost:${RACCORD_HTTP_PORT:-8080}/health/ready}"

echo "Creating pre-upgrade backup..."
bash scripts/backup.sh "$backup_directory"

echo "Building the new application image..."
docker compose build migrate web

echo "Applying database migrations..."
docker compose run --rm migrate

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
echo "Restore the pre-upgrade backup before retrying." >&2
exit 1
