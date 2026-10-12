---
name: document-storage
description: Binary documents are stored only through a document-store port, with a file-system adapter for self-hosting and an Azure Blob adapter for Azure.
status: Accepted
date: 2026-10-06
deciders: ["Juan G. Carmona"]
tags: ["documents", "storage", "azure", "self-hosting"]
---

# ADR-0009: Store documents through a port with file-system and Azure Blob adapters

## Context

Raccord stores binary documents such as invoice PDFs (ADR-0008). Self-hosted installations run on Docker, and Azure is the documented cloud path.

## Decision

We will store binary documents only through a document-store port, with a file-system adapter and an Azure Blob adapter.

- `IDocumentStore` (save/read by key) is the only way to store binary documents.
- `FileSystemDocumentStore` is the self-hosting default (a Docker volume). Keys are relative, cannot escape the root, and files are written atomically (temp file + move).
- `AzureBlobDocumentStore` uses `Aspire.Azure.Storage.Blobs`: Azurite under .NET Aspire locally, Azure Storage with managed identity in Azure (`ConnectionStrings__documents` is the blob endpoint URI). Soft delete and versioning are enabled by the Bicep template.
- Selected by `Storage:Provider` (`FileSystem` | `AzureBlob`).

## Consequences

- **Positive:**
    - Further adapters can be added behind the same port without changing callers.
- **Negative:**
    - Backups must include the document volume or container; the scripts do.
- **Neutral:**
    - The adapter is selected by configuration (`Storage:Provider`).

## Alternatives considered

- **S3-compatible storage:** rejected for now. It would cover more self-hosted products, but the container images of popular self-hosted S3 servers changed distribution terms recently, and Azure is the documented cloud path. An S3 adapter can be added behind the same port.

## References

- Related: ADR-0008, ADR-0013
