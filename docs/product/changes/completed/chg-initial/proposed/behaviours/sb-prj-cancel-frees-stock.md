---
id: SB-PRJ-CANCEL-FREES-STOCK
type: structured-behaviour
title: "Cancelling makes equipment available immediately"
status: draft
illustrates:
  - "BR-PRJ-CANCEL-RELEASES-EQUIPMENT"
  - "UC-PRJ-CANCEL-PROJECT"
given:
  - "Project A reserves all 8 moving heads for 3 to 5 May"
  - "Project B on 4 May shows a shortage of 4 moving heads"
when: "Project A is cancelled"
then:
  - "Project A's reservations are removed"
  - "Project B no longer shows a shortage"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/28083930405778.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Cancellation releases stock at once.

## Boundaries

None.
