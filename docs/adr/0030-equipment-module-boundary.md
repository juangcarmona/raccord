---
name: equipment-module-boundary
description: Equipment owns catalog and stock records and exposes them through a module contract.
status: Proposed
date: 2026-10-20
deciders: ["Juan G. Carmona"]
tags: ["equipment", "module-boundary", "persistence"]
---

# ADR-0030: Give Equipment ownership of its catalog and stock records

## Context

Raccord defines Equipment and Warehouse as separate bounded contexts. Equipment items, stock movements and physical counts currently reside in shared Domain and Infrastructure projects, and Equipment endpoints are mapped by the root Web project. Projects owns the `IEquipmentStock` interface even though the interface describes operations on Equipment records. Future catalog and warehouse-planning work will add more consumers and persistence, increasing the cost of leaving ownership implicit.

ADR-0018 establishes one assembly per context with contracts assemblies for consumers. ADR-0019 assigns each module its own schema and DbContext while allowing participating modules to share one transaction. ADR-0020 establishes synchronous contracts for cross-module calls. Those records do not assign ownership of the existing Equipment stock records or the current contract to a particular module.

## Decision

We will make Equipment the owner of catalog items and their stock-account records, including lifecycle state, stock movements, physical counts and the Equipment API. Equipment will expose the operations needed by other contexts through `Raccord.Modules.Equipment.Contracts`.

Projects will own project plans and project-associated equipment lines. It will call Equipment through the public contract for stock operations and will not reference Equipment persistence or implementation types. Future Warehouse workflows will use the same Equipment contract for stock data; Warehouse does not gain ownership of the catalog or stock-account records. The root Web host remains the composition root.

The Equipment module will own its DbContext and schema. Existing data and migration history must be preserved when moving the records. Calls spanning Equipment and another module will use the shared unit of work and transaction boundaries required by ADR-0019.

## Consequences

- **Positive:** Catalog and stock data have one explicit owner; future consumers use a narrow compile-time contract; persistence access can be enforced independently of route composition.
- **Negative:** The extraction touches project references, persistence mappings, endpoints and migration history; module contracts need maintenance as cross-context use cases evolve.
- **Neutral:** ADR-0018, ADR-0019 and ADR-0020 continue to govern module shape, schema and transaction ownership, and communication. This decision does not add catalog or Warehouse product behavior.

## Alternatives considered

- **Keep Equipment data and behavior in shared projects:** rejected because new Equipment and Warehouse work would continue to depend on shared implementation details without an enforceable ownership boundary.
- **Make Projects the owner of Equipment records:** rejected because Equipment is a separate product context and Project plans must consume stock rather than own the catalog and stock ledger.
- **Move catalog and stock ownership to Warehouse:** rejected because the product model assigns Equipment catalog, lifecycle, movement and count use cases to Equipment; Warehouse consumes those records for physical handling.
- **Split catalog and stock records between multiple modules now:** rejected because that would make the existing Equipment workflows span modules before Warehouse has a separately implemented module and would add transactional seams without a demonstrated need.

## References

- Issue: [#305](https://github.com/juangcarmona/raccord/issues/305)
- Epic: [#375](https://github.com/juangcarmona/raccord/issues/375)
- Product contexts: `BC-EQUIPMENT`, `BC-WAREHOUSE`
- Related: [ADR-0018](0018-module-assemblies-named-after-bounded-contexts.md), [ADR-0019](0019-module-owned-schemas-with-a-shared-transaction.md), [ADR-0020](0020-synchronous-module-contracts-with-deferred-events.md)
