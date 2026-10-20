<!-- pdac-scope: cited -->

## Why

Raccord's modular monolith exists only as namespaces inside six layered projects (ADR-0003). Nothing enforces boundaries between modules: one `RaccordDbContext` maps every table, use cases are registered by hand in `src/Raccord.Web/Program.cs`, and every route lives in `src/Raccord.Web/Api/Endpoints.cs`.

The product model defines ten bounded contexts (`docs/product/model/domain/bounded-contexts/`), and the next journey (`jrn-wsp-set-up-workspace`) adds identity and roles. Restructuring is cheapest now, while billing is the only implemented slice. The decisions are accepted in ADR-0018 to ADR-0022 (`docs/adr/`). This change implements ADR-0018, ADR-0019, ADR-0020 and ADR-0022. Authentication (ADR-0021) ships with the workspace journey in its own change.

<!-- pdac:cite id="BC-WORKSPACE" digest="sha256:013841dd37789974afef821eb2c071a85acaa72c083abd2285b7c406db95f8a5" -->

<!-- pdac:cite id="BC-CONTACTS" digest="sha256:bdadd84cb901027c8630d88c714ba8c52bcecdf383345d35a2631c7fa8b69c85" -->

<!-- pdac:cite id="BC-PROJECTS" digest="sha256:11cadbc8906365210716689609d66738e29fc576822aa2e8e7a925a8bae0933e" -->

<!-- pdac:cite id="BC-INVOICING" digest="sha256:386343a8a3613a73a2ab60d1b5767cbe7d880fb8ded0d2d1a7c5dee68fa2a567" -->

<!-- pdac:cite id="BC-COMPLIANCE" digest="sha256:92fbac72cbcb8e640f0c87ff7da3629698b6bb3ee7cf5bdf742db249ca898dc0" -->

<!-- pdac:cite id="JRN-BILL-FINISHED-PROJECT" digest="sha256:b9530a2b0a9aac0f9cb3a5a9608494d08be1fc1190f395887fc7bb7fb30df28e" -->

## What Changes

- **Module assemblies** (ADR-0018). Five modules, each under `src/modules/<Module>/Raccord.Modules.<Module>`, with a `.Contracts` assembly only when another module consumes it:
  - Workspace
  - Contacts
  - Projects
  - Invoicing
  - Compliance

    Shared code moves to `src/shared/Raccord.SharedKernel` and `src/shared/Raccord.BuildingBlocks`. The hosts move to `src/root/`. `Raccord.Domain`, `Raccord.Application`, `Raccord.Contracts` and `Raccord.Infrastructure` are removed.

- **Boundary enforcement** (ADR-0018). A new `src/shared/Raccord.ArchitectureAnalyzers` project with analyzer tests, an MSBuild reference guard, and architecture tests rewritten for the module graph.
- **Persistence per module** (ADR-0019):
  - one DbContext, schema and migrations history per module;
  - a unit of work that enlists every participating context in one transaction on one shared connection;
  - schema-move migrations that keep existing data and the append-only triggers.
- **Communication between modules** (ADR-0020): only through `.Contracts` interfaces in the caller's transaction. Audit goes through `IAuditAppender` in the SharedKernel. There are no events and no outbox.
- **Single-file slices** (ADR-0022):
  - an in-house `ISender` with validation and domain-exception bridging;
  - `ErrorOr` results, FluentValidation, and endpoints registered per module;
  - **two new production dependencies**, ErrorOr and FluentValidation, justified in ADR-0022.
- **Docs:**
  - ADR-0003 marked superseded by ADR-0018;
  - ADR-0018 to ADR-0020 and ADR-0022 accepted;
  - `docs/architecture/architecture-rules.md` and arc42 §4, §5, §6, §8, §9 and §11 updated;
  - `openspec/config.yaml` context updated to the new layout.

Behaviour is preserved:

- **HTTP API:** routes, payloads and problem details are unchanged. The OpenAPI document must be byte-identical or differ only in operation ids and tags, reviewed in the PR.
- **Frontend:** no changes.
- **Database:** **BREAKING** for direct SQL consumers only. Tables move from `public` to the module schemas; the data and the append-only triggers are preserved.
- **CI:** gates unchanged; new analyzer tests added.
- **Deployed environments:** the `migrate` command applies the schema moves.

## Capabilities

### New Capabilities

- `module-boundaries`: module assemblies, contracts-only references between modules, and compile-time and test-time enforcement.
- `module-persistence`: schemas owned by each module, the shared transaction, migrations, and preservation of the append-only guarantees.
- `use-case-slices`: the single-file slice shape, the sender pipeline, and the error mapping to problem details.

### Modified Capabilities

None. `openspec/specs/` has no archived capabilities yet.

## Impact

- **Product model:** no new product scope. The module name "Contacts" follows Product Change CHG-RENAME-PARTIES-TO-CONTACTS-001 (BC-PARTIES → BC-CONTACTS, `docs/product/changes/completed/chg-rename-parties-to-contacts-001/change.md`), applied on 2026-10-12.
- **Code:** every production C# file moves or is rewritten into slices. Domain logic, money rounding, numbering and the canonical invoice representation are moved without semantic changes.
- **Tests:**
  - Existing domain, web and end-to-end tests must pass unchanged apart from namespaces.
  - Web tests are the behavioural safety net and are not weakened.
- **Depends on:** ADR-0018, ADR-0019, ADR-0020 and ADR-0022 (accepted) and the Contacts Product Change above.
