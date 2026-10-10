---
id: FR-WHS-016
type: functional-requirement
title: "Shortage detection and explanation"
status: draft
derived-from:
  - "BR-WHS-SHORTAGE-DETECTION"
  - "BR-WHS-SHORTAGE-EXPLAINED"
  - "BR-WHS-INVALIDATION-ON-STOCK-CHANGE"
  - "BR-WHS-RECALC-ON-SHIFT"
verification:
  - scenario-ref: "SB-WHS-DOUBLE-BOOKED-SINGLE-UNIT"
  - scenario-ref: "SB-WHS-LOSS-INVALIDATES-RESERVATIONS"
  - scenario-ref: "SB-WHS-DELAY-CAUSES-SHORTAGE-ELSEWHERE"
uses-terms:
  - "TERM-SHORTAGE"
  - "TERM-SHORTAGE-KIND"
provenance:
  source: ".private/research/help-center/articles/360013836939.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST detect shortages per line, explain each with its kind, and recompute them whenever plans, schedules, stock, losses or warehouse movements change.

## Rationale

Uncovered demand must be visible.
