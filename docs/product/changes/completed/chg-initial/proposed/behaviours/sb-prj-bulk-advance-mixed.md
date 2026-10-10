---
id: SB-PRJ-BULK-ADVANCE-MIXED
type: structured-behaviour
title: "Bulk advance unavailable for mixed statuses"
status: draft
illustrates:
  - "BR-PRJ-TASK-BULK-ADVANCE"
given:
  - "Two selected tasks are To do and one is In progress"
when: "The user looks for the bulk advance action"
then:
  - "The action is not offered"
  - "Selecting only the two To do tasks offers advancing them to the next status"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360013703139.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Next status must be common.

## Boundaries

None.
