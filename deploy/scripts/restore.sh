#!/usr/bin/env bash
# Restores a backup made by backup.sh into a Docker Compose installation whose database is empty or disposable.
# Usage: deploy/scripts/restore.sh <backup-directory> <stamp>   (run from the deploy/ folder)
# The web service is stopped during the restore and started again afterwards.
set -euo pipefail

if (($# != 2)); then
  echo "Usage: bash scripts/restore.sh <backup-directory> <stamp>" >&2
  exit 2
fi

project="${COMPOSE_PROJECT_NAME:-raccord}"
source_dir="$(cd "$1" && pwd)"
stamp="$2"

( cd "$source_dir" && sha256sum --check "raccord-$stamp.sha256" )

image_file="$source_dir/raccord-image-$stamp.txt"
if [[ -s "$image_file" ]]; then
  export RACCORD_IMAGE="$(<"$image_file")"
  echo "Restoring application image: $RACCORD_IMAGE"
fi

docker compose -p "$project" up -d postgres
docker compose -p "$project" stop web || true
until docker compose -p "$project" exec -T postgres sh -c 'pg_isready -U "$POSTGRES_USER" -d "$POSTGRES_DB"' >/dev/null; do sleep 1; done

echo "Restoring database..."
# Recreate every application schema so pg_restore can restore the complete database dump,
# including module schemas and append-only triggers.
docker compose -p "$project" exec -T postgres sh -c 'psql -v ON_ERROR_STOP=1 -U "$POSTGRES_USER" -d "$POSTGRES_DB"' <<'SQL'
DO $restore$
DECLARE
    schema_name text;
BEGIN
    FOR schema_name IN
        SELECT nspname
        FROM pg_namespace
        WHERE nspname <> 'information_schema'
          AND nspname NOT LIKE 'pg_%'
    LOOP
        EXECUTE format('DROP SCHEMA %I CASCADE', schema_name);
    END LOOP;
    EXECUTE 'CREATE SCHEMA public';
END;
$restore$;
SQL
docker compose -p "$project" exec -T postgres sh -c 'pg_restore -U "$POSTGRES_USER" -d "$POSTGRES_DB" --no-owner --exit-on-error' \
  < "$source_dir/raccord-db-$stamp.dump"

echo "Restoring documents..."
docker volume create "${project}_documents" >/dev/null
docker run --rm -v "${project}_documents:/data" -v "$source_dir:/backup:ro" alpine:3 \
  sh -c "find /data -mindepth 1 -delete && tar xzf /backup/raccord-documents-$stamp.tgz -C /data && chown -R 1654:1654 /data"

docker compose -p "$project" up -d web
echo "Restore complete. Verify with: curl -fsS http://localhost:\${RACCORD_HTTP_PORT:-8080}/health/ready"
