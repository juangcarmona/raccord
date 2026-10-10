---
id: "SB-TRB-HIDDEN-ITEM-NOT-OFFERED"
type: structured-behaviour
title: "Item hidden from planning is missing from the list"
status: draft
illustrates:
  - "BR-EQP-HIDDEN-ITEM-STILL-BOOKABLE"
  - "UC-WHS-PLAN-EQUIPMENT"
given:
  - "A projector that is out of use has planner visibility switched off"
when: "The coordinator searches for the projector while planning a project"
then:
  - "The projector is not offered"
  - "Switching its planner visibility back on makes it plannable again"
uses-terms:
  - "TERM-PLANNER-VISIBILITY"
provenance:
  source: ".private/research/help-center/articles/360015031799.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Planner visibility explains missing items.

## Boundaries

None.
