---
id: SB-CRW-PARTIAL-AVAILABLE-UNKNOWN
type: structured-behaviour
title: "Partly available and otherwise unknown means available"
status: draft
illustrates:
  - "BR-CRW-PARTIAL-AVAILABILITY"
given:
  - "A crew member declared availability 10:00-14:00"
  - "Nothing is known for the rest"
when: "The planner views availability for a function on 10:00-18:00"
then:
  - "The crew member is shown available"
uses-terms:
  - "TERM-AVAILABILITY-STATE"
provenance:
  source: ".private/research/help-center/articles/360014685859.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Unknown does not reduce availability.

## Boundaries

None.
