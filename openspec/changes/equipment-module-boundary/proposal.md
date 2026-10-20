<!-- pdac-scope: cited -->

## Why

Equipment catalog, stock, persistence and HTTP behavior still live in shared Domain, Infrastructure and Web projects, while Projects owns the `IEquipmentStock` seam. Issue #305 moves this existing behavior behind the Equipment bounded context so future catalog and planning work has a clear owner without changing user-visible behavior.

## What Changes

- Add the Equipment implementation module and its consumer-facing Contracts assembly, registered through the root host.
- Move current Equipment domain, persistence and endpoint ownership behind that module while preserving HTTP routes, request/response shapes, persisted records, audit behavior and existing journeys.
- Move the stock contract to the Equipment-owned Contracts assembly; make Projects and current Warehouse flows depend on that contract, not Equipment persistence or implementation types.
- Enforce the module reference graph and Equipment schema ownership with architecture tests and build rules.
- Define the Equipment ownership and cross-context contract boundary in a proposed ADR for Owner acceptance before implementation.
- Add no product capability or requirement delta. This is an architecture and persistence-boundary change only; new equipment behavior remains in #306–#309 and Warehouse planning behavior remains in #310–#314.

## Capabilities

### New Capabilities

None. This is a behavior-preserving module extraction.

### Modified Capabilities

None. Existing product behavior and API contracts remain unchanged.

## Impact

**Code:** equipment aggregates, stock service, EF Core mapping, web endpoints, module registration, Projects integration, architecture tests, migration and E2E test configuration.

**Compatibility:** preserve current `/equipment` routes, current request and response contracts, existing database records and migration history, audit events, and the selected Equipment/Warehouse journeys and their context tags. No production dependency is added.

**Product definition:** this change implements the existing context boundary in `BC-EQUIPMENT` and preserves its Warehouse interactions in `BC-WAREHOUSE`; it does not alter either context's meaning or any use case. Structural impact was checked to depth 2 for `BC-EQUIPMENT`; neighboring Warehouse rules, use cases and terms were reviewed and remain unchanged. `UC-EQP-*`, `UC-WHS-*`, and `BR-WHS-*` behavior remains outside this refactor's delta.

<!-- pdac:cite id="BC-EQUIPMENT" digest="sha256:0e324b394e1d89bacbfa13bc9f312f0592c710fe99f8d9c515dc9c999025bf82" -->
<!-- pdac:cite id="BC-WAREHOUSE" digest="sha256:2604b7b7573cfde1759e4b8c0cb9c5c76a188520fac77862a06344f8d08ca2ff" -->

## Work item

Issue [#305](https://github.com/juangcarmona/raccord/issues/305), child of Epic [#375](https://github.com/juangcarmona/raccord/issues/375).
