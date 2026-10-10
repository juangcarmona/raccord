---
id: SB-PRJ-SERVICE-BREAKDOWN-FIGURES
type: structured-behaviour
title: "Service breakdown example"
status: draft
illustrates:
  - "BR-PRJ-SERVICE-BREAKDOWN"
given:
  - "Service 'Stage design' has estimated time 40 h, cost per hour 50, price 4000"
  - "Its tasks have planned time totalling 45 h and tracked time 60 h"
when: "The coordinator reads the breakdown"
then:
  - "Estimated cost is 2000"
  - "Planned cost is 2250"
  - "Actual cost is 3000"
  - "Revenue is 4000"
  - "Profit is 1000 before discount"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/38423504883858.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Shows margin from tracked work.

## Boundaries

Profit formula with discounts follows the general financial rules.
