---
id: FR-WHS-034
type: functional-requirement
title: "Custom warehouse statuses"
status: draft
derived-from:
  - "UC-WHS-CONFIGURE-WAREHOUSE-STATUSES"
  - "BR-WHS-LOCKED-STATUSES"
  - "BR-WHS-CUSTOM-STATUS-OUT"
verification:
  - scenario-ref: "SB-WHS-LOCKED-STATUS-NOT-REMOVABLE"
  - scenario-ref: "SB-WHS-CUSTOM-STATUS-UNAVAILABLE"
uses-terms:
  - "TERM-CUSTOM-WAREHOUSE-STATUS"
provenance:
  source: ".private/research/help-center/articles/15239120279442.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let administrators add, order and delete unused custom warehouse statuses around the locked ones, treating them as out of the warehouse.

## Rationale

Fit workflows.
