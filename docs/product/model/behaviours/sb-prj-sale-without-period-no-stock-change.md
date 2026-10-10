---
id: SB-PRJ-SALE-WITHOUT-PERIOD-NO-STOCK-CHANGE
type: structured-behaviour
title: "Sale project without period does not adjust stock"
status: draft
illustrates:
  - "BR-PRJ-SALE-STOCK-ON-BOOKING"
given:
  - "A sale project without any period contains 10 connectors"
when: "The connectors are booked out"
then:
  - "Stock of connectors is not reduced automatically"
  - "A manual stock adjustment is needed"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/28922703497490.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Why one period is recommended.

## Boundaries

None.
