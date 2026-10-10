---
id: BR-EQP-PROJECT-HISTORY-ALL-SELECTED
type: business-rule
title: "Project history of several items requires all"
status: draft
applies-to:
  - "UC-EQP-VIEW-EQUIPMENT-PROJECT-HISTORY"
uses-terms:
  - "TERM-EQUIPMENT-ITEM"
provenance:
  source: ".private/research/help-center/articles/17484144899986.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Asking for the projects of several selected items lists only projects that contain every selected item.

## Rationale

Narrows to projects combining the equipment of interest.

## Examples

- Selecting a mixer and a stage box lists projects using both, not either.

## Exceptions

None.
