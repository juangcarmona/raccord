---
id: BR-WSP-EDITED-IDENTIFIER-MISMATCH
type: business-rule
title: "Editing identifiers in a file corrupts the update"
status: draft
applies-to:
  - "UC-WSP-UPDATE-BY-IMPORT"
uses-terms:
  - "TERM-RECORD-IDENTIFIER"
provenance:
  source: ".private/research/help-center/articles/36084108749842.md"
  confidence: medium
  recovered-from: documentation
---

## Rule

The product trusts the identifier column: a changed identifier makes the row update another record, link the wrong records, or fail; identifiers in import files must be left as exported.

## Rationale

The identifier is the only link between a row and a record.

## Examples

- Swapping two identifiers swaps the prices of two unrelated items.

## Exceptions

None.
