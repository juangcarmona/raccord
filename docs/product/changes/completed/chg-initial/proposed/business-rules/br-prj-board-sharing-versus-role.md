---
id: BR-PRJ-BOARD-SHARING-VERSUS-ROLE
type: business-rule
title: "Sharing gives task access, roles give rights"
status: draft
applies-to:
  - "UC-PRJ-SHARE-PROJECT-TASK-BOARD"
uses-terms:
  - "TERM-PROJECT-TASK-BOARD"
provenance:
  source: ".private/research/help-center/articles/38422661951506.md, .private/research/help-center/articles/38422735121170.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Sharing a board decides which tasks a person can see and edit; the person's role decides what they may do (for example deleting others' tasks); sharing never adds rights. People on a board may change statuses, create tasks and edit others' tasks on it; outside shared boards only their role permits this.

## Rationale

Separates scope from permission.

## Examples

A shared colleague without delete rights cannot delete tasks on the board.

## Exceptions

None.
