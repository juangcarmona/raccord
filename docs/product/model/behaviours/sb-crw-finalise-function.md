---
id: SB-CRW-FINALISE-FUNCTION
type: structured-behaviour
title: "Finalising a function makes it visible to all its crew"
status: draft
illustrates:
  - "UC-CRW-SWITCH-PLAN-STATUS"
  - "BR-CRW-STAFFING-STATUS-DERIVATION"
given:
  - "A function with quantity 2 has two draft assignments"
when: "The planner finalises the function"
then:
  - "Both assignments are final"
  - "The function shows as fully planned"
  - "The function appears in both crew members' schedules"
  - "No message is sent to them"
uses-terms:
  - "TERM-PLAN-STATUS"
  - "TERM-STAFFING-STATUS"
provenance:
  source: ".private/research/help-center/articles/360014607700.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Visibility and communication are separate.

## Boundaries

None.
