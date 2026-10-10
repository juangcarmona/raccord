---
id: BR-PRJ-TASK-IMPORT-EXCLUSIONS
type: business-rule
title: "What a task import cannot create"
status: draft
applies-to:
  - "UC-PRJ-IMPORT-TASKS"
uses-terms:
  - "TERM-TASK-VISIBILITY"
provenance:
  source: ".private/research/help-center/articles/39225660180626.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A task import cannot create private tasks, tasks assigned to all crew members, or new task statuses; imported tasks are always assigned to specific crew members.

## Rationale

Keeps imports deterministic.

## Examples

A row meant for all crew must be reassigned after import.

## Exceptions

None.
