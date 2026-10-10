---
id: BR-EQP-IMPORT-NO-STRUCTURE
type: business-rule
title: "Imports do not build kits or accessories"
status: draft
applies-to:
  - "UC-EQP-IMPORT-EQUIPMENT"
  - "UC-EQP-IMPORT-KIT-CONTENT"
uses-terms:
  - "TERM-EQUIPMENT-IMPORT"
  - "TERM-DEFAULT-CONTENT"
  - "TERM-ACCESSORY"
provenance:
  source: ".private/research/help-center/articles/360013703140.md, .private/research/help-center/articles/13725992768786.md"
  confidence: high
  recovered-from: documentation
---

## Rule

An equipment import only sets each item's type; content items and accessories are imported as separate items and linked afterwards, either in the catalog or through a content import.

## Rationale

Rows describe items, not relationships.

## Examples

- A case and its three contents are four rows; the content is attached later.

## Exceptions

None.
