# 7. Deployment view

All paths use the same container image built by `deploy/docker/Dockerfile` (ADR-0013).

## Docker Compose (self-hosting)

```text
host
├── postgres (postgres:17-alpine)       volume postgres-data
├── migrate  (raccord image, "migrate") runs once, exits 0
└── web      (raccord image)            port ${RACCORD_HTTP_PORT:-8080} → 8080, volume documents
```

Configuration: `deploy/.env` (from `.env.example`). Operations: [self-hosting](../../operations/self-hosting.md), [backup and restore](../../operations/backup-and-restore.md). Put a TLS-terminating reverse proxy in front for anything beyond a trusted LAN.

## .NET Aspire (development)

`dotnet run --project src/Raccord.AppHost`: PostgreSQL (volume), Azurite (volume), `migrations` resource, `web` (API) and `frontend` (Vite with proxy). Dashboard with logs, traces and metrics. See [local development](../../operations/local-development.md).

## Azure (Bicep)

```text
Resource group
├── Container Apps environment ── Log Analytics
│   ├── Container App ca-raccord-<env>  (1 replica, ingress HTTPS, probes /health/live, /health/ready)
│   └── Container Apps Job caj-raccord-migrate-<env> (manual, args: migrate)
├── User-assigned managed identity ── Key Vault Secrets User, Storage Blob Data Contributor
├── Key Vault (RBAC, purge protection) ── secret: database connection string
├── PostgreSQL Flexible Server 17 (B1ms, 14-day backups) ── database raccord
└── Storage account (ZRS, no shared key, no public blobs, soft delete, versioning) ── container documents
```

See [Azure deployment](../../operations/azure-deployment.md).
