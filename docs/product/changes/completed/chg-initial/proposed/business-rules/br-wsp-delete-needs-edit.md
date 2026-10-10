---
id: BR-WSP-DELETE-NEEDS-EDIT
type: business-rule
title: "Deleting a project requires the right to change it"
status: draft
applies-to:
  - "UC-WSP-MANAGE-ROLE"
uses-terms:
  - "TERM-ROLE-PERMISSION"
provenance:
  source: ".private/research/help-center/articles/22185539659666.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A user can only delete projects they are allowed to change; schedule changes are only possible on projects visible to them.

## Rationale

Deletion is a stronger form of change.

## Examples

- A role allowed to delete projects but change only own projects can delete only own projects.

## Exceptions

None.
