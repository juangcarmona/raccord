---
id: BR-EQP-ARCHIVE-PRESERVES-HISTORY
type: business-rule
title: "Archiving removes from stock but keeps history"
status: draft
applies-to:
  - "UC-EQP-ARCHIVE-ITEM"
  - "UC-EQP-CHANGE-ITEM-TYPE"
uses-terms:
  - "TERM-EQUIPMENT-RECORD-STATE"
  - "TERM-ARCHIVED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360013852060.md, .private/research/help-center/articles/360013718240.md"
  confidence: high
  recovered-from: documentation
---

## Rule

An archived item leaves stock and can no longer be planned, while projects, documents and statistics that used it keep it; on project equipment lists it appears struck through, on documents and in price calculations it appears normally. Only archived items can be permanently deleted, and deletion is irreversible.

## Rationale

Historic records must remain intact; deletion would break documents.

## Examples

- An archived projector still prints on last year's invoice.

## Exceptions

None.
