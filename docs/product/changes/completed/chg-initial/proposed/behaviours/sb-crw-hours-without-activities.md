---
id: SB-CRW-HOURS-WITHOUT-ACTIVITIES
type: structured-behaviour
title: "Hours not covered by activities"
status: draft
illustrates:
  - "BR-CRW-ACTIVITY-DURATION-CONSISTENCY"
given:
  - "A registration runs 28 January 09:00-18:00"
  - "It has one activity of 4 hours"
when: "The registration is exported without activity columns"
then:
  - "The 5 hours without activities are not exported as activity hours"
uses-terms:
  - "TERM-REGISTRATION-ACTIVITY"
provenance:
  source: ".private/research/help-center/articles/23814779870610.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Intent

Uncovered time is visible.

## Boundaries

None.
