# Azure Bicep

- `main.bicep` — resource-group deployment: monitoring, managed identity, storage, PostgreSQL, Key Vault, Container Apps (web + migration job).
- `main.bicepparam` — example parameters; the image and database password are read from `RACCORD_IMAGE` and `RACCORD_POSTGRES_PASSWORD`.
- `modules/` — one module per concern.

Validate locally:

```bash
az bicep build --file deploy/azure/bicep/main.bicep --stdout
```

```bash
az bicep lint --file deploy/azure/bicep/main.bicep
```

Usage, outputs and production hardening: [Azure deployment](../../../docs/operations/azure-deployment.md). Decisions: ADR-0013.
