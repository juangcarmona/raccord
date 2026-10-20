---
id: SB-PRJ-DRAG-REASSIGNS
type: structured-behaviour
title: "Dragging between assignee columns reassigns"
status: active
illustrates:
  - "BR-PRJ-BOARD-MOVE-EDITS-TASK"
given:
  - "A board is grouped by assignee"
  - "A task is in Ana's column"
when: "The user drags it to Ben's column"
then:
  - "The task is assigned to Ben"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/38422661951506.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Visual moves are real edits.

## Boundaries

None.
