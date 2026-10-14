# Deployment

- `docker/Dockerfile` — the single application image (frontend build + .NET publish, non-root, port 8080). Build from the repository root.
- `docker-compose.yml` + `.env.example` — self-hosted installation: PostgreSQL, one-shot `migrate` job, `web`. See [self-hosting](../docs/operations/self-hosting.md).
- `scripts/backup.sh`, `scripts/restore.sh` — database dump + document archive with checksums and configurable local retention. See [backup and restore](../docs/operations/backup-and-restore.md).
- `scripts/upgrade.sh` — backup-gated image build, migration and readiness check. See [self-hosting](../docs/operations/self-hosting.md).
- `azure/bicep/` — Azure Container Apps, PostgreSQL Flexible Server, Storage, Key Vault, managed identity. See [Azure deployment](../docs/operations/azure-deployment.md).
- .NET Aspire development orchestration lives in `src/root/Raccord.Aspire`.

Templates never contain secrets or customer-specific identifiers. `deploy/.env` and `deploy/backups*/` are git-ignored.
