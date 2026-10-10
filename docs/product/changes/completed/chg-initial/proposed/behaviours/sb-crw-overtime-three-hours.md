---
id: SB-CRW-OVERTIME-THREE-HOURS
type: structured-behaviour
title: "Overtime crossing two bands"
status: draft
illustrates:
  - "BR-CRW-RATE-FACTOR-CALCULATION"
given:
  - "The same rate of 450 up to 10 hours, 67.50 per hour up to 12 hours and 90 per hour beyond"
  - "The crew member is planned 10 hours"
when: "The crew member registers 13 hours on the function"
then:
  - "The cost of the assignment is 675.00 (450 plus two hours at 67.50 plus one hour at 90)"
uses-terms:
  - "TERM-RATE-FACTOR"
provenance:
  source: ".private/research/help-center/articles/7191443180562.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Each band's hourly amount applies only to hours inside that band.

## Boundaries

None.
