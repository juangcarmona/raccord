---
id: UC-PRJ-PROGRESS-TASK
type: use-case
title: "Move a task through its statuses"
status: draft
primary-actor: "ACT-CREW-MEMBER"
supporting-actors:
  - "ACT-OPERATIONS-COORDINATOR"
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-TASK-STATUS-ANY-USER"
  - "BR-PRJ-TASK-BULK-ADVANCE"
  - "BR-PRJ-TASK-COMPLETION-REVERSIBLE"
  - "BR-PRJ-TASK-OVERDUE"
  - "BR-PRJ-BOARD-MOVE-EDITS-TASK"
uses-terms:
  - "TERM-TASK-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013703139.md, .private/research/help-center/articles/38422661951506.md, .private/research/help-center/articles/38421530397202.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The team sees where each task stands.

## Trigger

Work on a task starts, advances or finishes.

## Preconditions

The user can see the task.

## Main Flow

1. The user changes the task's status in the task, by dragging its card to another column, or by advancing selected tasks to the next status.
2. The product updates the task everywhere it appears.

## Alternative Flows

- Completing by the completion control.
- Completed by mistake: the user sets an earlier status again.

## Failure Conditions

Bulk advance with mixed statuses is not offered.

## Postconditions

Status updated; overdue marking ends on completion.
