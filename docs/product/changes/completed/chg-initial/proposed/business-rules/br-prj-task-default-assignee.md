---
id: BR-PRJ-TASK-DEFAULT-ASSIGNEE
type: business-rule
title: "The creator is the default assignee"
status: draft
applies-to:
  - "UC-PRJ-ADD-TASK"
uses-terms:
  - "TERM-TASK"
provenance:
  source: ".private/research/help-center/articles/360013703139.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A new task is assigned to its creator unless the assignee is changed; it may be assigned to nobody, to all crew or to several specific crew members.

## Rationale

Most tasks are notes to self.

## Examples

Creating a task without touching the assignee assigns it to oneself.

## Exceptions

Imported tasks follow their file.
