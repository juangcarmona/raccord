---
id: SB-PRJ-DELETE-STATUS-MOVES-TASKS
type: structured-behaviour
title: "Deleting a status moves its tasks"
status: draft
illustrates:
  - "BR-PRJ-TASK-STATUS-DELETION"
given:
  - "Status 'Review' is held by 4 tasks"
when: "The administrator deletes 'Review' choosing 'In progress' as replacement"
then:
  - "'Review' no longer exists"
  - "The 4 tasks are In progress"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/38421530397202.md"
  confidence: high
  recovered-from: documentation
---

## Intent

No orphaned tasks.

## Boundaries

None.
