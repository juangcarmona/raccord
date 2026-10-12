---
name: module-owned-schemas-with-a-shared-transaction
description: Each module owns a DbContext, a PostgreSQL schema and its migrations, and a unit of work enlists every participating context in one transaction on one connection.
status: Accepted
date: 2026-10-12
deciders: ["Juan G. Carmona"]
tags: ["persistence", "postgresql", "ef-core", "transactions", "modules"]
---

# ADR-0019: Give each module its own schema and DbContext, sharing one transaction

## Context

ADR-0004 chose PostgreSQL with EF Core 10. Today one `RaccordDbContext` maps every table in the default schema: customers, projects, invoice drafts, series, number sequences, issuer profiles, issued invoices, audit events and compliance records. `UnitOfWork` calls `SaveChangesAsync` on that one context, so a use case is atomic by construction.

ADR-0018 splits the code into module assemblies. A shared DbContext would let any module query any table and would undo the boundary. Some business operations still have to span modules atomically. Finalizing an invoice (runtime scenario R-2) does all of the following in one transaction:

- allocates a gapless number (ADR-0007);
- writes the append-only issued invoice (ADR-0004, ADR-0005);
- marks the project's billable lines as invoiced;
- appends an audit event;
- creates the pending compliance record (ADR-0010).

A gap in numbering, or an issued invoice without its compliance record, is a legal defect, not a retryable inconsistency.

## Decision

We will give every module its own DbContext and PostgreSQL schema in the one Raccord database, and make cross-module writes atomic through a shared connection and transaction.

- **One DbContext per module.** Each module has its own context with `HasDefaultSchema("<module>")`: `workspace`, `contacts`, `projects`, `invoicing` and `compliance`. Each has its own migrations history table in its schema, its migrations in `Persistence/Migrations`, and a design-time factory. No context maps another module's tables.
- **One connection, one transaction:**
    - All module contexts in a request share one scoped `NpgsqlConnection`.
    - `IUnitOfWork` (BuildingBlocks) opens one transaction and enlists every context the use case touched (`Database.UseTransactionAsync`). It saves each context, commits once, and maps concurrency and unique violations to conflicts as today.
    - Isolation stays at PostgreSQL's default.
    - Numbering keeps its current guarantees: the sequence row is protected by `xmin`, and conflicts surface as 409.
    - The Npgsql retrying execution strategy, when enabled, wraps the whole unit of work, not single saves.
- **No foreign keys across modules.** Cross-module references are plain identifiers, because EF Core cannot model a foreign key into another context. Where the legal record needs the referenced data, it snapshots it, as issued invoices already snapshot party identities (ADR-0005). The owning module validates references through `.Contracts` at write time (ADR-0020).
- **Migrations:**
    - The `migrate` command (ADR-0004) applies every module's migrations in dependency order. Workspace and Compliance come first.
    - The move from the current public-schema tables is one migration per module that runs `ALTER TABLE … SET SCHEMA` and keeps the data.
    - The append-only triggers and the shared `raccord_reject_mutation()` function move to the owning schema or stay in `public`. Either way they must keep rejecting `UPDATE`/`DELETE` on `issued_invoices` and `audit_events`. A test proves this after the move.
- **Tests:** Testcontainers integration tests run all module migrations into one database per test, as today (ADR-0014).

## Consequences

- **Positive:**
    - Modules cannot read each other's tables through EF.
    - Each module's schema evolves on its own.
    - Finalizing an invoice stays a single atomic transaction.
    - The database layout shows the module map.
- **Negative:**
    - Losing cross-module foreign keys moves referential integrity from the database to the application. A billable line can, in principle, reference a project id that never existed.
    - A change that spans two modules needs two migrations.
    - The shared-connection unit of work is new infrastructure that must be tested, including rollback when one context fails.
    - Reporting queries across schemas need explicit read models.
- **Neutral:**
    - Existing installations need the schema-move migration. Installations created from scratch get the new layout directly.
    - Arc42 §5, §6 (R-2, R-4) and §8 must describe the per-module persistence.

## Alternatives considered

- **Keep one shared DbContext:** rejected because it makes every table reachable from every module and defeats ADR-0018.
- **One database per module:** rejected because finalizing an invoice would need a distributed transaction or a saga for a legal operation that must be atomic.
- **Eventual consistency between modules through an outbox:** rejected for finalization, because a committed invoice without its compliance record or numbering is not acceptable even briefly (see ADR-0020).
- **Serializable isolation for every unit of work:** rejected as a blanket default. Numbering is already protected by `xmin` and uniqueness, and serializable everywhere would add retries without a demonstrated need.

## References

- Related: ADR-0004, ADR-0005, ADR-0007, ADR-0010, ADR-0018, ADR-0020
- Runtime scenario R-2 in `docs/architecture/arc42/06-runtime-view.md`
