---
id: UC-PRJ-WORK-WITH-TASK-BOARD
type: use-case
title: "Work with a task board"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors:
  - "ACT-CREW-MEMBER"
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-BOARD-IS-VIEW"
  - "BR-PRJ-BOARD-MOVE-EDITS-TASK"
  - "BR-PRJ-BOARD-GROUPING-SCOPE"
  - "BR-PRJ-BOARD-HIDING-PERSONAL"
uses-terms:
  - "TERM-TASK-BOARD"
  - "TERM-SAVED-VIEW"
provenance:
  source: ".private/research/help-center/articles/38422661951506.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The user sees what is in progress, late and who does what.

## Trigger

Daily coordination.

## Preconditions

The user has access to the board.

## Main Flow

1. The user opens a project or team board in board or list view.
2. The user filters, groups and subgroups, orders cards within a column, and adds columns such as planned and tracked time.
3. The user saves the arrangement as a view, or saves grouping to the board for everyone.

## Alternative Flows

- The user hides boards not relevant to them and shows hidden boards again.

## Failure Conditions

- A user without access to a board does not see it or its tasks.
- Moving a card to a column the task cannot take, or without permission to edit the task, is refused and the task is unchanged (BR-PRJ-BOARD-MOVE-EDITS-TASK).
- If another user changed the same task in the meantime, the move is refused and the user must reload and retry.
- Saving grouping to the board for everyone without the right is refused; it stays a personal view (BR-PRJ-BOARD-GROUPING-SCOPE).
- Hiding a board never changes its tasks or other users (BR-PRJ-BOARD-HIDING-PERSONAL).

## Postconditions

The board reflects the chosen arrangement.
