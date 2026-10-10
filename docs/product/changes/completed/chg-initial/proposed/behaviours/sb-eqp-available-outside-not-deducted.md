---
id: SB-EQP-AVAILABLE-OUTSIDE-NOT-DEDUCTED
type: structured-behaviour
title: "Flexible content stays in free stock"
status: draft
illustrates:
  - "BR-EQP-AVAILABLE-OUTSIDE-BEHAVES-VIRTUAL"
given:
  - "A physical kit contains 1 amplifier reserved from stock and 1 microphone available outside the kit"
when: "The kit's stock is set to 4"
then:
  - "Amplifier free stock drops by 4"
  - "Microphone free stock is unchanged"
  - "Microphones remain plannable on other projects"
uses-terms:
  - "TERM-CONTENT-STOCK-BINDING"
provenance:
  source: ".private/research/help-center/articles/19895983469330.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Shows mixed fixed and flexible content inside one case.

## Boundaries

None.
