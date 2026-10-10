---
id: BR-PRJ-PROJECT-BOARD-VISIBILITY
type: business-rule
title: "Who sees a project task board"
status: draft
applies-to:
  - "UC-PRJ-SHARE-PROJECT-TASK-BOARD"
uses-terms:
  - "TERM-PROJECT-TASK-BOARD"
provenance:
  source: ".private/research/help-center/articles/38422661951506.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Only users entitled to edit every project, plus those the board was explicitly shared with, can open a project task board; holding a task on the board does not give access to it.

## Rationale

Assignment gives the task, not the wider picture.

## Examples

See SB-PRJ-ASSIGNEE-WITHOUT-BOARD.

## Exceptions

None.
