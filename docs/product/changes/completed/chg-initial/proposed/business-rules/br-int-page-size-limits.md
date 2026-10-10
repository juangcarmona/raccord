---
id: BR-INT-PAGE-SIZE-LIMITS
type: business-rule
title: "Pages hold 300 records by default and 1500 at most"
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

A collection request returns at most the requested limit of records, 300 when no limit is given, and never more than 1500.

## Rationale

Bounded responses keep the interface responsive.

## Examples

A request with limit 5000 is limited to 1500 records per page.

## Exceptions

None.
