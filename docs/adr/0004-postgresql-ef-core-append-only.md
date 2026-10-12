---
name: postgresql-ef-core-append-only
description: Raccord uses PostgreSQL as its only database through EF Core, and a database trigger makes issued invoices and audit events append-only.
status: Accepted
date: 2026-10-06
deciders: ["Juan G. Carmona"]
tags: ["persistence", "postgresql", "ef-core", "immutability", "migrations"]
---

# ADR-0004: Use PostgreSQL with EF Core and enforce append-only financial tables in the database

## Context

Raccord is its own system of record. Issued invoices and audit events must not change, even through manual SQL or a bug in the application.

## Decision

We will use PostgreSQL with EF Core and let the database enforce the append-only financial tables.

- PostgreSQL is the only database. EF Core 10 with Npgsql, registered through the Aspire client integration (`Aspire.Npgsql.EntityFrameworkCore.PostgreSQL`) for health checks, retries and telemetry.
- Domain entities are mapped directly (private setters, backing fields, owned collections); value objects are reconstructed from primitive columns to keep mapping simple.
- Optimistic concurrency uses PostgreSQL's `xmin` system column on projects, billable lines, drafts, issuer profile and number sequences. Conflicts become `ConcurrencyConflictException` → HTTP 409.
- A trigger function `raccord_reject_mutation()` rejects `UPDATE`, `DELETE` and `TRUNCATE` on `issued_invoices` and `audit_events`, and `DELETE`/`TRUNCATE` on `compliance_records` (whose status may change).
- Snake-case naming is applied by a small model convention instead of a naming-convention package.
- Migrations are generated with a repository-local `dotnet-ef` tool and applied by an explicit `migrate` command, never implicitly at web start-up. CI fails if the model has changes without a migration.

## Consequences

- **Positive:**
  - Issued invoices and audit events cannot be changed through manual SQL or an application bug.
- **Negative:**
  - A database superuser can still disable triggers; tamper detection (ADR-0005) covers that case.
- **Neutral:**
  - Restores must recreate the schema (pg_restore of a full dump does), so the triggers come back with the data — verified in the restore rehearsal.

## Alternatives considered

- **Support several databases:** rejected because the decision relies on PostgreSQL-specific features (`xmin` concurrency, trigger functions).
- **Enforce immutability only in the application:** rejected because issued invoices and audit events must not change even through manual SQL or a bug in the application.
- **A naming-convention package:** rejected in favour of a small model convention for snake-case naming.
- **Apply migrations implicitly at web start-up:** rejected; migrations are applied by an explicit `migrate` command.

## References

- Related: ADR-0005
