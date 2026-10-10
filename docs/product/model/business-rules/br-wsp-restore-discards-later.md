---
id: BR-WSP-RESTORE-DISCARDS-LATER
type: business-rule
title: "Restoring a backup discards all later changes"
status: draft
applies-to:
  - "UC-WSP-REQUEST-RESTORE"
uses-terms:
  - "TERM-WORKSPACE-BACKUP"
provenance:
  source: ".private/research/help-center/articles/360015902019.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Restoring the installation returns all its data to the chosen backup moment and loses every change made since; it is performed only after the administrator has chosen the backup and given explicit approval.

## Rationale

Restores are destructive and must be deliberate.

## Examples

- Restoring yesterday's backup removes today's new projects.

## Exceptions

- To recover a single item, the data is re-entered by hand instead.
