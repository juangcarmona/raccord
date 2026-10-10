---
id: "BR-TRB-OFFSET-PAGING-RETIRED"
type: business-rule
title: "Offset paging is retired in favour of cursor paging"
status: draft
applies-to:
  - "UC-INT-READ-RECORDS-THROUGH-API"
  - "UC-INT-ADAPT-TO-API-CHANGE"
uses-terms:
  - "TERM-PAGINATION-CURSOR"
  - "TERM-API-DEPRECATION"
  - "TERM-COLLECTION-PAGE"
provenance:
  source: ".private/research/help-center/articles/37200827246610.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Paging collections by a caller-supplied offset is deprecated and will be removed; after removal, collections are paged only by following the next-page address, which preserves the original filters, sort order and page size, until it is empty.

## Rationale

Cursor paging is faster and stays consistent while data changes.

## Examples

See SB-TRB-SORTED-TOP-N and SB-TRB-FETCH-ALL-BY-CURSOR.

## Exceptions

Until the announced removal date offset paging still works.
