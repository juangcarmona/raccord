# ADR-0009: Document storage port with file-system and Azure Blob adapters

- Status: accepted
- Date: 2026-10-06

## Decision

- `IDocumentStore` (save/read by key) is the only way to store binary documents.
- `FileSystemDocumentStore` is the self-hosting default (a Docker volume). Keys are relative, cannot escape the root, and files are written atomically (temp file + move).
- `AzureBlobDocumentStore` uses `Aspire.Azure.Storage.Blobs`: Azurite under .NET Aspire locally, Azure Storage with managed identity in Azure (`ConnectionStrings__documents` is the blob endpoint URI). Soft delete and versioning are enabled by the Bicep template.
- Selected by `Storage:Provider` (`FileSystem` | `AzureBlob`).

## Alternatives considered

S3-compatible storage would cover more self-hosted products, but the container images of popular self-hosted S3 servers changed distribution terms recently, and Azure is the documented cloud path. An S3 adapter can be added behind the same port.

## Consequences

Backups must include the document volume or container; the scripts do.
