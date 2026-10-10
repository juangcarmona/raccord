---
id: BR-INT-OFFSET-PAGING-FOR-OTHER-SORTS
type: business-rule
title: "Offset paging applies when sorting by another column"
status: draft
applies-to:
  - "UC-INT-READ-RECORDS-THROUGH-API"
uses-terms:
  - "TERM-COLLECTION-PAGE"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When a collection is sorted on a column other than the identifier, paging uses an offset given by the caller and no next-page address is returned.

## Rationale

Cursors are defined on the identifier order.

## Examples

Sorting contacts by name requires the client to compute offsets 0, 300, 600.

## Exceptions

Superseded by BR-TRB-OFFSET-PAGING-RETIRED, documented later: offset paging is retired and the sort order travels inside the cursor. Raccord implements cursor paging only.
