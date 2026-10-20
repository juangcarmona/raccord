---
id: BR-PRJ-TASK-CREATION-BY-BOARD-ACCESS
type: business-rule
title: "Task creation in a project follows board access"
status: active
applies-to:
  - "UC-PRJ-ADD-TASK"
uses-terms:
  - "TERM-PROJECT-TASK-BOARD"
provenance:
  source: ".private/research/help-center/articles/360013703139.md, .private/research/help-center/articles/38421530397202.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A user may add and edit tasks in a project if they can see that project's task board, even without project edit rights.

## Rationale

Collaborators contribute to tasks without full project access.

## Examples

A crew member with board access but no project edit rights adds a task.

## Exceptions

Installations without structured task work keep task creation tied to the record.
