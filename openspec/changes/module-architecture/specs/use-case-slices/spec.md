## Purpose

Every use case reads the same way: one file from route to handler, one result type, and one mapping to problem details, matching the sibling repositories' house style.

## ADDED Requirements

### Requirement: One file per use case

Each use case SHALL live in `src/modules/<Module>/Raccord.Modules.<Module>/Features/<Feature>/<UseCase>.cs` and contain its route mapping, request and response types, command or query, validator and `internal sealed` handler. Each module SHALL list its slices explicitly in `Map<Module>Endpoints()`.

#### Scenario: Locating a use case

- **WHEN** a contributor looks for the code behind `POST /api/invoice-drafts/{id}/finalize`
- **THEN** a single file under `Raccord.Modules.Invoicing/Features/` contains its route, validation and handler

### Requirement: Handlers return result errors through the sender

Handlers SHALL return `ErrorOr<T>` and be invoked through `ISender` from `Raccord.BuildingBlocks`. The sender SHALL run FluentValidation before the handler, and SHALL turn `BusinessRuleViolationException` and concurrency conflicts thrown below the handler into `ErrorOr` errors that carry the original code.

#### Scenario: Invalid input

- **WHEN** a customer is created with an empty legal name
- **THEN** the response is 422 problem details with `code` and per-field `issues`, the same as before this change

#### Scenario: Domain rule violation

- **WHEN** finalizing a draft that has no lines
- **THEN** the response is 422 problem details with the domain's existing rule code

### Requirement: Problem details keep their shape

Every error response SHALL be RFC 9457 problem details with `type` `urn:raccord:problem:<code>` and a `code` extension. Status codes SHALL be unchanged for every existing error code. Only defects and `issued-invoice.integrity-violation` SHALL reach the global exception handler as 500.

#### Scenario: Unchanged error contract

- **WHEN** the existing tests in `tests/Raccord.Web.Tests` (`InvoicingSliceTests`, `InvoicingRulesTests`) run after the change
- **THEN** they pass without changes to their expected status codes or problem codes

#### Scenario: Unchanged API contract

- **WHEN** the frontend `api:generate` script regenerates `openapi/raccord.json`
- **THEN** routes, request schemas and response schemas are unchanged
