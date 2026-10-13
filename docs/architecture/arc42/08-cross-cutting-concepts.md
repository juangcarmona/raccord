---
title: Cross-cutting concepts
arc42-section: "08"
description: Explains concepts and mechanisms that span multiple Raccord building blocks.
---
<!-- markdownlint-disable MD025 -->

# 8. Cross-cutting concepts

## Domain rules and errors

Invariants throw `BusinessRuleViolationException` with a stable, kebab-case code (`billing.line-not-unbilled`, `numbering.issue-date-before-previous`, …). The API returns RFC 9457 problem details with `code` (and `issues` for draft validation); the UI localizes by code. Mapping: rule violation → 422, issued draft → 409, concurrency → 409, integrity failure → 500, unknown resource → 404.

## Money, tax and rounding

`decimal` everywhere, `Money` with currency, rounding policy in ADR-0006. Unit prices up to four decimals. Calculations happen only in the domain.

## Identity and time

UUIDv7 identifiers (time-ordered). `IClock` is the only time source: UTC instants (`timestamptz`), calendar dates as `date`. "Today" is evaluated in `Raccord:TimeZone` (default `Europe/Madrid`). The UI formats date-only values without time-zone shifts.

## Persistence and concurrency

EF Core, snake_case schema, explicit migrations, `xmin` optimistic concurrency, unique indexes as a second safety net, append-only triggers (ADR-0004).

## Immutability and integrity

Issued representation + SHA-256 verified on every read (ADR-0005); documents regenerable byte-for-byte; audit events append-only.

## Audit

`SharedKernel.Auditing.AuditEntry(occurredAt, actor, action, entityType, entityId, details)` is accepted through `IAuditAppender` and adapted by the current `IAuditTrail` to `Domain.Auditing.AuditEvent`, appended in the same transaction as the change. Current actions: `invoice-draft.created`, `invoice-draft.line-removed`, `invoice-draft.discarded`, `invoice.issued`, `invoice.document.stored`, `invoice.document.regenerated`, `issuer-profile.updated`.

## Security

- No authentication yet (MVP slice 2); the actor is recorded as `unauthenticated` so the gap is visible. Do not expose installations publicly until then.
- Secrets only through environment variables, user secrets or Key Vault; `.env` files are git-ignored; Azure uses managed identity.
- Containers run as non-root; health endpoints reveal only a status word.
- File-storage keys cannot escape the storage root.

## Observability

Aspire service defaults: OpenTelemetry logs, traces and metrics (ASP.NET Core, HTTP, runtime, Npgsql via the Aspire integration); OTLP export when `OTEL_EXPORTER_OTLP_ENDPOINT` is set; health endpoints excluded from tracing.

## Localization and accessibility

Typed message catalogs (es, en); `Intl` formatting; semantic HTML, labelled controls, skip link, focus moved to the page heading after navigation, live regions for status, `jsx-a11y` strict linting. Invoice PDFs use Spanish labels.

## Configuration

| Key | Default | Purpose |
| --- | --- | --- |
| `ConnectionStrings__raccord` | — | PostgreSQL |
| `Storage__Provider` | `FileSystem` | `FileSystem` or `AzureBlob` |
| `Storage__FileSystemRoot` | `data/documents` (`/var/lib/raccord/documents` in the image) | Document folder |
| `Storage__ContainerName` | `documents` | Blob container |
| `ConnectionStrings__documents` | — | Blob service URI or connection string |
| `Raccord__TimeZone` | `Europe/Madrid` | Organization time zone |
| `Raccord__SeedDemoData` | `false` | Fictional demo data on `migrate` |
| `OTEL_EXPORTER_OTLP_ENDPOINT` | — | Telemetry export |
