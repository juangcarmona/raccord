---
id: SB-EQP-ACCESSORY-MERGE
type: structured-behaviour
title: "Accessory quantities merge into one line"
status: draft
illustrates:
  - "BR-EQP-ACCESSORY-OPTIONS"
given:
  - "A light has an automatic accessory that is not set to always add as a new line and not set to skip if present"
when: "The planner plans the light twice"
then:
  - "One accessory line exists with quantity 2"
uses-terms:
  - "TERM-ACCESSORY"
provenance:
  source: ".private/research/help-center/articles/360013703140.md"
  confidence: medium
  recovered-from: documentation
---

## Intent

Fixes the merge behaviour.

## Boundaries

None.
