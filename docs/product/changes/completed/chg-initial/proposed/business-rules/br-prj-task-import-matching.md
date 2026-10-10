---
id: BR-PRJ-TASK-IMPORT-MATCHING
type: business-rule
title: "Task import matches statuses, people and items"
status: draft
applies-to:
  - "UC-PRJ-IMPORT-TASKS"
uses-terms:
  - "TERM-TASK"
  - "TERM-TASK-STATUS"
provenance:
  source: ".private/research/help-center/articles/39225660180626.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A task import matches status by exact name (never creating statuses), assignees by e-mail, full name or external reference, priorities by a user-defined value mapping, and linked items by identifier, name, number or code; a row with unknown status or assignee fails; a non-unique link value links the most recently created match.

## Rationale

Imports create only tasks and subtasks.

## Examples

See SB-PRJ-TASK-IMPORT-UNKNOWN-STATUS.

## Exceptions

None.
