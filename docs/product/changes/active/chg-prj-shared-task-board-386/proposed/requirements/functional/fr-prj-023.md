---
id: FR-PRJ-023
type: functional-requirement
title: "Task boards and sharing"
status: active
derived-from:
  - "UC-PRJ-WORK-WITH-TASK-BOARD"
  - "UC-PRJ-SHARE-PROJECT-TASK-BOARD"
  - "BR-PRJ-BOARD-IS-VIEW"
  - "BR-PRJ-BOARD-MOVE-EDITS-TASK"
  - "BR-PRJ-PROJECT-BOARD-VISIBILITY"
  - "BR-PRJ-EXTERNAL-BOARD-INVITE"
verification:
  - scenario-ref: "SB-PRJ-TASK-ON-TWO-BOARDS"
  - scenario-ref: "SB-PRJ-DRAG-REASSIGNS"
  - scenario-ref: "SB-PRJ-ASSIGNEE-WITHOUT-BOARD"
  - scenario-ref: "SB-PRJ-EXTERNAL-SEES-BOARD-NOT-PROJECT"
uses-terms:
  - "TERM-TASK-BOARD"
  - "TERM-PROJECT-TASK-BOARD"
provenance:
  source: ".private/research/help-center/articles/38422661951506.md, .private/research/help-center/articles/38422735121170.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST provide a task board per project and team boards, in board and list views with filters, groups and views, and MUST let Admin and Office users share project boards with colleagues and invited outsiders without granting project access.

## Rationale

Shared picture of production work.
