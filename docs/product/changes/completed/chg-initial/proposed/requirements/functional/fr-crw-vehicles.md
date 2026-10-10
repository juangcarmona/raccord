---
id: FR-CRW-VEHICLES
type: functional-requirement
title: "Maintain vehicles"
status: draft
derived-from:
  - "UC-CRW-ADD-VEHICLE"
  - "BR-CRW-PLANNABLE-FLAG"
verification:
  - scenario-ref: "SB-CRW-HIDDEN-RESOURCE-NOT-PLANNABLE"
uses-terms:
  - "TERM-VEHICLE"
provenance:
  source: ".private/research/help-center/articles/360014373260.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST maintain vehicles with registration number, seats, folder, tags and a show-in-planning flag that controls whether they can be planned.

## Rationale

Transport planning needs vehicles.
