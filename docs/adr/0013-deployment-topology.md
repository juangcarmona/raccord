---
name: deployment-topology
description: Raccord ships one application image that runs under Docker Compose, .NET Aspire and Azure Container Apps, with migrations applied as a separate step.
status: Accepted
date: 2026-10-06
deciders: ["Juan G. Carmona"]
tags: ["deployment", "docker", "aspire", "azure", "health-checks"]
---

# ADR-0013: Ship one application image deployed through Compose, Aspire and Azure

## Context

Deployment paths must be documented for Docker Compose, .NET Aspire, and Azure/Bicep (AGENTS.md). Migrations are applied by an explicit command, never at web start-up (ADR-0004), and binary documents need storage (ADR-0009).

## Decision

We will build one application image and deploy it through Docker Compose, .NET Aspire and Azure.

- **One application image** (`deploy/docker/Dockerfile`): frontend build + `dotnet publish` of `Raccord.Web`, running as the non-root `app` user on port 8080. The same image runs the API/UI or, with the argument `migrate`, applies migrations and exits (`--seed-demo` adds fictional data).
- **Docker Compose** (`deploy/docker-compose.yml`): PostgreSQL 17, a one-shot `migrate` service, and `web` that starts only after migrations succeed. Documents on a named volume. Health checks on both services. Configuration via `deploy/.env` (template: `.env.example`).
- **.NET Aspire** (`src/Raccord.Aspire`): PostgreSQL with a data volume, Azurite blob storage, a `migrations` resource (`migrate --seed-demo`), the API (waits for migrations, health-checked on `/health/ready`) and the Vite dev server.
- **Azure** (`deploy/azure/bicep`): Container Apps environment with the web app (1 replica, liveness/readiness probes) and a manual Container Apps Job for migrations; PostgreSQL Flexible Server (Burstable B1ms, PG 17); Storage account (ZRS, no shared keys, no public blobs, soft delete + versioning); Key Vault (RBAC, purge protection) holding the connection string; a user-assigned managed identity for Key Vault and Blob access; Log Analytics.
- Health: `/health/live` (process) and `/health/ready` (including the database) are exposed in every environment and return only a status word.

## Consequences

- **Positive:**
    - Starting topology cost is low.
- **Negative:**
    - Private networking (VNet integration, private endpoints), Entra-only PostgreSQL auth and zone redundancy are documented hardening steps, not defaults.
- **Neutral:**
    - Upgrades: run the migration job first, then roll the web app.

## Alternatives considered

- **Private networking, Entra-only PostgreSQL auth and zone redundancy by default:** rejected for the starting topology to keep its cost low; they remain documented hardening steps.
- **Apply migrations when the web app starts:** rejected because migrations are an explicit step (ADR-0004); the same image runs them with the `migrate` argument.

## References

- Related: ADR-0004, ADR-0007, ADR-0009, ADR-0011
