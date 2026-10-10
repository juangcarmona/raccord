---
id: BR-PRJ-TASK-OVERDUE
type: business-rule
title: "Uncompleted tasks past a fixed deadline are overdue"
status: draft
applies-to:
  - "UC-PRJ-PROGRESS-TASK"
uses-terms:
  - "TERM-TASK-DEADLINE"
provenance:
  source: ".private/research/help-center/articles/360013703139.md, .private/research/help-center/articles/38422661951506.md, .private/research/help-center/articles/360014243139.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A task with a deadline that has passed and is not Completed is marked overdue (highlighted on boards and lists and counted as overdue in task widgets).

## Rationale

Late work must stand out.

## Examples

See SB-PRJ-TASK-TURNS-OVERDUE.

## Exceptions

Tasks without deadline are never overdue.
