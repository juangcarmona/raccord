---
id: FR-PRJ-021
type: functional-requirement
title: "Manage tasks"
status: draft
derived-from:
  - "UC-PRJ-ADD-TASK"
  - "UC-PRJ-PROGRESS-TASK"
  - "BR-PRJ-TASK-NAME-REQUIRED"
  - "BR-PRJ-TASK-OVERDUE"
  - "BR-PRJ-TASK-RELATIVE-DEADLINE"
  - "BR-PRJ-SUBTASK-LIMITS"
  - "BR-PRJ-TASK-VISIBILITY-RULES"
verification:
  - scenario-ref: "SB-PRJ-TASK-TURNS-OVERDUE"
  - scenario-ref: "SB-PRJ-RELATIVE-DEADLINE-MOVES"
  - scenario-ref: "SB-PRJ-BULK-ADVANCE-MIXED"
uses-terms:
  - "TERM-TASK"
  - "TERM-TASK-DEADLINE"
provenance:
  source: ".private/research/help-center/articles/360013703139.md, .private/research/help-center/articles/38421530397202.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST manage tasks with the documented attributes, deadlines (none, fixed, relative), visibility, subtasks and status progression, flagging overdue tasks.

## Rationale

Production work outside the event must be planned.
