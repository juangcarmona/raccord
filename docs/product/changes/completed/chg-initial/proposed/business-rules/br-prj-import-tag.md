---
id: BR-PRJ-IMPORT-TAG
type: business-rule
title: "Every import is tagged for undo"
status: draft
applies-to:
  - "UC-PRJ-IMPORT-TASKS"
  - "UC-PRJ-IMPORT-CONTACTS"
uses-terms:
  - "TERM-TAG"
provenance:
  source: ".private/research/help-center/articles/39225660180626.md, .private/research/help-center/articles/360013553000.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Records created by an import receive a tag naming the import's date and hour, so the whole import can be found and deleted.

## Rationale

Imports can go wrong.

## Examples

Deleting all contacts tagged with the faulty import's tag.

## Exceptions

None.
