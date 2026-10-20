<!-- pdac-scope: cited -->
<!-- pdac:cite id="BC-EQUIPMENT" digest="sha256:0e324b394e1d89bacbfa13bc9f312f0592c710fe99f8d9c515dc9c999025bf82" -->
<!-- pdac:cite id="BC-WAREHOUSE" digest="sha256:2604b7b7573cfde1759e4b8c0cb9c5c76a188520fac77862a06344f8d08ca2ff" -->

# Tasks: Equipment module boundary

Implementation is blocked until the Owner accepts ADR-0030 and approves the plan at the Planned gate.

- [ ]   1. Record the Equipment ownership decision in ADR-0030 using the ADR template; keep status `Proposed` until the Owner accepts it.
- [ ]   2. Add the Equipment implementation and Contracts projects, solution entries and explicit root-host registration; enforce the allowed project-reference graph.
- [ ]   3. Move the Equipment stock port and public module contracts; update Projects and current Warehouse consumers to use only the Equipment Contracts assembly.
- [ ]   4. Move Equipment domain, persistence, mappings, use cases and endpoints behind Equipment; keep all existing public routes and contracts stable.
- [ ]   5. Add Equipment-owned schema migration and upgrade/restore evidence proving existing data and migration history are preserved.
- [ ]   6. Extend architecture tests for module references, internal implementation types and Equipment schema ownership; add regression tests before implementation changes (TDD).
- [ ]   7. Update architecture, migration and local-development documentation; index ADR-0023 in arc42 section 9 only after it is accepted.
- [ ]   8. Run backend, architecture, migration, API/contract, complete selected AppHost E2E, security, SBOM/license, and deployment-artifact checks. Preserve existing E2E tags and evidence modes.
- [ ]   9. Update the issue-to-test mapping and PR limitations; complete an independent Done audit and record durable lessons in `docs/operations/process-ledger.md` before integration.

## Definition of Done mapping

| Issue criterion                                                   | Planned proof                                                                                                 |
| ----------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------- |
| Equipment boundary and contracts recorded in accepted ADR         | Owner-accepted ADR-0030 and its contract ownership; ADR-0018/0019/0020 remain referenced.                     |
| Domain, persistence and API ownership moved behind module         | Equipment module owns use cases, schema/DbContext and endpoint mapping; root host only composes modules.      |
| Projects and Warehouse use contracts only                         | Project-reference tests reject implementation dependencies; integrations compile through Equipment.Contracts. |
| Architecture tests enforce references and schema ownership        | Positive and negative executable architecture tests in CI.                                                    |
| Existing Equipment/Warehouse E2E stays green and correctly tagged | Exact-head full selected AppHost battery and context-grouped evidence report.                                 |

## Known limitations to retain in the PR

- Product work #306–#309 remains outside this architecture issue.
- Warehouse planning and availability stories #310–#314 remain separate; #314's documented ProductShape/source conflict is unresolved and must not be encoded by this refactor.
- Cancellation/release integration remains #384.
