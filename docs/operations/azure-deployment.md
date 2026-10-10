# Azure deployment (Bicep)

Templates: `deploy/azure/bicep/main.bicep` and modules. Topology: [arc42 §7](../architecture/arc42/07-deployment-view.md).

## Prerequisites

- Azure CLI with Bicep (`az bicep install`)
- A resource group
- The Raccord image pushed to a registry the Container App can pull from (public or with registry credentials added to the template)

## Deploy

Build and push the image (example with GitHub Container Registry):

```bash
docker build -f deploy/docker/Dockerfile -t ghcr.io/<owner>/raccord:<tag> .
```

```bash
docker push ghcr.io/<owner>/raccord:<tag>
```

Deploy the infrastructure. The password is read from an environment variable by `main.bicepparam` and is never written to a file:

```bash
RACCORD_IMAGE=ghcr.io/<owner>/raccord:<tag> RACCORD_POSTGRES_PASSWORD='<strong password>' az deployment group create --resource-group <rg> --parameters deploy/azure/bicep/main.bicepparam
```

Run migrations (the job name is a deployment output), then confirm readiness on the application URL:

```bash
az containerapp job start --resource-group <rg> --name caj-raccord-migrate-dev
```

## What gets created

Container Apps environment + app (1 replica, HTTPS ingress, probes) + migration job; PostgreSQL Flexible Server 17 (B1ms) with database `raccord`; Storage account with private `documents` container (no shared keys); Key Vault with the connection string; user-assigned managed identity with *Key Vault Secrets User* and *Storage Blob Data Contributor*; Log Analytics.

## Hardening for production

- VNet-integrated Container Apps environment and private endpoints for PostgreSQL, Storage and Key Vault; remove the "Azure services" firewall rule.
- Entra ID authentication for PostgreSQL with the managed identity instead of a password.
- Zone-redundant PostgreSQL (General Purpose tier), longer backup retention, geo-redundant backups.
- Application Insights via the OTLP exporter; alerts on readiness failures and integrity errors.
- Authentication in front of the app until Raccord's own sign-in ships (Container Apps built-in auth with Entra ID).
