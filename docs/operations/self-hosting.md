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
|---|---|---|
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

```bash
git pull
```

```bash
docker compose up -d --build
```

The `migrate` service applies new migrations before `web` starts. Take a backup first.

## TLS and exposure

Terminate TLS in a reverse proxy (Caddy, Traefik, nginx) and forward to the published port. Do not publish the PostgreSQL port.

See also [backup and restore](backup-and-restore.md).
