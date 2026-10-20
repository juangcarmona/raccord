<!-- pdac-scope: cited -->

## Context

The Contacts module owns contact creation and directory queries. Organization is product behavior owned by the applied `CHG-CONTACT-ORGANIZATION-302`; API, persistence and UI changes stay inside the Contacts boundary. Reuse current authorization. ADR-0012 defines one organization per deployment and the database as the isolation boundary; the model currently has no workspace key. This change will not introduce multi-organization tenancy or accept a workspace id from the client. Each installation's Contacts rows, tags and folders are isolated by its own database.

<!-- pdac:cite id="FR-CON-004" digest="sha256:a492f73718d6ae6fffae47c5aaa562c092ab8a86e167c5a1f167b667ad55cfe6" -->
<!-- pdac:cite id="FR-CON-005" digest="sha256:351710f9bb90a6e303b5eb4a6a6a2d09df952aeb3b6d69d620030a507ac18bff" -->
<!-- pdac:cite id="BR-CON-CONTACT-TAG-SCOPE" digest="sha256:08680cad6deb2e72201f7a7d3585a8197131d694897c9b0c56e824b0b909f739" -->
<!-- pdac:cite id="BR-CON-FOLDER-HIERARCHY" digest="sha256:9607b972d82930e5ca57988dc52ec26241b8b9104bc7cd3a6eeada76251dd0c7" -->
<!-- pdac:cite id="SB-CON-CONTACTS-ISOLATED-BY-WORKSPACE" digest="sha256:8f28e980e4bafd4fa79db9083f468bdd258d208c6d30d4d091a8886c65e95960" -->

## Goals and non-goals

- Persist reusable deployment-scoped tags, contact assignments, and a hierarchical folder tree.
- Support filtering by folder and selected tags with any/all matching.
- Keep authorization and workspace isolation enforced server-side for every read and write.
- Keep Contacts list interactions accessible and cover them with API, component, and browser tests.
- Do not add organization to other record types, bulk editing, or import-time assignment.

## Decisions

### Persistence and invariants

Add `ContactTag`, `ContactTagAssignment`, and `ContactFolder` entities in the Contacts module. Their rows share the existing deployment database boundary; do not add a tenant/workspace column because one deployment serves one organization. Tag names are unique within that database; assignments have a unique `(contact, tag)` key. A contact has a nullable folder id and at most one folder. A folder has a nullable parent id and sibling position. Foreign keys prevent invalid relationships. No endpoint accepts a client-selected workspace id.

Folder moves reject self/descendant targets. Folder deletion is allowed only when it has no assigned contacts and no child folders. Tag deletion is allowed only when the tag has no assignments; the UI explains that assignments must be removed first. Removing an assignment leaves both the tag and contact intact.

### API and authorization

Expose Contacts endpoints for listing/creating/deleting tags, assigning/removing a contact tag, listing/creating/updating/deleting/reordering folders, assigning/unfiling a contact, and filtering the contact directory. Require existing Contacts read permission for list/filter operations and Contacts management permission for organization mutations. Use request/response contracts in `Raccord.Contracts`; register endpoints alongside Contacts routes and regenerate OpenAPI types. No endpoint accepts a workspace id: ADR-0012 places company isolation at the database-per-deployment boundary.

### Directory filters

Extend the existing contact query with optional folder id, tag ids and match mode (`any` or `all`). No tag selection means no tag restriction. `any` uses a matching assignment; `all` requires every selected tag assignment. Folder and tag filters combine with AND. A well-formed unknown folder or tag id matches no contacts; mutations that target an unknown contact, tag or folder return not found. Data from another installation is inaccessible because each installation has its own database. Preserve existing pagination and sort behavior.

### Frontend

Add an organization panel to the Contacts page for tag management/assignment, folder tree management/assignment, and filters. Keep route/navigation inside the Contacts feature, use existing API client and i18n patterns, expose loading/error/empty states, and make destructive or invalid operations understandable. Avoid a new UI dependency.

### Database and migration

Add EF entities/configuration and one additive migration. Existing contacts remain unfiled and untagged; no data rewrite is needed. Update the in-memory test directory/repository consistently. Migration rollback removes only the new organization tables/columns. The migration does not change the single-organization-per-deployment architecture.

### Rejected alternatives

- Generic cross-module labels/folders: rejected because the approved product scope is Contacts only.
- Client-side-only filtering: rejected because it cannot guarantee workspace isolation or correct pagination.
- Mandatory folder assignment: rejected because contacts may remain unfiled.
- Deleting nonempty folders or assigned tags: rejected to avoid silently dropping organization data.

## Risks

- Adding filter predicates can affect query performance at scale; use contact/tag/folder indexes and validate generated SQL behavior through repository tests.
- The model does not implement multiple workspaces per installation; do not imply row-level tenant isolation or trust a client-supplied workspace id.
- The UI may become dense; keep organization controls grouped, keyboard-accessible, and localized.

## Architecture rules

No architecture-rule deviation and no production dependency is introduced. The one-database-per-deployment isolation decision in ADR-0012 remains in force.

<!-- pdac:cite id="BR-CON-TAG-FILTER-MODES" digest="sha256:fbdc5c466fa3c20260000a51e7105b01cab9002409efcae7bd795d2ba83f1966" -->
<!-- pdac:cite id="BR-CON-FOLDER-DELETE-NONEMPTY" digest="sha256:3a1f1955d9faa33b90388cd7c56418adf2c2c5dd468b44878edf731d5ff074bd" -->
