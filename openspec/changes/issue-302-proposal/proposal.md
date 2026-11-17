# Proposal

<!-- pdac-scope: cited -->

## Why

Operations coordinators need to group and find contacts using reusable tags and optional folders. The accepted product model already defines this behavior, but the runtime prerequisites and recovered organization work are not yet delivered on the declared base.

## What Changes

- Add Contacts-only tag creation, removal and assignment, and any/all directory filtering combined with existing role and folder filters.
- Add an optional single-folder assignment and manageable folder hierarchy, with safe deletion and cycle prevention.
- Preserve existing Contacts permissions and separate installation databases. Add API, persistence and accessible localized UI coverage, plus an authenticated synthetic journey.
- Adapt the historical #396 implementation after #299 and #301 land, without replacing newer directory or import work.

<!-- pdac:cite id="SB-CON-ORGANIZE-AND-FILTER-CONTACTS" digest="sha256:ed07fa1bacf8f84b6e6e255289a94b5cfe2ac70d4da2893823b086ab7fc57e79" -->

## Capabilities

### New Capabilities

- `contacts-tags-and-filters`: Contacts tag management, assignment and directory filtering.
- `contacts-folders`: optional filing and a safe Contacts folder hierarchy.

### Modified Capabilities

None. The current `openspec/specs/` inventory has no Contacts organization capability.

## Impact

Contacts contracts, application and persistence ports, Minimal APIs, PostgreSQL migration, generated OpenAPI client, frontend and tests change. No breaking interface change is intended; existing directory and import behavior must remain intact. ADR-0012 keeps one installation per database. Applied Product Change `CHG-CONTACT-ORGANIZATION-302` and the reconciled baseline remain product authority. Relevant model roots are FR-CON-004/005, the three Contacts use cases, their tag and folder rules and the three cited structured behaviours. The generic folder rule and setup journey were checked for the accepted optional-filing exception; project folders, import-time organization, bulk edits and multi-organization deployments (#415) are excluded. Broad Workspace impact does not expand scope.

The owner explicitly authorized delivery and merge in queue #437. Prerequisites #299 and #301 are integrated at `166fd329cc8f312137ecc0abb81052a1ee8793dc` and `5d5052b8f599d5b2e0481764cd52275083a40908`; implementation started only after both actual merges. Qualification and integration remain separate steps.
