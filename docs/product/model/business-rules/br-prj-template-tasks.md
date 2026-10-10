---
id: BR-PRJ-TEMPLATE-TASKS
type: business-rule
title: "Template tasks are created on every new project"
status: draft
applies-to:
  - "UC-PRJ-CREATE-PROJECT-TEMPLATE"
  - "UC-PRJ-CREATE-FROM-TEMPLATE"
uses-terms:
  - "TERM-TASK"
  - "TERM-TASK-DEADLINE"
provenance:
  source: ".private/research/help-center/articles/360013703139.md, .private/research/help-center/articles/38421530397202.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Tasks stored in a template are created on each project made from it, including status, planned time, subtasks and relative deadlines recalculated against the new project's schedule.

## Rationale

Recurring work is not forgotten.

## Examples

A 'send rider' task due three days before load-in gets a different date on every new project.

## Exceptions

None.
