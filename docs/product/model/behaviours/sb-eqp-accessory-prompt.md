---
id: SB-EQP-ACCESSORY-PROMPT
type: structured-behaviour
title: "Non-automatic accessory is offered"
status: draft
illustrates:
  - "BR-EQP-ACCESSORY-OPTIONS"
given:
  - "A moving head has a rain cover accessory that is not automatic"
when: "The planner plans the moving head"
then:
  - "The planner is asked whether to add the rain cover"
  - "Nothing is added if the planner declines"
uses-terms:
  - "TERM-ACCESSORY"
provenance:
  source: ".private/research/help-center/articles/360013703140.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Distinguishes prompted from automatic accessories.

## Boundaries

None.
