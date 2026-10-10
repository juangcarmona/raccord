---
id: SB-EQP-COUNT-SHORTAGE-BULK
type: structured-behaviour
title: "Completing a short count"
status: draft
illustrates:
  - "BR-EQP-COUNT-COMPLETION"
given:
  - "A cable item has expected warehouse stock 4"
  - "3 were counted"
when: "The count is completed"
then:
  - "The line result is shortage"
  - "Cable stock is reduced by 1"
  - "One cable is recorded as lost equipment"
uses-terms:
  - "TERM-COUNT-RESULT"
  - "TERM-LOST-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360015960120.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Shows automatic correction and loss recording.

## Boundaries

None.
