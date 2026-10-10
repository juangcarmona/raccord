---
id: FR-INT-003
type: functional-requirement
title: "Enforce interface coverage and status separation"
status: draft
derived-from:
  - "UC-INT-WRITE-RECORDS-THROUGH-API"
  - "BR-INT-API-COVERAGE-BOUNDARY"
  - "BR-INT-WAREHOUSE-STATUS-NOT-WRITABLE"
verification:
  - scenario-ref: SB-INT-QUOTE-CREATION-REFUSED
  - scenario-ref: SB-INT-WAREHOUSE-STATUS-WRITE-REFUSED
uses-terms:
  - "TERM-API-RESOURCE"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md, .private/research/help-center/articles/35485155508370.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST accept through the public interface only the operations each record kind offers, applying the same validation as the user interface.

## Rationale

One consistent contract for every integration.
