---
name: single-file-use-case-slices-with-result-errors
description: Each use case is one file holding its endpoint, request, validator and handler, dispatched through an in-house CQS sender that returns ErrorOr results and validates with FluentValidation.
status: Accepted
date: 2026-10-12
deciders: ["Juan G. Carmona"]
tags: ["application", "api", "validation", "errors", "conventions"]
---

# ADR-0022: Write each use case as a single-file slice returning result errors

## Context

Raccord's application layer uses use-case classes that group many operations (`CustomerDirectory`, `ProjectWorkspace`, `InvoicingWorkflow`, `OrganizationSettings`, `AuditLog`). Every route is in one file, `src/Raccord.Web/Api/Endpoints.cs`. Business-rule failures are exceptions (`BusinessRuleViolationException` with a stable kebab-case code), mapped to RFC 9457 problem details by `ApiExceptionHandler` (arc42 §8).

ADR-0018 moves code into module assemblies, so each module needs to register its own endpoints and use cases. The same maintainer works on two sibling .NET modular monoliths, which have converged on the same shape:

- one file per use case, holding the endpoint, request, command or query, validator and handler;
- an in-house CQS sender with a validation step;
- `ErrorOr<T>` results mapped to problem details;
- FluentValidation.

Their risk logs note that contributors and coding agents copy whichever pattern they find. One shared shape across these repositories reduces that drift.

## Decision

We will write every use case as a single-file slice, dispatched through an in-house sender. Handlers return `ErrorOr<T>` and input is validated with FluentValidation.

- **Slice shape.** `Features/<Feature>/<UseCase>.cs` contains:
    - a static `Map(IEndpointRouteBuilder)` that declares the Minimal API route, its permission (ADR-0021) and OpenAPI metadata;
    - nested `Request` and `Response` records;
    - the `Command` or `Query`;
    - a FluentValidation `Validator`;
    - an `internal sealed` `Handler`.

    Each module lists its slices explicitly in `Map<Module>Endpoints()` under the `/api` group.

- **Sender.** `ISender` lives in `Raccord.BuildingBlocks`. Its pipeline runs validation, then the handler, then logging. There is no MediatR, no assembly-scanning library and no caching behaviour. Handlers and validators are registered per module assembly by a small in-house helper.
- **Errors:**
    - Handlers return `ErrorOr<T>`. `ApiResults` maps the error types: validation → 422 with per-field issues, rule → 422, conflict → 409, not found → 404, forbidden → 403, unauthenticated → 401.
    - Every response is RFC 9457 problem details and keeps the existing shape: `type` `urn:raccord:problem:<code>`, the `code` extension, and the `issues` extension for validation. API clients and the frontend see no change.
    - The global exception handler only handles defects (500), plus the existing integrity violation (`issued-invoice.integrity-violation`, ADR-0005).
- **Domain stays framework-free.** Aggregates and value objects keep protecting their invariants by throwing `BusinessRuleViolationException` with a stable code, because the domain must not depend on ErrorOr.
    - One sender step catches `BusinessRuleViolationException` and turns it into an `ErrorOr` rule error with the same code.
    - So there is one error path from handler to HTTP, and domain code needs no rewrite.
    - Concurrency conflicts from the unit of work (ADR-0019) become conflict errors the same way.
- **Validation split.** Validators check the shape of the input: presence, length, format. Business rules stay in the domain. A validator never duplicates a domain rule.

## Consequences

- **Positive:**
    - One place to read and change a use case.
    - The same structure as the sibling repositories, for people and agents moving between them.
    - Endpoints are registered per module, as ADR-0018 requires.
    - Expected failures become explicit return values instead of control flow through exceptions.
- **Negative:**
    - Adds two production dependencies, `ErrorOr` and `FluentValidation`, justified here.
    - The bridge from domain exceptions to errors means two error mechanisms remain, separated by the domain boundary.
    - Large use-case classes must be split into several slices during the restructure.
    - Single files grow long for complex use cases. A slice may then move its handler into a sibling file in the same folder.
- **Neutral:**
    - `Endpoints.cs` and `ApiExceptionHandler.cs` are replaced.
    - The OpenAPI document must not change unintentionally. The contract-drift gate checks this.
    - Arc42 §5 and §8 must describe the slice shape.

## Alternatives considered

- **Keep use-case classes and exceptions mapped to problem details:** rejected because it differs from the sibling repositories and still needs per-module endpoint registration.
- **Single-file slices that keep exceptions, without ErrorOr or FluentValidation:** rejected because it avoids two dependencies but loses the shared result and validation shape, which is the reason for the change.
- **MediatR for dispatch:** rejected because a small in-house sender covers the one validation step needed, and it avoids a dependency with a commercial licence.
- **ErrorOr inside the domain:** rejected because the domain must stay free of frameworks and packages (`architecture-rules.md`).

## References

- Related: ADR-0005, ADR-0011, ADR-0014, ADR-0018, ADR-0019, ADR-0021
- Error contract: `docs/architecture/arc42/08-cross-cutting-concepts.md`
