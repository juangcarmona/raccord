<!-- pdac-scope: cited -->

## Product and contract groundwork

- [x] Verify the applied Product Change and citation set; record scope and data invariants for implementation.
- [x] Add backend request/response contracts and OpenAPI coverage for tags, assignments, folders and directory filters.
- [x] Add failing Contacts domain/repository tests for tag reuse, database-per-deployment isolation assumptions, any/all filters, folder tree operations, empty-only deletion and single-folder assignment.

## Persistence and API

- [x] Add Contacts tag, assignment and folder entities, database indexes and additive EF migration; existing contacts remain unfiled and untagged.
- [x] Implement repository operations using the existing database-per-deployment isolation boundary; do not accept client-selected workspace ids.
- [x] Implement authorized endpoints for tags, assignments, folders, hierarchy changes, contact folder assignment and combined directory filters.
- [x] Add API integration tests for authorization, unknown mutation targets, any/all matching, pagination, invalid filters, empty-only deletion and recursive move rejection.

<!-- pdac:cite id="FR-CON-004" digest="sha256:a492f73718d6ae6fffae47c5aaa562c092ab8a86e167c5a1f167b667ad55cfe6" -->
<!-- pdac:cite id="FR-CON-005" digest="sha256:351710f9bb90a6e303b5eb4a6a6a2d09df952aeb3b6d69d620030a507ac18bff" -->
<!-- pdac:cite id="BR-CON-TAG-FILTER-MODES" digest="sha256:fbdc5c466fa3c20260000a51e7105b01cab9002409efcae7bd795d2ba83f1966" -->
<!-- pdac:cite id="BR-CON-FOLDER-DELETE-NONEMPTY" digest="sha256:3a1f1955d9faa33b90388cd7c56418adf2c2c5dd468b44878edf731d5ff074bd" -->
<!-- pdac:cite id="SB-CON-CONTACTS-ISOLATED-BY-WORKSPACE" digest="sha256:8f28e980e4bafd4fa79db9083f468bdd258d208c6d30d4d091a8886c65e95960" -->

## Contacts UI and E2E

- [x] Add failing frontend tests for tag creation/assignment/removal, folder creation/move/assignment, any/all filters, loading and API errors.
- [x] Implement accessible Contacts organization and filter controls using the generated API client and existing design/i18n patterns.
- [x] Add one synthetic Playwright journey that creates tags and a folder, organizes two contacts, verifies any/all and folder filtering, and checks nonempty folder deletion refusal; add it to the per-PR selection. Verify deployment isolation through the database-per-deployment boundary and API tests rather than claiming multi-organization tenancy.
- [x] Update OpenAPI snapshots and verify generated contract drift is empty.

## Documentation and verification

- [x] Update Contacts architecture/runtime/risk documentation and process ledger with durable lessons.
- [x] Run formatters, focused tests, database migration checks, ProductShape validation and citation verification, OpenSpec validation, Markdown lint, complete CI-equivalent frontend/.NET tests and selected Aspire E2E; reconcile all evidence against the diff.
- [x] Mark this task list complete only after all final-head CI checks pass.

<!-- pdac:cite id="UC-CON-TAG-CONTACTS" digest="sha256:c19ff9af883fc97b09e564a63540df53b6235343c791288bfdd1d8cae856f912" -->
<!-- pdac:cite id="UC-CON-MANAGE-CONTACT-FOLDERS" digest="sha256:64bf01e23b42803074aa40b1c666ecfc1c2a1fbd0b453c4d15c2c11c4572c082" -->
<!-- pdac:cite id="UC-CON-FILTER-CONTACT-DIRECTORY" digest="sha256:8c11b6106c10adc68b9e495e57ed5bdd3e1763b98b1033c7cc4c9614770413a9" -->
<!-- pdac:cite id="SB-CON-TAG-FILTER-ANY-AND-ALL" digest="sha256:ee82a9793807a2a39857f77f49c6fb2ee0c21de3831a710d782d2db96c6e940f" -->
<!-- pdac:cite id="SB-CON-ORGANIZE-AND-FILTER-CONTACTS" digest="sha256:a23f45220d361110b70224d3fe20f656fa4e46591f3840da6ff8ddeb955fa003" -->
<!-- pdac:cite id="SB-CON-CONTACTS-ISOLATED-BY-WORKSPACE" digest="sha256:8f28e980e4bafd4fa79db9083f468bdd258d208c6d30d4d091a8886c65e95960" -->
