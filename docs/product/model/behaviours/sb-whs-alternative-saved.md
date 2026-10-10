---
id: SB-WHS-ALTERNATIVE-SAVED
type: structured-behaviour
title: "A searched alternative is remembered"
status: draft
illustrates:
  - "BR-WHS-ALTERNATIVE-SAVED"
  - "UC-WHS-SOLVE-WITH-ALTERNATIVE"
given:
  - "Article A has no configured alternative and is short"
when: "The coordinator searches article C and uses it to solve the shortage"
then:
  - "C is listed as an alternative of A for later shortages"
uses-terms:
  - "TERM-EQUIPMENT-ALTERNATIVE"
provenance:
  source: ".private/research/help-center/articles/360017700639.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Alternatives accumulate.

## Boundaries

None.
