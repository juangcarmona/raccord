---
id: QR-CRW-PLANNER-HORIZON
type: quality-requirement
title: "Cross-project planner handles more than a year of events"
status: draft
quality-attribute: "performance"
applies-to:
  - "UC-CRW-ADJUST-FUNCTION-TIMES-IN-PLANNER"
  - "UC-CRW-PLAN-RESOURCE-ON-FUNCTION"
verification:
  - scenario: "Loading a planning range of 13 months of functions remains usable without noticeable degradation"
uses-terms:
  - "TERM-PROJECT-FUNCTION"
provenance:
  source: ".private/research/help-center/articles/360014625560.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

The cross-project planner MUST load and navigate more than one year of functions without performance problems.

## Measurement

Range loaded versus responsiveness; a range above 12 months loads with the same interaction responsiveness as a one-month range.
