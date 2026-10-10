---
id: SB-CRW-RECURRING-SHIFT-WITHOUT-END
type: structured-behaviour
title: "A recurring shift without end date creates one shift"
status: draft
illustrates:
  - "BR-CRW-RECURRENCE-SERIES"
when: "The planner creates a weekly shift without an end date"
then:
  - "Exactly one shift is created"
uses-terms:
  - "TERM-RECURRENCE-PATTERN"
provenance:
  source: ".private/research/help-center/articles/17290277763090.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Recurrence must be bounded.

## Boundaries

None.
