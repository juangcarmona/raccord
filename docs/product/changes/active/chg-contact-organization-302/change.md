---
id: CHG-CONTACT-ORGANIZATION-302
type: product-change
title: Accept contact tags, folders and filtering
status: proposed
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

- Are tags shared across module types or scoped to Contacts? Recommendation: scoped to Contacts only, consistent with #302's explicit exclusion of other record types.
- Must every contact belong to exactly one folder, or may it be unfiled? Recommendation: allow zero or one folder so the existing directory create flow remains usable without folder setup.
- Which folder operations are required: create roots/subfolders, rename, reorder, move and delete? Recommendation: support create, rename, reorder and move; refuse deletion while the folder contains contacts or child folders.
- Who can manage tags/folders? Recommendation: users authorized to manage Contacts may organize contact records; workspace-level administration remains governed by existing authorization rules.
- Confirm that multi-tag filtering supports both “any” and “all”. Recommendation: support both modes as described by the draft behavior model.

## Product Acceptance

- The Product Owner confirms Contacts-only tag scope and workspace isolation.
- The Product Owner confirms whether folder assignment is optional and which hierarchy operations are supported.
- The Product Owner confirms the existing any/all tag matching behavior.
- The Product Owner confirms which Contacts permissions allow managing tags, folders and assignments.

## Out of Scope

Tags on other record types, project folders, bulk editing, import assignment of tags or folders, custom fields, implementation, technical design, release or deployment.
