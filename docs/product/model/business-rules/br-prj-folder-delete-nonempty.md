---
id: BR-PRJ-FOLDER-DELETE-NONEMPTY
type: business-rule
title: Folders with records cannot be deleted
status: draft
applies-to: []
uses-terms:
  - TERM-RECORD-FOLDER
provenance:
  source: .private/research/help-center/articles/360013700660.md
  confidence: high
  recovered-from: documentation
---

## Rule

A record folder that still contains contacts, equipment, crew members or vehicles cannot be deleted.

## Rationale

Deleting a container must not remove or silently reassign the records it contains; this does not require every contact to have a folder.

## Examples

See SB-PRJ-NONEMPTY-FOLDER-DELETE-REFUSED.

## Exceptions

Contacts may remain unfiled under BR-CON-FOLDER-HIERARCHY. BR-CON-FOLDER-DELETE-NONEMPTY additionally protects child folders.
