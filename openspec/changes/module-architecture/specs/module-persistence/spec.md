<!-- pdac-scope: cited -->

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

<!-- pdac:cite id="BR-COMPLIANCE-OBLIGATION" digest="sha256:dab51b5c3c98ca25c40abec4a8e477fdc172ac986788fd2233eb2560f14a4757" -->

<!-- pdac:cite id="BR-GAPLESS-NUMBERING" digest="sha256:1d6404217e241d191188f8a20bdaedda2204e10548c344df7d46793d1c204adb" -->

<!-- pdac:cite id="BR-AUDITED-ACTIONS" digest="sha256:33a6a76f11ff3ed15dcbd22798ea6b65a05d65d4130cb17c7751eb516266e54c" -->

<!-- pdac:cite id="QR-CONCURRENCY-CONSISTENCY" digest="sha256:320945ebf06be10006380b4e183377f48569c75224692b903f3cf093887736a2" -->

<!-- pdac:cite id="FR-COMPLIANCE-001" digest="sha256:a53549df92d28a28435890b7663e0a9c92ec126ff0988b6cb103c39d5aa050d8" -->

<!-- pdac:cite id="FR-AUDIT-001" digest="sha256:7646b5398f69286e802b2a3175edcf8b18cfc3b742edc9d3c2a44476c6159417" -->

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

<!-- pdac:cite id="FR-INVOICE-007" digest="sha256:a385379cb621363063c9a6659eb650b01e228846b5e8bbdd17ed53b0901784f1" -->

#### Scenario: Upgrade with data

- **WHEN** a database with customers, projects, drafts, issued invoices, audit events and compliance records is migrated
- **THEN** the row counts per table are unchanged and every issued invoice still passes SHA-256 verification on read

### Requirement: Append-only guarantees are preserved

`UPDATE` and `DELETE` on issued invoices and audit events, and `DELETE` on compliance records, SHALL still be rejected by database triggers after the move.

<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:8138f31518261c618e3fef1dc93d5deaef6f55a4c3b2bfd41dfc0c5a4b75aecc" -->

<!-- pdac:cite id="FR-INVOICE-006" digest="sha256:fee328e3745e8275c00914dd830f3c7cbd3a1aec267f18ed117ef24ddb8e2466" -->

<!-- pdac:cite id="FR-AUDIT-001" digest="sha256:7646b5398f69286e802b2a3175edcf8b18cfc3b742edc9d3c2a44476c6159417" -->

<!-- pdac:cite id="FR-COMPLIANCE-001" digest="sha256:a53549df92d28a28435890b7663e0a9c92ec126ff0988b6cb103c39d5aa050d8" -->

#### Scenario: Mutation attempt after upgrade

- **WHEN** a raw `UPDATE` targets a row of `invoicing.issued_invoices`
- **THEN** PostgreSQL rejects it through `raccord_reject_mutation()`

### Requirement: No cross-module foreign keys

A module SHALL reference another module's records only by identifier, and SHALL validate the identifier through the owning module's contract at write time.

<!-- pdac:cite id="UC-CREATE-PROJECT" digest="sha256:b8bbb65b0f50ceeb882d7f30ec3219623b5f1242292f19240785f442960b755f" -->

#### Scenario: Unknown customer

- **WHEN** a project is created for a customer id that Contacts does not know
- **THEN** the request fails with the same 404 or 422 problem code as before this change
