---
id: SB-CRW-APPOINTMENT-MAKES-UNAVAILABLE
type: structured-behaviour
title: "An overlapping appointment makes a crew member unavailable"
status: draft
illustrates:
  - "BR-CRW-AVAILABILITY-DERIVATION"
given:
  - "A crew member has a blocking appointment Tuesday 10:00-11:00"
when: "The planner views availability for a function on Tuesday 09:00-12:00"
then:
  - "The crew member is shown unavailable"
uses-terms:
  - "TERM-APPOINTMENT"
  - "TERM-AVAILABILITY-STATE"
provenance:
  source: ".private/research/help-center/articles/360014685859.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Appointments count as commitments.

## Boundaries

None.
