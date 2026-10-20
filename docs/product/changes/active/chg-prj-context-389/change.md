---
id: CHG-PRJ-CONTEXT-389
type: product-change
title: Accept project notes, files, metadata and history workflows
status: proposed
base-revision: "5a92fa0cc2ad8841410e007f10391e469492915f"
operations:
  add: []
  modify:
    - UC-PRJ-ADD-NOTE
    - UC-PRJ-ATTACH-FILE
    - UC-PRJ-MANAGE-FILE-FOLDERS
    - UC-PRJ-DEFINE-CUSTOM-FIELD
    - UC-PRJ-SAVE-VIEW
    - UC-PRJ-TAG-RECORDS
    - UC-PRJ-REVIEW-HISTORY
    - FR-PRJ-019
    - FR-PRJ-020
    - BR-PRJ-NOTE-DEFAULT-VISIBILITY
    - BR-PRJ-NOTE-DISPLAY
    - BR-PRJ-NOTE-WITHOUT-EDIT-RIGHTS
    - BR-PRJ-FILE-RIGHTS
    - BR-PRJ-CONFIDENTIAL-FOLDER
    - BR-PRJ-GENERATED-DOCUMENT-FOLDER
    - BR-PRJ-FOLDER-DELETE-CASCADE
    - BR-PRJ-CUSTOM-FIELD-FROZEN
    - BR-PRJ-CONFIDENTIAL-CUSTOM-FIELD
    - BR-PRJ-SAVED-VIEW-CONTENT
    - BR-PRJ-TAG-RULES
    - BR-PRJ-TAG-FILTER-MODES
    - BR-PRJ-HISTORY-STRUCTURE
    - BR-PRJ-HISTORY-FROM-ACTIVATION
  remove: []
---

## Problem

The Product Definition describes project notes, attached files and folders, custom fields, saved views, tags and change history, but those use cases and their requirements or governing rules remain drafts. Issue #389 cannot be implemented as a unified project workspace without an accepted contract for visibility, access and history.

## Intended Product Outcome

Authorized users can keep notes and files with projects, organize files into folders, apply modeled tags and review project history. Workspace administrators can define the modeled custom fields and reusable views. Note visibility, confidential content, file rights, field restrictions and saved-view sharing follow the existing product rules.

## Rationale

Issue #389 unifies existing project-context behavior in the web workspace. Accepting the modeled rules first gives implementation and its browser journey an explicit access and audit contract while keeping generated document templates and generation under #329–#330.

## Affected Product Areas

Project notes, file attachments and folders, workspace custom fields, project tags and filters, saved project views and project change history.

## Open Questions

- Confirm the file constraints that the first web workflow must enforce, including allowed file types, size limits and unsafe-file handling; the current Product Definition specifies rights and confidentiality but not those constraints. Recommendation: follow the repository's existing safe storage boundary and reject unsafe or unsupported files visibly; record exact limits before implementation.
- Confirm whether #389 activates the complete custom-field definition behavior (including linked crew/contact fields, filtering, document display, import and export) or limits the first slice to the fields needed on projects. Recommendation: activate the existing contract, but implement only project-facing surfaces required by the issue.
- Confirm whether saved views in #389 need cross-user sharing, and which roles may set it. Recommendation: honor the existing permission rule and include private plus authorized shared views in the browser journey.
- Confirm the history boundary: only project history for #389, or the existing use case across contacts, equipment and crew as well. Recommendation: verify the project path here and leave other bounded-context journeys to their own issues.
- Confirm whether folder deletion and its cascade to contained files belongs in the first slice. Recommendation: include the confirmation and cascade rule because FR-PRJ-020 already makes it part of the accepted file contract.

## Product Acceptance

- The Product Owner resolves each open question and approves this Product Change.
- The accepted definition makes the seven context workflows, file and note requirements, and their governing visibility, authorization, confidentiality, sharing and history rules active and coherent.
- The #389 browser journey uses synthetic files and records to add a note and attachment, organize project context and inspect history, while asserting modeled access boundaries.
- Document templates and generated project documents remain in #329–#330.

## Out of Scope

Document template editing, document generation and email delivery; changing audit semantics outside project context; expanding project request, task, schedule or time workflows; technical design, code, release or deployment.
