---
name: module-assemblies-named-after-bounded-contexts
description: Each bounded context that has a real seam becomes its own module assembly, with a contracts assembly only when another module consumes it and boundaries enforced at compile time.
status: Accepted
date: 2026-10-12
deciders: ["Juan G. Carmona"]
tags: ["modular-monolith", "modules", "boundaries", "analyzers"]
---

# ADR-0018: Split modules into assemblies named after the product's bounded contexts

## Context

ADR-0001 chose a modular monolith. ADR-0003 implemented it as six layered projects (`Domain`, `Contracts`, `Application`, `Infrastructure`, `Web`, `ServiceDefaults`). Modules are only namespaces inside those projects, and "module boundaries inside a project are enforced by review and namespaces, not by assemblies".

The architecture tests enforce the dependency direction between layers. Nothing enforces boundaries between modules: any use case can reach any aggregate or repository. Use cases are registered by hand in `src/Raccord.Web/Program.cs`, and every route lives in one file, `src/Raccord.Web/Api/Endpoints.cs`.

The product model (`docs/product/model/domain/bounded-contexts/`) defines ten bounded contexts. Only billing has code today, and the code namespaces (Customers, Parties, Organization, Billing, Auditing…) do not match the contexts one to one. Each new journey adds a context. Restructuring is cheapest now, while only one vertical slice exists.

Two sibling .NET 10 modular monoliths by the same team have run module assemblies successfully. Both enforce the boundaries with Roslyn analyzers and architecture tests. One of them also derives module names from its product model's bounded contexts.

## Decision

We will make every module its own assembly, named after the product model's bounded context it implements.

- **Projects:** `src/modules/<Module>/Raccord.Modules.<Module>`, plus `Raccord.Modules.<Module>.Contracts` only when another module actually consumes something.
- **Initial modules:**
    - **Workspace** (BC-WORKSPACE): users, roles, settings, number series.
    - **Contacts** (BC-CONTACTS, which replaces BC-PARTIES through Product Change CHG-RENAME-PARTIES-TO-CONTACTS-001): customers, the issuer's legal identity, later suppliers and venues.
    - **Projects** (BC-PROJECTS).
    - **Invoicing** (BC-INVOICING).
    - **Compliance** (BC-COMPLIANCE): audit events and compliance records.

    Commercial, Equipment, Warehouse, Crew and Documents become modules when their first journey is implemented.

- **Seam test.** A capability becomes a module only if it maps to a product-model bounded context and has an independent lifecycle, its own data ownership, or an event-driven boundary. Anything else is a feature slice inside an existing module. A new module requires an ADR.
- **Module interior:**
    - `Domain/` holds aggregates and value objects. They stay free of frameworks: no EF Core, ASP.NET, ErrorOr or FluentValidation references.
    - `Features/<Feature>/<UseCase>.cs` holds the use cases, in the shape set by ADR-0022.
    - `Persistence/` holds the DbContext, configurations and migrations (see ADR-0019).
    - `<Module>Module.cs` holds registration.

    Calculations stay in the domain, as `architecture-rules.md` requires.

- **Shared code:**
    - `src/shared/Raccord.SharedKernel` references no other Raccord assembly. It holds the value objects several contexts use: `Money`, `Currency`, `TaxId`, `PostalAddress`, `LegalParty`. It also holds the cross-cutting ports `IClock`, `ICurrentUser` and `IAuditAppender`.
    - `src/shared/Raccord.BuildingBlocks` holds the CQS sender, result mapping, permission policies and the unit of work. It references only the SharedKernel.
- **Hosts:**
    - `src/root/Raccord.Web` is the only composition root. It registers each module explicitly with `builder.Add<Module>Module()` and `api.Map<Module>Endpoints()`. There is no assembly scanning, so the composition stays readable in one file.
    - `Raccord.Aspire` and `Raccord.ServiceDefaults` keep their roles.
- **Wire contracts.** The current `Raccord.Contracts` project holds HTTP DTOs. It is dissolved: request and response types live with their slice. The OpenAPI document generated from the endpoints stays the frontend contract (ADR-0011), and the contract-drift gate stays.
- **Enforcement at compile time.** We will add `src/shared/Raccord.ArchitectureAnalyzers`, attached to every module project, with rules at `error`:
    - a module may reference another module only through its `.Contracts` assembly;
    - commands and queries are `internal`;
    - handlers are `internal sealed`;
    - persistence entities are `internal`;
    - public members do not expose domain or persistence types;
    - no MVC controllers.

    The module list comes from MSBuild properties (`CompilerVisibleProperty`), not from code. A `BeforeBuild` target fails on any `ProjectReference` to another module's implementation or to a host. `Raccord.Architecture.Tests` keeps the dependency-direction rules, rewritten for the new graph. The analyzers get their own test project.

## Consequences

- **Positive:**
    - Module boundaries become compile errors instead of review comments.
    - Each new context has an obvious home and template.
    - Module names match the product model, so the language in the model, the code and the UI lines up.
    - The billing slice is the only code that has to move.
- **Negative:**
    - More projects in the solution.
    - Deciding whether something is a module or a slice needs judgement.
    - A wrong module boundary is costlier to undo than a wrong namespace.
    - Moving the billing slice touches almost every production file once.
    - Project-level rules such as "Domain references only the BCL" become namespace-level architecture tests inside each module.
- **Neutral:**
    - This record supersedes ADR-0003.
    - `docs/architecture/architecture-rules.md` and arc42 §4, §5 and §8 must be updated with the restructure.
    - The `Raccord.Domain.Parties` namespace becomes part of the SharedKernel and the Contacts module, following Product Change CHG-RENAME-PARTIES-TO-CONTACTS-001 (BC-CONTACTS).

## Alternatives considered

- **Keep ADR-0003's layered projects with module namespaces:** rejected because nothing enforces boundaries between modules, and ten contexts in six projects would erode them.
- **Layered projects per module (`Raccord.<Module>.Domain`, `.Application`, `.Infrastructure`):** rejected because it gives up to four projects per module for little gain. A namespace test keeps the domain free of frameworks.
- **Discover modules by scanning assemblies through an `IModule` interface:** rejected because explicit registration is easier to read and debug, and Raccord will have about ten modules, not hundreds.
- **Create all ten contexts as modules now:** rejected because empty modules invent boundaries no journey has tested yet.

## References

- Supersedes: ADR-0003
- Related: ADR-0001, ADR-0019, ADR-0020, ADR-0021, ADR-0022
- Product model: `docs/product/model/domain/bounded-contexts/`
- Product Change: CHG-RENAME-PARTIES-TO-CONTACTS-001 (draft), which adds BC-CONTACTS in place of BC-PARTIES: `docs/product/changes/active/chg-rename-parties-to-contacts-001/change.md`
