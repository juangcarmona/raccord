# ADR-0013: Deployment topology for Compose, Aspire and Azure

- Status: accepted
- Date: 2026-10-06

## Decision

- **One application image** (`deploy/docker/Dockerfile`): frontend build + `dotnet publish` of `Raccord.Web`, running as the non-root `app` user on port 8080. The same image runs the API/UI or, with the argument `migrate`, applies migrations and exits (`--seed-demo` adds fictional data).
- **Docker Compose** (`deploy/docker-compose.yml`): PostgreSQL 17, a one-shot `migrate` service, and `web` that starts only after migrations succeed. Documents on a named volume. Health checks on both services. Configuration via `deploy/.env` (template: `.env.example`).
- **.NET Aspire** (`src/Raccord.Aspire`): PostgreSQL with a data volume, Azurite blob storage, a `migrations` resource (`migrate --seed-demo`), the API (waits for migrations, health-checked on `/health/ready`) and the Vite dev server.
- **Azure** (`deploy/azure/bicep`): Container Apps environment with the web app (1 replica, liveness/readiness probes) and a manual Container Apps Job for migrations; PostgreSQL Flexible Server (Burstable B1ms, PG 17); Storage account (ZRS, no shared keys, no public blobs, soft delete + versioning); Key Vault (RBAC, purge protection) holding the connection string; a user-assigned managed identity for Key Vault and Blob access; Log Analytics.
- Health: `/health/live` (process) and `/health/ready` (including the database) are exposed in every environment and return only a status word.

## Consequences

- Starting topology cost is low; private networking (VNet integration, private endpoints), Entra-only PostgreSQL auth and zone redundancy are documented hardening steps, not defaults.
- Upgrades: run the migration job first, then roll the web app.
