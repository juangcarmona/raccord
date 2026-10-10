---
id: FR-CRW-PLANNING-VS-USAGE
type: functional-requirement
title: "Separate crew planning from charged time"
status: draft
derived-from:
  - "UC-CRW-RETIME-FUNCTION"
  - "UC-CRW-RETIME-FUNCTION-GROUP"
  - "BR-CRW-USAGE-VERSUS-PLANNING-PERIOD"
  - "BR-CRW-RETIME-WITH-PLANNED-RESOURCES"
  - "BR-CRW-PLANNER-MOVE-UPDATES-PROJECT"
verification:
  - scenario-ref: "SB-CRW-EARLY-CALL-KEEPS-PRICE"
  - scenario-ref: "SB-CRW-RETIME-KEEP-PLANNED"
  - scenario-ref: "SB-CRW-RETIME-REMOVE-PLANNED"
  - scenario-ref: "SB-CRW-PLANNER-DRAG-PRICE-UNCHANGED"
uses-terms:
  - "TERM-CREW-PLANNING-PERIOD"
  - "TERM-FUNCTION-USAGE-PERIOD"
provenance:
  source: ".private/research/help-center/articles/360014594620.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST keep the crew planning period independent from the charged usage period, change prices only through usage periods, and ask how planned resources are treated when a staffed function or group is retimed.

## Rationale

Crew calls must not leak into prices.
