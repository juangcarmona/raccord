---
id: SB-CRW-OVERTIME-ONE-HOUR
type: structured-behaviour
title: "One overtime hour on a banded rate"
status: draft
illustrates:
  - "BR-CRW-RATE-FACTOR-CALCULATION"
  - "BR-CRW-OVERTIME-FROM-REGISTERED-HOURS"
given:
  - "A rate has a fixed 450 for up to 10 hours, 67.50 per hour from 10 to 12 hours and 90 per hour beyond 12 hours"
  - "A crew member with that rate is planned 10 hours on a function"
when: "The crew member registers 11 hours on the function"
then:
  - "The cost of the assignment is 517.50 (450 plus one hour at 67.50)"
uses-terms:
  - "TERM-RATE-FACTOR"
  - "TERM-OVERTIME"
provenance:
  source: ".private/research/help-center/articles/7191443180562.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Fixes how the first overtime band adds to the fixed band.

## Boundaries

None.
