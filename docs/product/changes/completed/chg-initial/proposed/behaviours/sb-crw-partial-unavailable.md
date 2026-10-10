---
id: SB-CRW-PARTIAL-UNAVAILABLE
type: structured-behaviour
title: "Partly unavailable means unavailable"
status: draft
illustrates:
  - "BR-CRW-PARTIAL-AVAILABILITY"
given:
  - "A crew member declared themselves unavailable 16:00-20:00"
  - "The rest of the day is unknown"
when: "The planner views availability for a function on 10:00-18:00"
then:
  - "The crew member is shown unavailable for the function"
uses-terms:
  - "TERM-AVAILABILITY-STATE"
provenance:
  source: ".private/research/help-center/articles/360014685859.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Any known clash excludes the person.

## Boundaries

None.
