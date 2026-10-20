<!-- pdac-scope: cited -->
<!-- pdac:cite id="BC-EQUIPMENT" digest="sha256:0e324b394e1d89bacbfa13bc9f312f0592c710fe99f8d9c515dc9c999025bf82" -->
<!-- pdac:cite id="BC-WAREHOUSE" digest="sha256:2604b7b7573cfde1759e4b8c0cb9c5c76a188520fac77862a06344f8d08ca2ff" -->

# Design: Equipment module boundary

Work item: [#305](https://github.com/juangcarmona/raccord/issues/305). This plan changes ownership and dependency direction only. It must not implement the new product behavior in #306–#314.

## Current state observed

- Equipment entities are in `src/Raccord.Domain/Equipment/`; `EquipmentCatalog` and Equipment EF mappings are in `src/Raccord.Infrastructure/Persistence/`; equipment routes are in `src/Raccord.Web/Api/Endpoints.cs`.
- `RaccordDbContext` owns Equipment tables in the shared persistence project. Projects' `Raccord.Modules.Projects.Contracts/IEquipmentStock.cs` is implemented by `EquipmentCatalog` and consumed by `ProjectWorkspace`.
- Existing module implementations and contracts live under `src/modules/<Context>/Raccord.Modules.<Context>[.Contracts]`; the root Web project is the explicit composition root.
- Architecture tests currently enumerate exposed modules and project-reference direction. The current equipment and warehouse browser evidence is primarily API-mode E2E, and must retain its existing tags and report grouping.

## Proposed architecture decision

Create ADR-0030, **Equipment owns its catalog and stock records behind a module contract**, using the ADR template and status `Proposed`. It records the durable ownership choice and rejected alternatives. It cannot be marked Accepted by this change; Owner acceptance is a hard prerequisite to code that depends on the new boundary. Existing ADR-0018, ADR-0019 and ADR-0020 govern module shape, schema/transaction ownership and synchronous module contracts.

The proposal assigns catalog items, lifecycle, stock movements, counts and existing equipment API operations to Equipment. Projects requests stock operations through the Equipment Contracts assembly. The current Warehouse flows also call the Equipment contract through the root composition, without creating a Warehouse module in this issue. Warehouse module extraction and new planning/booking workflows remain scoped to #376 and #310–#314.

## Extraction sequence

1. Add `Raccord.Modules.Equipment` and `Raccord.Modules.Equipment.Contracts` using the established module project layout. Keep application use cases and persistence entities internal; expose only consumer ports and transport types required at module seams.
2. Move the stock interface and the request/response DTOs to the Equipment Contracts assembly. Update Projects to reference that contract. The root host composes the Equipment implementation and injects its port into Projects and existing Warehouse consumers; no implementation project may reference another implementation project.
3. Move Equipment aggregates, catalog use cases, EF configurations and tables behind Equipment-owned persistence. Register the Equipment DbContext and endpoints from `EquipmentModule`. Keep the public `/equipment` routes and their OpenAPI shapes stable.
4. Add the Equipment schema and a forward migration that preserves existing item, movement, count, lifecycle and scan-code values and relationships. Keep the existing migration chain usable for installations upgrading from the current schema. Do not drop or recreate customer data as a shortcut.
5. Extend executable project-reference rules and architecture tests to cover Equipment implementation, Equipment.Contracts consumers and schema ownership. Retain the rule that module contracts may reference only SharedKernel and other module contracts.
6. Update architecture and operations documentation in the same change. Do not add an Equipment row to arc42 section 9 while ADR-0030 is Proposed; index it after acceptance in the implementation PR.

## Compatibility and risk controls

- Treat route, DTO, OpenAPI, permission, audit and existing E2E changes as regressions unless the approved ADR explicitly identifies a compatibility change. No such change is currently proposed.
- Verify migration from the current database state with existing stock data and movements, and verify the reverse/restore procedure used by the repository's migration rehearsal. Migration history and data must not be silently reset.
- Keep writes that span modules inside the existing shared unit of work required by ADR-0019. Verify stock and audit rows commit or roll back together.
- Keep current `Area=equipment` and `Area=warehouse` traits and ensure the existing per-PR battery still selects the journeys. API-mode evidence must remain labeled as API evidence.
- Do not fold catalog types, serial units, images, accessories, alternatives, kits, imports, project line periods, reservations, cancellation releases, manual booking or unresolved found/lost semantics into this boundary move.

## Test plan

| Layer                     | Evidence                                                                                                                                                                                                                                                           |
| ------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Architecture              | Tests fail for direct implementation references from Projects or Warehouse consumers and for tables outside the Equipment-owned schema. Tests pass for allowed contracts and root-host composition.                                                                |
| Domain and application    | Existing Equipment behavior tests pass after moving namespaces and ownership; no behavior assertion changes merely to accommodate the move.                                                                                                                        |
| Persistence and migration | Apply the upgrade to a populated pre-change database; verify item, stock, movements, counts, lifecycle, scan code, audit references and IDs survive. Exercise transactional rollback for participating writes.                                                     |
| API and contract          | Existing requests/responses, routes, authorization, OpenAPI generation and contract-drift check remain stable.                                                                                                                                                     |
| End to end                | Run the complete committed per-PR AppHost battery. Verify existing Equipment and Warehouse rows still pass and retain context tags/evidence modes. Add a focused architecture/migration journey only if the existing public flows cannot prove the upgrade safely. |
| Delivery                  | Run all CI-derived checks, security, license/SBOM and deployment-artifact checks. Link exact-head CI and runtime evidence from the PR.                                                                                                                             |

## Open questions

- Owner must accept ADR-0023 before implementation. Specifically review whether stock movements and counts belong to Equipment while booking, packing and reservation orchestration remain owned by Projects/Warehouse, and confirm the proposed Equipment contract is the only cross-module access path.
- Confirm the migration rehearsal can upgrade databases created by the currently supported installation paths without changing the public API or resetting migration history. Resolve any incompatibility in the plan before implementation.
