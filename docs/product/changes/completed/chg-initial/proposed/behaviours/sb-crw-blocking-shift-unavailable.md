---
id: SB-CRW-BLOCKING-SHIFT-UNAVAILABLE
type: structured-behaviour
title: "A blocking shift makes crew unavailable"
status: draft
illustrates:
  - "BR-CRW-SHIFT-AVAILABILITY-BLOCKING"
given:
  - "A crew member is on a shift 08:00-16:00 that blocks availability"
when: "The planner checks availability for a function 12:00-14:00"
then:
  - "The crew member is shown unavailable"
uses-terms:
  - "TERM-SHIFT"
provenance:
  source: ".private/research/help-center/articles/17290277763090.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Blocking shifts are commitments.

## Boundaries

None.
