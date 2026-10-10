---
id: BR-PRJ-EQUIPMENT-IMPORT-MATCHING
type: business-rule
title: "Import rows are matched to catalogue items"
status: draft
applies-to:
  - "UC-PRJ-IMPORT-EQUIPMENT-LIST"
uses-terms:
  - "TERM-SAVED-IMPORT-MATCH"
provenance:
  source: ".private/research/help-center/articles/4413562661138.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Each import row is matched to a catalogue item by the column chosen (name, code, design-tool code or a custom field); unmatched rows may be matched manually or turned into temporary items individually or in bulk.

## Rationale

External lists use their own identifiers.

## Examples

A row whose design-tool code equals an item's code plans that item.

## Exceptions

None.
