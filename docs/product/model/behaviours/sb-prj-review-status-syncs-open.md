---
id: SB-PRJ-REVIEW-STATUS-SYNCS-OPEN
type: structured-behaviour
title: "Intermediate statuses sync as not completed"
status: draft
illustrates:
  - "BR-PRJ-TASK-CALENDAR-SYNC"
given:
  - "A task assigned to the user is in status Review"
  - "The user synchronises tasks to an external calendar"
when: "The calendar refreshes"
then:
  - "The task appears as not completed"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360013703139.md, .private/research/help-center/articles/38421530397202.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Two-state external representation.

## Boundaries

None.
