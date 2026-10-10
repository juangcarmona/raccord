---
id: FR-EQP-006
type: functional-requirement
title: "Compute kit stock and availability"
status: draft
derived-from:
  - "BR-EQP-VIRTUAL-KIT-STOCK-DERIVED"
  - "BR-EQP-PHYSICAL-KIT-CONSUMES-CONTENT-STOCK"
  - "BR-EQP-PHYSICAL-KIT-AVAILABILITY"
  - "BR-EQP-AVAILABLE-OUTSIDE-BEHAVES-VIRTUAL"
verification:
  - scenario-ref: "SB-EQP-VIRTUAL-KIT-STOCK-LIMITED"
  - scenario-ref: "SB-EQP-VIRTUAL-KIT-AVAILABLE-LIMITED"
  - scenario-ref: "SB-EQP-VIRTUAL-KIT-IGNORES-SALE"
  - scenario-ref: "SB-EQP-PHYSICAL-KIT-DEDUCTS-CONTENT"
  - scenario-ref: "SB-EQP-PHYSICAL-KIT-OVERALLOCATION"
  - scenario-ref: "SB-EQP-AVAILABLE-OUTSIDE-NOT-DEDUCTED"
uses-terms:
  - "TERM-VIRTUAL-KIT"
  - "TERM-PHYSICAL-KIT"
  - "TERM-CONTENT-STOCK-BINDING"
provenance:
  source: ".private/research/help-center/articles/4402870209682.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST derive virtual kit stock and availability from rental content, MUST deduct bound content of physical kits from the content items' free stock, and MUST treat content marked available outside the kit as free stock.

## Rationale

Kits must never hide double bookings.
