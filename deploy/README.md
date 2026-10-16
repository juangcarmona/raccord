# Deployment

- `docker/Dockerfile` — the single application image (frontend build + .NET publish, non-root, port 8080). Build from the repository root.
- `docker-compose.yml` + `.env.example` — self-hosted installation: PostgreSQL, one-shot `migrate` job, `web`. When an official release is available, set `RACCORD_IMAGE` to its exact tag or digest from [GitHub Releases](https://github.com/juangcarmona/raccord/releases). If the release list is empty, there is no supported official image to install yet. See [self-hosting](../docs/operations/self-hosting.md).
- `scripts/backup.sh`, `scripts/restore.sh` — database dump + document archive with checksums and configurable local retention. See [backup and restore](../docs/operations/backup-and-restore.md).
- `scripts/upgrade.sh` — pulls the selected release, verifies a database and document backup, runs migrations, replaces the web image and checks readiness. The backup records the previous image for recovery. See [self-hosting](../docs/operations/self-hosting.md).
- `.github/workflows/release.yml` — publishes stable `vMAJOR.MINOR.PATCH` tags to GHCR and creates a GitHub Release with the immutable image digest, SBOM and provenance.
- `scripts/export.sh` — host-operator migration bundle with database, documents and checksum. See [data portability](../docs/operations/data-portability.md).
- `azure/bicep/` — Azure Container Apps, PostgreSQL Flexible Server, Storage, Key Vault, managed identity. See [Azure deployment](../docs/operations/azure-deployment.md).
- .NET Aspire development orchestration lives in `src/root/Raccord.Aspire`.

Templates never contain secrets or customer-specific identifiers. `deploy/.env` and `deploy/backups*/` are git-ignored.
