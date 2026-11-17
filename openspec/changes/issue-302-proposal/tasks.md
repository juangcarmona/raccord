# Tasks

<!-- pdac-scope: cited -->

## 1. Prerequisites and data

- [x] 1.1 Confirm #299 then #301 are delivered on the actual base and queue #437 permits activation; record the base revision and compare the historical #396 packet before implementation.
- [x] 1.2 Adapt Contacts tables, constraints and nullable folder assignment; regenerate the migration and EF metadata and verify upgrade, preservation and rollback on a disposable database.

## 2. Organization and filters

- [x] 2.1 Adapt tags, folder and assignment ports and use cases; verify create/remove, whitespace rejection, unfile, unknown targets and existing Contacts authorization at API and domain seams. Compare assigned-tag deletion refusal to accepted semantics.
- [x] 2.2 Implement shared transactional hierarchy serialization and controlled conflict mapping; prove opposing moves, reorder, delete, child creation and assignment races cannot commit invalid state, while cancellation and infrastructure failures remain distinguishable.
- [x] 2.3 Adapt directory predicates and generated API contract; verify any/all, repeated IDs, invalid modes, role/folder intersections and stable ordered pagination in repository and API tests.
- [x] 2.4 Verify all organization reads, writes and filters against two independent installation databases and permissions, including identical names and no cross-database access.
<!-- pdac:cite id="SB-CON-CONTACTS-ISOLATED-BY-WORKSPACE" digest="sha256:5eb794eff7858450263824556fd9756d03d96ea9d32b5db2df4b672778539165" -->

## 3. Frontend

- [x] 3.1 Adapt feature-local organization and directory controls with generated client and Spanish/English copy; verify tag, folder, filters, empty/error states, keyboard and visible focus with frontend tests.
- [x] 3.2 Capture rendered desktop and narrow viewport evidence in supported appearances; compare with accepted UI contract, contrast, focus and reduced motion using the Taste fallback and record operator review under the existing owner delivery and merge authorization.

## 4. Integration evidence

- [x] 4.1 Add the authenticated synthetic ContactOrganizationJourney_Should_ManageTagsFoldersAndFilters to the per-PR manifest and run it with the existing directory and import journeys; record revisions, commands, run IDs and exit status.
- [ ] 4.2 Run CI-equivalent build, tests, CSharpier, Prettier, lint-staged, commitlint and OpenAPI drift checks on the actual base; record results and failures.
- [x] 4.3 Refresh affected provider citations as needed and run ProductShape citation/evidence verification and OpenSpec strict validation; record current model digest, citation status and results.

## Workflow follow-up

- Request owner review of the implementation and evidence; archive only after required acceptance.
