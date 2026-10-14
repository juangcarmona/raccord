# Self-hosting with Docker Compose

Raccord runs as three services: PostgreSQL, a one-shot migration job and the web application (API and UI in one container).

> **Security notice:** authentication is not implemented yet (MVP slice 2). Run Raccord only on a trusted network or behind an authenticating reverse proxy until then.

## Install

From the `deploy/` folder:

1. Create the configuration file and set a strong database password:

   ```bash
   cp .env.example .env
   ```

   Edit `.env`: set `POSTGRES_PASSWORD` (for example `openssl rand -base64 32`). Set `RACCORD_SEED_DEMO_DATA=true` only for a demo installation.

2. Build and start:

   ```bash
   docker compose up -d --build
   ```

3. Check health (should print `Healthy`):

   ```bash
   curl -fsS http://localhost:8080/health/ready
   ```

4. Open `http://localhost:8080`, go to **Empresa emisora / Issuing company** and enter the legal details used on invoices.

## Configuration

| Variable | Default | Meaning |
| --- | --- | --- |
| `POSTGRES_PASSWORD` | — (required) | Database password |
| `POSTGRES_DB`, `POSTGRES_USER` | `raccord` | Database and user |
| `RACCORD_HTTP_PORT` | `8080` | Published port |
| `RACCORD_TIME_ZONE` | `Europe/Madrid` | Determines "today" for issue dates (`Atlantic/Canary` for the Canary Islands) |
| `RACCORD_SEED_DEMO_DATA` | `false` | Fictional demo data on an empty database |
| `RACCORD_IMAGE` | `raccord:local` | Image tag to run instead of building |

Documents (invoice PDFs) are stored in the `documents` volume; the database in `postgres-data`.

## Health checks

- `GET /health/live` — the process is running.
- `GET /health/ready` — the application can reach the database.

Both return only `Healthy`/`Unhealthy`. Compose uses readiness for the `web` service.

## Upgrades

Use the guarded upgrade script from `deploy/` after reviewing the release notes:

```bash
bash scripts/upgrade.sh ./backups
```

The script creates a database and document backup, builds the new image, runs the migration job, replaces `web` only after migrations succeed and verifies `/health/ready`. It leaves the previous web container running while the migration gate is executing, so a failed migration does not publish the new application image.

For a manual upgrade, preserve the same order:

1. Run `bash scripts/backup.sh ./backups` and keep the reported stamp.
2. Run `docker compose build migrate web`.
3. Run `docker compose run --rm migrate` and stop if it fails.
4. Run `docker compose up -d --no-deps web`.
5. Verify `curl -fsS http://localhost:8080/health/ready`.

If readiness fails after the web replacement, stop the stack, restore the recorded backup with `scripts/restore.sh`, and verify readiness before retrying. Keep the backup until the upgraded installation has passed its smoke checks.

## TLS and exposure

Terminate TLS in a reverse proxy (Caddy, Traefik, nginx) and forward to the published port. Do not publish the PostgreSQL port.

See also [backup and restore](backup-and-restore.md).
