---
id: BR-INT-CURSOR-PAGING-DEFAULT
type: business-rule
title: "Collections are paged with an opaque cursor"
status: draft
applies-to:
  - "UC-INT-READ-RECORDS-THROUGH-API"
uses-terms:
  - "TERM-PAGINATION-CURSOR"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md"
  confidence: high
  recovered-from: documentation
---

## Rule

By default each collection page includes the address of the next page, which is empty on the last page; the cursor in it must be used unchanged.

## Rationale

Cursor paging stays consistent while data changes.

## Examples

A client loops until the next-page address is empty.

## Exceptions

None.
