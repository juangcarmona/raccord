---
id: SB-EQP-PHYSICAL-KIT-OVERALLOCATION
type: structured-behaviour
title: "Over-allocated content creates a lasting shortage"
status: draft
illustrates:
  - "BR-EQP-PHYSICAL-KIT-AVAILABILITY"
given:
  - "Each kit holds 1 item A reserved from stock"
  - "Item A has stock 3"
when: "The physical kit is saved with stock quantity 5"
then:
  - "Item A shows free stock of minus 2"
  - "Item A is short wherever it is considered"
  - "The kit itself is available as 5 units"
uses-terms:
  - "TERM-PHYSICAL-KIT"
  - "TERM-SHORTAGE"
provenance:
  source: ".private/research/help-center/articles/4402870209682.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Documents the warned-about permanent shortage when cases claim more content than owned.

## Boundaries

None.
