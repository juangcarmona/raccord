# Self-hosting with Docker Compose

Raccord runs as three services: PostgreSQL, a one-shot migration job and the web application (API and UI in one container).

## Choose a release

Docker Compose is the supported self-host installation path. Stable releases are published as versioned container images in [GitHub Releases](https://github.com/juangcarmona/raccord/releases) and GitHub Container Registry. Use the latest stable release and copy its immutable `ghcr.io/juangcarmona/raccord@sha256:...` image reference from the release notes into `RACCORD_IMAGE`. The `latest` image tag is a convenience alias; pinning by digest keeps an installation on the exact image it selected.

Only the latest stable release receives support and security fixes. Source-built and modified images are available for development and customization, but do not receive the official release support promise. .NET Aspire is the development workflow; Azure is a separate deployment topology.

> **Security notice:** sign-in is required after the first administrator is created, and there is no public registration. The first administrator is created through a one-time unauthenticated setup endpoint. Run the setup only on a trusted network and complete it before making the application reachable to others. Use TLS for remote access and never publish the PostgreSQL port.

## Install

From the `deploy/` folder:

1. Create the configuration file:

   ```bash
   cp .env.example .env
   ```

   Edit `.env` and set `RACCORD_IMAGE` to the immutable image reference from the chosen GitHub Release. Set `POSTGRES_PASSWORD` (for example `openssl rand -base64 32`). Leave `RACCORD_SEED_DEMO_DATA=false` for a normal installation. Setting it to `true` seeds fictional data and a shared demo account (`admin@example.com` / `DemoPassword123!`); never expose that demo installation to other people or the internet.

2. Pull the selected official release and start the stack:

   ```bash
   docker compose pull
   docker compose up -d
   ```

   Compose waits for PostgreSQL, runs migrations from the selected image, and starts the web service only after migrations succeed.

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
| `RACCORD_IMAGE` | — (required) | Selected official release tag or immutable image digest |
| `RACCORD_HTTP_PORT` | `8080` | Published port |
| `RACCORD_TIME_ZONE` | `Europe/Madrid` | Determines "today" for issue dates (`Atlantic/Canary` for the Canary Islands) |
| `RACCORD_SEED_DEMO_DATA` | `false` | Fictional demo data on an empty database |

Documents (invoice PDFs) are stored in the `documents` volume; the database in `postgres-data`. Compose never builds the application implicitly. For a source or modified image, build it explicitly and set `RACCORD_IMAGE` to that image; it remains outside the official release support promise.

## Health checks

- `GET /health/live` — the process is running.
- `GET /health/ready` — the application can reach the database.

Both return only `Healthy`/`Unhealthy`. Compose uses readiness for the `web` service.

## Upgrades

To upgrade from `deploy/`, choose the latest stable release, copy its immutable image reference into `RACCORD_IMAGE` in `.env`, then run the guarded upgrade script:

```bash
bash scripts/upgrade.sh ./backups
```

The script verifies the Compose configuration and pulls the new image before changing the database. It then creates a database and document backup, records the running application image in that backup, verifies all backup checksums, runs migrations and replaces `web` only after migrations succeed. It checks `/health/ready` before reporting success.

If migration fails, the current web container is left in place and the script prints the exact restore command. If readiness fails after the web service is replaced, restore the paired database, document archive and previous application image before retrying. For example:

```bash
bash scripts/restore.sh ./backups <backup-stamp>
curl -fsS http://localhost:8080/health/ready
```

The restore script verifies the backup, restores the database and documents, and starts the web service using the image reference recorded with that backup. Keep successful backups off-host and encrypted until the upgraded installation has passed its smoke checks.

## Development and modified images

The application is developed with .NET Aspire. To build a local container image for a deployment-specific change, run this from `deploy/` and set `RACCORD_IMAGE=raccord:dev` in `.env` before running Compose:

```bash
docker build -f docker/Dockerfile -t raccord:dev ..
docker compose up -d
```

Do not describe a source-built or modified image as an official release. It receives no official release support or security-fix promise.

## TLS and exposure

Terminate TLS in a reverse proxy (Caddy, Traefik, nginx) and forward to the published port. Do not publish the PostgreSQL port. Keep the initial setup endpoint reachable only from the trusted network until the first administrator has been created.

See also [backup and restore](backup-and-restore.md).
