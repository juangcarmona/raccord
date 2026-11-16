---
id: CHG-CONTACT-ORGANIZATION-302
type: product-change
title: Accept contact tags, folders and filtering
status: applied
base-revision: "769edf87e11dec97b23de000741c2f745545256b"
operations:
  add:
    - UC-CON-TAG-CONTACTS
    - UC-CON-MANAGE-CONTACT-FOLDERS
    - UC-CON-FILTER-CONTACT-DIRECTORY
    - FR-CON-004
    - FR-CON-005
    - BR-CON-CONTACT-TAG-SCOPE
    - BR-CON-TAG-FILTER-MODES
    - BR-CON-FOLDER-HIERARCHY
    - BR-CON-FOLDER-DELETE-NONEMPTY
    - TERM-CONTACT-TAG
    - TERM-CONTACT-FOLDER
    - SB-CON-TAG-FILTER-ANY-AND-ALL
    - SB-CON-CONTACTS-ISOLATED-BY-WORKSPACE
    - SB-CON-ORGANIZE-AND-FILTER-CONTACTS
  modify: []
  remove: []
---

## Problem

Issue #302 requires users to organize and filter Contacts records using tags and folders. Current product artifacts describe draft tag and folder behavior across several modules, which would exceed the issue's Contacts-only scope if accepted unchanged.

## Intended Product Outcome

Authorized users can create and remove tags for contacts, assign or remove a tag on a contact, and filter the directory by one or more tags. When several tags are selected, users choose whether a contact must match any selected tag or all selected tags. Users can create a hierarchical folder structure for Contacts and organize records in it. Contact tags, folders and filters are isolated to the active workspace and do not classify other record types.

## Rationale

Contacts needs its own organization tools while keeping the accepted product scope and tenant boundary clear. This change uses Contacts-specific terms and workflows rather than activating draft cross-module behavior.

## Affected Product Areas

Contacts directory, contact tags, Contacts folder structure, directory filters, permissions and workspace isolation.

## Open Questions

None. The recommendations below are accepted.

## Product Acceptance

- Tags and folders apply only to Contacts and are isolated by workspace.
- Contacts may remain unfiled or belong to at most one folder.
- Contact folders support creating roots and subfolders, renaming, reordering, moving, and deletion only when empty of contacts and child folders.
- Users with existing Contacts management permissions may manage tags, folders, and assignments; no additional workspace-level role is introduced.
- Multi-tag filters support both any-match and all-match modes.

## Approval

Approved by the product owner on 2026-10-20 under the explicit authorization to proceed unattended with the Contact backlog implementation. The recommended decisions in the proposal are accepted as written.

## Out of Scope

Tags on other record types, project folders, bulk editing, import assignment of tags or folders, custom fields, implementation, technical design, release or deployment.
