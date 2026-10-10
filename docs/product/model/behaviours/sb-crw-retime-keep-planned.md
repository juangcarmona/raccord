---
id: SB-CRW-RETIME-KEEP-PLANNED
type: structured-behaviour
title: "Retiming a function while keeping planned crew times"
status: draft
illustrates:
  - "BR-CRW-RETIME-WITH-PLANNED-RESOURCES"
given:
  - "A function 09:00-15:00 has two planned crew members"
when: "The planner moves the function to 10:00-16:00 and chooses to keep the planned periods"
then:
  - "The function's period is 10:00-16:00"
  - "Both planned crew members keep 09:00-15:00"
uses-terms:
  - "TERM-CREW-ASSIGNMENT"
provenance:
  source: ".private/research/help-center/articles/360014594620.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Explicit choice preserves commitments.

## Boundaries

None.
