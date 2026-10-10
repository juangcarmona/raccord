---
id: BR-PRJ-FOLDER-DELETE-NONEMPTY
type: business-rule
title: "Folders with records cannot be deleted"
status: draft
applies-to:
  - "UC-PRJ-MANAGE-RECORD-FOLDERS"
uses-terms:
  - "TERM-RECORD-FOLDER"
provenance:
  source: ".private/research/help-center/articles/360013700660.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A record folder that still contains contacts, equipment, crew members or vehicles cannot be deleted.

## Rationale

Records must always have a folder.

## Examples

See SB-PRJ-NONEMPTY-FOLDER-DELETE-REFUSED.

## Exceptions

None.
