---
id: SB-EQP-SCHEDULED-DELIVERY
type: structured-behaviour
title: "Future delivery available from its date"
status: draft
illustrates:
  - "BR-EQP-MOVEMENT-EFFECTIVE-DATE"
given:
  - "An item has a delivery of 10 scheduled on a future date"
when: "Availability is checked for the day before and the day after the delivery date"
then:
  - "The day before shows the old stock"
  - "The day after includes the 10 delivered units"
uses-terms:
  - "TERM-SCHEDULED-STOCK-MOVEMENT"
provenance:
  source: ".private/research/help-center/articles/16459216697106.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Fixes the dating of movements.

## Boundaries

None.
