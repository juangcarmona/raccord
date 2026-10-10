---
id: QR-INT-API-PAGE-SIZE
type: quality-requirement
title: "Bounded collection pages"
status: draft
quality-attribute: "performance"
applies-to:
  - "UC-INT-READ-RECORDS-THROUGH-API"
verification:
  - scenario-ref: SB-INT-PAGE-DEFAULT
  - scenario-ref: SB-INT-PAGE-LIMIT-CAPPED
uses-terms:
  - "TERM-COLLECTION-PAGE"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

Collection responses MUST contain at most 1500 records, 300 by default.

## Measurement

Count records per page for limits absent, 300, 1500 and above 1500; maximum 1500, default 300.
