---
id: FR-WHS-027
type: functional-requirement
title: "Manual and scanned booking"
status: draft
derived-from:
  - "UC-WHS-BOOK-MANUALLY"
  - "UC-WHS-BOOK-BY-SCANNING"
  - "BR-WHS-DEFAULT-BOOKING-TARGET"
  - "BR-WHS-NO-EARLY-CHECK-OUT"
verification:
  - scenario-ref: "SB-WHS-NO-EARLY-BOOKING"
  - scenario-ref: "SB-WHS-ALL-AT-STATUS-PROPOSES"
uses-terms:
  - "TERM-WAREHOUSE-BOOKING"
provenance:
  source: ".private/research/help-center/articles/13901986840082.md, .private/research/help-center/articles/360015601560.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let technicians book equipment between warehouse statuses manually (all, per combination, per unit or per amount) or by scanning codes with a phone camera or a webcam, on computer and mobile web app.

## Rationale

Physical tracking.
