## Context

The current layout and the reasons for changing it are in ADR-0018 to ADR-0022. This document maps today's code onto the new modules and records the implementation choices those ADRs leave open.

## Goals / Non-Goals

**Goals:**

- Move the implemented billing slice into five module assemblies without changing behaviour.
- Make module boundaries fail the build.
- Keep finalization atomic across Invoicing, Projects and Compliance.

**Non-Goals:**

- Authentication and roles. That is ADR-0021, delivered with `jrn-wsp-set-up-workspace` in a separate change.
- New product behaviour, pagination, editing customers (debt D-1), or the VERI\*FACTU worker.
- Renaming `LegalParty` or `PartyDocument`, or changing the canonical issued-invoice representation (ADR-0005).

## Decisions

### Mapping current code to modules

| Current location                                                                           | Target                                                                                                                             |
| ------------------------------------------------------------------------------------------ | ---------------------------------------------------------------------------------------------------------------------------------- |
| `Domain/Common` (`Money`, `Currency`, `BusinessRuleViolationException`)                    | `Raccord.SharedKernel`                                                                                                             |
| `Domain/Parties` (`LegalParty`, `TaxId`, `PostalAddress`, `AddressFields`)                 | `Raccord.SharedKernel`. These are legal value objects used by Contacts, Workspace and Invoicing                                    |
| `Domain/Pricing`                                                                           | `Raccord.SharedKernel` while only Projects and Invoicing use it; it moves to Commercial when that module exists                    |
| `Domain/Customers`, `Application/Customers`                                                | `Raccord.Modules.Contacts`                                                                                                         |
| `Domain/Organization/IssuerProfile`, `Application/Organization`                            | `Raccord.Modules.Workspace` (company settings, BC-WORKSPACE)                                                                       |
| `Series` definitions and `/api/invoice-series`                                             | `Raccord.Modules.Workspace` (number series, BC-WORKSPACE), exposed through `Workspace.Contracts` (`IInvoiceSeriesCatalog`)         |
| `NumberSequence`, gapless allocation (ADR-0007)                                            | `Raccord.Modules.Invoicing`. Allocation must commit with the issued invoice                                                        |
| `Domain/Projects`, `Application/Projects`                                                  | `Raccord.Modules.Projects`; `Projects.Contracts` exposes `IBillableWork`                                                           |
| `Domain/Billing`, `Application/Invoicing`, the PDF renderer                                | `Raccord.Modules.Invoicing`                                                                                                        |
| `Domain/Auditing`, `Application/Auditing`, `Application/Compliance`, `ComplianceRecordRow` | `Raccord.Modules.Compliance`; it implements `IAuditAppender` (SharedKernel) and `Compliance.Contracts.IComplianceRecorder`         |
| `IDocumentStore` with the FileSystem and AzureBlob adapters (ADR-0009)                     | `Raccord.BuildingBlocks` (Storage), because Documents does not exist yet                                                           |
| `IClock`, `ICurrentActor`                                                                  | SharedKernel (`IClock`, `ICurrentUser`). Until ADR-0021 ships, `ICurrentUser` returns the same "unauthenticated" identity as today |
| `Raccord.Web` `Program.cs`, `migrate` command, SPA fallback                                | `src/root/Raccord.Web`; registers modules explicitly                                                                               |

Contract dependencies between modules: Invoicing → Projects, Contacts, Workspace and Compliance contracts. Projects → Contacts (customer existence). This graph is acyclic.

### Shared-transaction unit of work

- A scoped `NpgsqlConnection` comes from the Aspire-provided data source. Each module context is configured with `UseNpgsql(connection)`.
- `IUnitOfWork.ExecuteAsync(...)` opens the transaction and enlists the contexts the handler resolved. They are tracked by a scoped `ParticipatingContexts` registry that each context adds itself to on first use. It saves them in registration order, then commits.
- The existing conflict mapping (`DbUpdateConcurrencyException` and unique violation → conflict) is kept.
- **Rejected:** a `TransactionScope` with ambient enlistment. Npgsql supports it, but it hides the transaction boundary and complicates async flows.

### Schema move

- Each module's first migration in its new history table is generated against an empty schema and then edited into `ALTER TABLE public.<t> SET SCHEMA <module>` statements. It is guarded by `IF EXISTS`, so that new databases created from scratch get the plain `CREATE TABLE` path.
- **Rejected:** dropping and recreating the tables, because it loses data in installations that already exist.
- The trigger function `raccord_reject_mutation()` stays in `public` as a shared database utility, created idempotently by the Compliance module's first migration and reused by the Invoicing triggers.

### Sender pipeline

The pipeline runs: validation (FluentValidation; failures → `ErrorOr` validation errors), then the domain bridge (catches `BusinessRuleViolationException`, `InvoiceValidationException` and `ConcurrencyConflictException` and turns them into rule or conflict errors with the same codes), then the handler. There is no reflection-based behaviour discovery; the pipeline is composed in code.

## Risks / Trade-offs

- **A big-bang move hides regressions.** Mitigation: move one module per task, keep the full test suite green after each task, and compare the OpenAPI output in CI (contract-drift gate).
- **Lost cross-module foreign keys** (ADR-0019). Mitigation: contract calls validate referenced ids at write time; web tests cover the "unknown customer or project" paths.
- **The schema move on existing databases.** Mitigation: an integration test migrates a database built from the pre-change migrations, with data, and verifies the row counts and the triggers.
- **E2E fixture paths change** (`Projects.Raccord_Aspire`). Mitigation: update the AppHost project reference in the same task as the host move.

## Migration Plan

1. Merge with all gates green.
2. Deploy. The `migrate` resource (Compose, Aspire, Container Apps) applies the schema moves before the web container starts, as today.
3. Rollback: restore the pre-deploy backup (`deploy/scripts/restore.sh`). The schema moves are not reversed automatically.

## Open Questions

- Should `Pricing` live in the SharedKernel until Commercial exists, or move to Projects? Default: SharedKernel.
- Should the number series definitions stay in Invoicing for now, to avoid one cross-module contract? Default: follow BC-WORKSPACE and put them in Workspace.
