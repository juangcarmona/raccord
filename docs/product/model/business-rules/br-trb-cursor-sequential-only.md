---
id: "BR-TRB-CURSOR-SEQUENTIAL-ONLY"
type: business-rule
title: "Cursor paging offers no random page access"
status: draft
applies-to:
  - "UC-INT-READ-RECORDS-THROUGH-API"
uses-terms:
  - "TERM-PAGINATION-CURSOR"
provenance:
  source: ".private/research/help-center/articles/37200827246610.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Cursor paging moves through a collection only sequentially; an arbitrary page cannot be requested directly, so integrations page forward from the start, preferably after narrowing the collection with filters.

## Rationale

The cursor encodes a position, not a page number.

## Examples

See SB-TRB-NO-JUMP-TO-PAGE.

## Exceptions

None.
