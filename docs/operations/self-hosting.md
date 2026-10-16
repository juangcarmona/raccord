# Self-hosting with Docker Compose

Raccord runs as three services: PostgreSQL, a one-shot migration job and the web application (API and UI in one container).

> **Security notice:** sign-in is required after the first administrator is created, and there is no public registration. The first administrator is created through a one-time unauthenticated setup endpoint. Run the setup only on a trusted network and complete it before making the application reachable to others. Use TLS for remote access and never publish the PostgreSQL port.

## Install

From the `deploy/` folder:

1. Create the configuration file and set a strong database password:

   ```bash
   cp .env.example .env
   ```

   Edit `.env`: set `POSTGRES_PASSWORD` (for example `openssl rand -base64 32`). Leave `RACCORD_SEED_DEMO_DATA=false` for a normal installation. Setting it to `true` seeds fictional data and a shared demo account (`admin@example.com` / `DemoPassword123!`); never expose that demo installation to other people or the internet.

2. Build and start:

   ```bash
   docker compose up -d --build
   ```

3. Check health (should print `Healthy`):

   ```bash
   curl -fsS http://localhost:8080/health/ready
   ```

4. Create the first administrator on the trusted host. The command below prompts for the email and password so the password does not appear in shell history or the `curl` arguments. It uses Python 3 to encode the request body safely:

   ```bash
   read -r -p 'Administrator email: ' ADMIN_EMAIL
   read -r -s -p 'Administrator password: ' ADMIN_PASSWORD
   printf '\n'
   export ADMIN_EMAIL ADMIN_PASSWORD
   python3 -c 'import json, os; print(json.dumps({"email": os.environ["ADMIN_EMAIL"], "password": os.environ["ADMIN_PASSWORD"]}))' \
     | curl -fsS -X POST http://localhost:8080/api/setup \
       -H 'Content-Type: application/json' --data-binary @-
   unset ADMIN_EMAIL ADMIN_PASSWORD
   ```

   Choose a unique password that meets the application’s validation rules. The setup endpoint accepts this operation only while the installation has no users. Avoid shared terminal logs and unset the variables if the command fails.

5. Open `http://localhost:8080`, sign in with the administrator account, go to **Empresa emisora / Issuing company** and enter the legal details used on invoices.

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

Terminate TLS in a reverse proxy (Caddy, Traefik, nginx) and forward to the published port. Do not publish the PostgreSQL port. Keep the initial setup endpoint reachable only from the trusted network until the first administrator has been created.

See also [backup and restore](backup-and-restore.md).
