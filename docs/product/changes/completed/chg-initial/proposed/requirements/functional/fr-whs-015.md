---
id: FR-WHS-015
type: functional-requirement
title: "Reallocation of reservations"
status: draft
derived-from:
  - "UC-WHS-REALLOCATE-RESERVATIONS"
  - "BR-WHS-RESERVATION-PRIORITY"
verification:
  - scenario-ref: "SB-WHS-RESERVATION-GIVES-PRIORITY"
  - scenario-ref: "SB-WHS-REALLOCATE-BETWEEN-PROJECTS"
uses-terms:
  - "TERM-RESERVATION"
provenance:
  source: ".private/research/help-center/articles/360013158280.md, .private/research/help-center/articles/13415175193490.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let planners change reserved quantities between overlapping projects within reservable stock and recompute all affected shortages.

## Rationale

Business priority.
