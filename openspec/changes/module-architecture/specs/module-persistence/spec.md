## Purpose

Each module owns its tables, and an operation that spans several modules still commits or rolls back as a whole, so the legal guarantees of invoicing survive the split.

## ADDED Requirements

### Requirement: Each module owns a schema

Each module SHALL have one DbContext mapping only its own tables, in a PostgreSQL schema named after the module (`workspace`, `contacts`, `projects`, `invoicing`, `compliance`), with its own `__EFMigrationsHistory` table in that schema and migrations under `Persistence/Migrations`.

#### Scenario: Fresh database

- **WHEN** `migrate` runs against an empty database
- **THEN** every module's tables exist in that module's schema and none in `public`

### Requirement: Cross-module writes share one transaction

`IUnitOfWork` SHALL run the use case's writes to every participating module context on one connection, in one transaction, and commit once. A failure in any context SHALL roll back all of them.

#### Scenario: Finalization is atomic

- **WHEN** an invoice draft is finalized
- **THEN** the number allocation, the issued invoice, the invoiced billable lines, the audit event and the pending compliance record are all committed together

#### Scenario: Failure rolls back every module

- **WHEN** saving the compliance record fails during finalization
- **THEN** no issued invoice, number allocation, billable-line change or audit event is persisted

#### Scenario: Conflicts keep their mapping

- **WHEN** two finalizations race for the same number sequence
- **THEN** one succeeds and the other yields a 409 problem with the same code as before this change

### Requirement: Existing data moves without loss

Migrating a database created by the pre-change migrations SHALL move every table into its module schema, preserving all rows.

#### Scenario: Upgrade with data

- **WHEN** a database with customers, projects, drafts, issued invoices, audit events and compliance records is migrated
- **THEN** the row counts per table are unchanged and every issued invoice still passes SHA-256 verification on read

### Requirement: Append-only guarantees are preserved

`UPDATE` and `DELETE` on issued invoices and audit events, and `DELETE` on compliance records, SHALL still be rejected by database triggers after the move.

#### Scenario: Mutation attempt after upgrade

- **WHEN** a raw `UPDATE` targets a row of `invoicing.issued_invoices`
- **THEN** PostgreSQL rejects it through `raccord_reject_mutation()`

### Requirement: No cross-module foreign keys

A module SHALL reference another module's records only by identifier, and SHALL validate the identifier through the owning module's contract at write time.

#### Scenario: Unknown customer

- **WHEN** a project is created for a customer id that Contacts does not know
- **THEN** the request fails with the same 404 or 422 problem code as before this change
