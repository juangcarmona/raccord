---
id: UC-PRJ-ADD-TASK
type: use-case
title: "Create a task"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors:
  - "ACT-CREW-MEMBER"
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-TASK-NAME-REQUIRED"
  - "BR-PRJ-TASK-DEFAULT-ASSIGNEE"
  - "BR-PRJ-TASK-CREATION-BY-BOARD-ACCESS"
  - "BR-PRJ-TASK-VISIBILITY-RULES"
  - "BR-PRJ-SUBTASK-LIMITS"
  - "BR-PRJ-TASK-RELATIVE-DEADLINE"
  - "BR-PRJ-TASK-ASSIGNEE-PROJECT-VIEW"
  - "BR-PRJ-TASK-NOT-IN-CREW-PLANNING"
uses-terms:
  - "TERM-TASK"
  - "TERM-TASK-DEADLINE"
  - "TERM-TASK-VISIBILITY"
  - "TERM-SUBTASK"
provenance:
  source: ".private/research/help-center/articles/360013703139.md, .private/research/help-center/articles/38421530397202.md, .private/research/help-center/articles/360013628759.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Work to be done is recorded with owner and deadline.

## Trigger

Work, reminder or follow-up is identified.

## Preconditions

The user can see the relevant board or task area.

## Main Flow

1. The user creates a task from a record, the task area, a project's board, a board column or a details panel.
2. The user enters a name and optionally deadline, status, priority, colour, details, assignees, visibility, planned time, teams, subtasks, notes, files, tags, custom fields and service.
3. The product saves, assigning the creator by default.

## Alternative Flows

- Created in a project from a template: created automatically.

## Failure Conditions

A task without name is refused.

## Postconditions

The task appears in all boards and lists that match it.
