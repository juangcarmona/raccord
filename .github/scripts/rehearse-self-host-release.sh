#!/usr/bin/env bash
set -euo pipefail

previous_image="${1:?Provide the previous image reference}"
candidate_image="${2:?Provide the candidate image reference}"
broken_image="${3:?Provide the deliberately failing image reference}"
backup_directory="${RUNNER_TEMP:-/tmp}/raccord-release-rehearsal-backups"
repository_directory="$(pwd)"
cd "$repository_directory/deploy"

export COMPOSE_PROJECT_NAME="raccord-release-rehearsal"
export POSTGRES_PASSWORD="ci-only-release-rehearsal"
export RACCORD_HTTP_PORT="18080"
export RACCORD_SEED_DEMO_DATA="true"
export RACCORD_IMAGE="$previous_image"

cleanup() {
  RACCORD_IMAGE="$candidate_image" docker compose down --volumes --remove-orphans || true
  docker rm -f raccord-release-registry >/dev/null 2>&1 || true
}
trap cleanup EXIT

rm -rf "$backup_directory"
mkdir -p "$backup_directory"

echo "Installing the PR base image from a clean database..."
docker compose up -d --wait
customer_count_before="$(docker compose exec -T postgres psql -At -U raccord -d raccord -c 'SELECT count(*) FROM contacts.customers')"
project_count_before="$(docker compose exec -T postgres psql -At -U raccord -d raccord -c 'SELECT count(*) FROM projects.projects')"
if [[ "$customer_count_before" -eq 0 || "$project_count_before" -eq 0 ]]; then
  echo "The clean install did not seed the expected customer and project records." >&2
  exit 1
fi

docker run --rm -v "${COMPOSE_PROJECT_NAME}_documents:/data" alpine:3.22 \
  sh -c 'printf "release rehearsal document\n" > /data/release-rehearsal.txt'
docker run --rm -v "${COMPOSE_PROJECT_NAME}_documents:/data:ro" alpine:3.22 \
  sh -c 'test "$(cat /data/release-rehearsal.txt)" = "release rehearsal document"'

echo "Upgrading to the PR candidate image..."
RACCORD_IMAGE="$candidate_image" bash scripts/upgrade.sh "$backup_directory"
customer_count_after="$(docker compose exec -T postgres psql -At -U raccord -d raccord -c 'SELECT count(*) FROM contacts.customers')"
project_count_after="$(docker compose exec -T postgres psql -At -U raccord -d raccord -c 'SELECT count(*) FROM projects.projects')"
[[ "$customer_count_after" == "$customer_count_before" ]]
[[ "$project_count_after" == "$project_count_before" ]]
docker run --rm -v "${COMPOSE_PROJECT_NAME}_documents:/data:ro" alpine:3.22 \
  sh -c 'test "$(cat /data/release-rehearsal.txt)" = "release rehearsal document"'
upgraded_web_container="$(docker compose ps -q web)"

sleep 1
echo "Checking that a failed migration leaves the current web container running and can be restored..."
if RACCORD_IMAGE="$broken_image" bash scripts/upgrade.sh "$backup_directory"; then
  echo "The deliberately failing migration unexpectedly succeeded." >&2
  exit 1
fi
web_after_failed_migration="$(docker compose ps -q web)"
[[ "$web_after_failed_migration" == "$upgraded_web_container" ]]

failed_upgrade_stamp="$(find "$backup_directory" -maxdepth 1 -type f -name 'raccord-*.sha256' -printf '%f\n' | sort | tail -n 1 | sed -E 's/^raccord-(.*)\.sha256$/\1/')"
[[ -n "$failed_upgrade_stamp" ]]
RACCORD_IMAGE="$candidate_image" bash scripts/restore.sh "$backup_directory" "$failed_upgrade_stamp"
customer_count_restored="$(docker compose exec -T postgres psql -At -U raccord -d raccord -c 'SELECT count(*) FROM contacts.customers')"
project_count_restored="$(docker compose exec -T postgres psql -At -U raccord -d raccord -c 'SELECT count(*) FROM projects.projects')"
[[ "$customer_count_restored" == "$customer_count_before" ]]
[[ "$project_count_restored" == "$project_count_before" ]]
curl --fail --silent --show-error "http://localhost:${RACCORD_HTTP_PORT}/health/ready" >/dev/null
docker run --rm -v "${COMPOSE_PROJECT_NAME}_documents:/data:ro" alpine:3.22 \
  sh -c 'test "$(cat /data/release-rehearsal.txt)" = "release rehearsal document"'

echo "Self-host release rehearsal passed. This exercises the PR base/candidate path before any prior official release exists."
