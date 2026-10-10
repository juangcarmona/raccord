---
id: SB-PRJ-TASK-IMPORT-UNKNOWN-STATUS
type: structured-behaviour
title: "Task row with unknown status fails"
status: draft
illustrates:
  - "BR-PRJ-TASK-IMPORT-MATCHING"
  - "UC-PRJ-IMPORT-TASKS"
given:
  - "The installation has statuses To do, In progress, Completed"
  - "A file row has status 'Doing'"
when: "The task file is imported"
then:
  - "That row fails with an unknown-status reason"
  - "No status 'Doing' is created"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/39225660180626.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Statuses are never created by import.

## Boundaries

None.
