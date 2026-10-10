---
id: TERM-TASK-STATUS
type: domain-term
title: "Task status"
status: draft
defined-in: "BC-PROJECTS"
synonyms: []
uses-terms:
  - "TERM-TASK"
  - "TERM-TASK-STATUS-TO-DO"
  - "TERM-TASK-STATUS-COMPLETED"
provenance:
  source: ".private/research/help-center/articles/38421530397202.md, .private/research/help-center/articles/360013703139.md, .private/research/help-center/articles/39225660180626.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The company-wide, ordered workflow of task states. Two states always exist: To do (start of every workflow) and Completed (end); both may be renamed and recoloured but never removed. Administrators may add intermediate states (for example In progress, Review) with a name and colour, reorder them (the order is the board column order) and delete them, choosing a replacement for tasks that hold the deleted state.

Transitions: any user may move a task they can see to any state; bulk advancing moves selected tasks with an identical state to the next state in the order; setting a completed task back to an earlier state is allowed and loses nothing. A task is complete exactly when its state is Completed. When status workflows are first enabled, incomplete tasks become To do and done tasks remain Completed. External calendars see only two states: Completed and not completed.

## Distinguish From

Project status: lifecycle of a whole project.

## Usage

Board columns, filters, imports (matched by name).
