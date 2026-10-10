---
id: BR-PRJ-SUBTASK-LIMITS
type: business-rule
title: "Subtasks are plain checklist items"
status: draft
applies-to:
  - "UC-PRJ-ADD-TASK"
uses-terms:
  - "TERM-SUBTASK"
provenance:
  source: ".private/research/help-center/articles/360013703139.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A subtask has only a title and a done state; it cannot be assigned, have a deadline, planned time or tracked time. Work needing any of these must be a separate task.

## Rationale

Keeps subtasks lightweight.

## Examples

A 'technical drawing' step needing its own owner becomes a task.

## Exceptions

None.
