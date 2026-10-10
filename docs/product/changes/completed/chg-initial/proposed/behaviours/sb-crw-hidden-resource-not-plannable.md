---
id: SB-CRW-HIDDEN-RESOURCE-NOT-PLANNABLE
type: structured-behaviour
title: "A vehicle not shown in planning cannot be planned"
status: draft
illustrates:
  - "BR-CRW-PLANNABLE-FLAG"
given:
  - "A vehicle has show-in-planning switched off"
when: "The planner looks for vehicles for a transport function"
then:
  - "The vehicle is not offered"
uses-terms:
  - "TERM-VEHICLE"
provenance:
  source: ".private/research/help-center/articles/360014373260.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Retired resources stay out of planning.

## Boundaries

None.
