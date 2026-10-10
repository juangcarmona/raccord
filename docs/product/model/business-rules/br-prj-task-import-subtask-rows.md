---
id: BR-PRJ-TASK-IMPORT-SUBTASK-ROWS
type: business-rule
title: "Subtasks are imported as repeated parent rows"
status: draft
applies-to:
  - "UC-PRJ-IMPORT-TASKS"
uses-terms:
  - "TERM-SUBTASK"
provenance:
  source: ".private/research/help-center/articles/39225660180626.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Each subtask occupies one row that repeats the parent task's details exactly and differs only in subtask description and completion; rows with differing parent details create separate tasks.

## Rationale

Identifies which rows form one task.

## Examples

See SB-PRJ-SUBTASK-ROWS-GROUPED.

## Exceptions

None.
