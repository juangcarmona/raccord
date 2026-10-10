---
id: BR-WSP-EMPTY-IDENTIFIER-CREATES
type: business-rule
title: "Rows without identifier become new records"
status: draft
applies-to:
  - "UC-WSP-UPDATE-BY-IMPORT"
uses-terms:
  - "TERM-RECORD-IDENTIFIER"
  - "TERM-IMPORT-MATCH-COLUMN"
provenance:
  source: ".private/research/help-center/articles/36084108749842.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When importing a file matched on record identifiers, rows with an identifier update the corresponding record and rows with an empty identifier are created as new records.

## Rationale

Allows extending an exported list with new rows in the same file.

## Examples

- Exporting 500 items and adding 100 rows without identifier yields 500 updates and 100 new items.

## Exceptions

- A row whose identifier was accidentally cleared is also created as a new record, producing a duplicate.
