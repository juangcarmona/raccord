---
name: publish-versioned-compose-releases
description: Distribute supported self-hosted releases as versioned application images through Docker Compose.
status: Accepted
date: 2026-10-16
deciders: ["Juan G. Carmona"]
tags: ["deployment", "releases", "docker", "compose"]
---

# ADR-0026: Distribute versioned application images through Docker Compose

## Context

Raccord has Docker Compose, .NET Aspire and Azure deployment assets, but no release channel or supported operator update contract. The Compose file builds the application locally by default and can select an image through `RACCORD_IMAGE`. The guarded upgrade script takes a database and document backup before applying migrations, and operators can restore that backup if the new version cannot run. Issued invoices, their numbering, audit history and stored documents must survive supported upgrades and recovery.

## Decision

We will make Docker Compose the supported self-host installation path and publish official, immutable application images to GitHub Container Registry for tagged releases. Each release will have a version tag and a recorded image digest; operators will pin an exact version in their environment file. Source builds and custom images remain available, but documentation will label them as modified or development builds without the official release support promise. The Compose migration service will apply migrations before the web service starts. Every upgrade must begin with a verified database and document backup. A failed migration or unhealthy upgraded application requires restoring that backup before retrying; an application-only rollback is unsupported when the database has been migrated. Only the latest published stable release receives support and security fixes. .NET Aspire remains a development workflow, and Azure remains a separately documented deployment topology rather than the self-host release installer.

## Consequences

- **Positive:** Operators can install a named release without cloning the source or compiling the application, and can reproduce the same selected image later.
- **Negative:** The project must maintain a release workflow, publish and scan images, and keep the Compose upgrade and restore path tested. Operators must manage backups and downtime for migration and recovery.
- **Neutral:** Support is limited to the latest stable release. Older installations must update to a current release before receiving support. A failed migration is recovered by restoring both database and documents; the release process must document the resulting recovery point clearly.

## Alternatives considered

- **Keep source builds as the supported installation path:** rejected because operators would need a compiler toolchain and a source checkout to install every update, and could not distinguish an official build from a modified one.
- **Use .NET Aspire as the end-user installer:** rejected because the current Aspire host is the development topology and does not provide a supported release-selection or upgrade contract for operators.
- **Publish an installer that automatically rolls the database back:** rejected because database migrations can be irreversible and restoring only the application image could corrupt or hide business records.
- **Support several older release lines:** rejected for the initial release process because a single-maintainer project cannot yet promise concurrent security fixes and migration testing across multiple lines.

## References

- Issue: [#264](https://github.com/juangcarmona/raccord/issues/264)
- Follow-up implementation: [#265](https://github.com/juangcarmona/raccord/issues/265)
- Related: [ADR-0004](0004-postgresql-ef-core-append-only.md), [ADR-0009](0009-document-storage.md), [ADR-0013](0013-deployment-topology.md), [ADR-0019](0019-module-owned-schemas-with-a-shared-transaction.md)
- Operations: [self-hosting](../operations/self-hosting.md), [backup and restore](../operations/backup-and-restore.md)
