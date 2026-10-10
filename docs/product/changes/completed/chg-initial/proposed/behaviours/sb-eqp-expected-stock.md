---
id: SB-EQP-EXPECTED-STOCK
type: structured-behaviour
title: "Expected warehouse stock in a count"
status: draft
illustrates:
  - "BR-EQP-EXPECTED-STOCK"
given:
  - "A microphone has quantity 10"
  - "2 are content of physical kits"
  - "3 are on location for projects"
when: "A stock count including the microphone is created"
then:
  - "The expected warehouse stock of the microphone is 5"
uses-terms:
  - "TERM-EXPECTED-WAREHOUSE-STOCK"
provenance:
  source: ".private/research/help-center/articles/360015960120.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Fixes the expected stock formula.

## Boundaries

None.
