---
id: FR-WHS-033
type: functional-requirement
title: "Status independence and automatic progress"
status: draft
derived-from:
  - "BR-WHS-STATUS-INDEPENDENCE"
  - "BR-WHS-AUTOMATIC-PROGRESS"
verification:
  - scenario-ref: "SB-WHS-PROJECT-STATUS-DOES-NOT-RETURN-GEAR"
  - scenario-ref: "SB-WHS-FIRST-BOOKING-STARTS-PROJECT"
  - scenario-ref: "SB-WHS-ALL-BACK-FINALIZES"
uses-terms:
  - "TERM-WAREHOUSE-STATUS"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md, .private/research/help-center/articles/15239125718546.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST keep warehouse statuses independent of project statuses, propose (not force) status changes when all equipment reached a status, and set in progress and finalizing automatically from bookings.

## Rationale

Single entry.
