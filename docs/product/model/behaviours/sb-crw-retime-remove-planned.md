---
id: SB-CRW-RETIME-REMOVE-PLANNED
type: structured-behaviour
title: "Retiming a function and removing planned resources"
status: draft
illustrates:
  - "BR-CRW-RETIME-WITH-PLANNED-RESOURCES"
given:
  - "A function has one crew member and one vehicle planned"
when: "The planner changes the function's times and chooses to remove all planned resources"
then:
  - "No crew member or vehicle remains planned on the function"
  - "The function is not fully planned"
uses-terms:
  - "TERM-STAFFING-STATUS"
provenance:
  source: ".private/research/help-center/articles/360014594620.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Removal is an explicit option.

## Boundaries

None.
