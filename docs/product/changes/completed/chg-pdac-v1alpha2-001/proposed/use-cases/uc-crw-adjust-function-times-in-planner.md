---
id: UC-CRW-ADJUST-FUNCTION-TIMES-IN-PLANNER
type: use-case
title: "Move and resize functions across projects"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-PLANNER-MOVE-UPDATES-PROJECT"
  - "BR-CRW-FUNCTIONS-WITHOUT-TIMES"
uses-terms:
  - "TERM-PROJECT-FUNCTION"
  - "TERM-FUNCTION-TIMING-MODE"
provenance:
  source: ".private/research/help-center/articles/360014625560.md, .private/research/help-center/articles/360014610140.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Function times are adjusted directly on the cross-project timeline.

## Trigger

Plans shift.

## Preconditions

Functions exist in the period shown.

## Main Flow

1. The planner moves a function, changes its start or end, or moves several selected functions together.
2. The product updates the times in the projects.

## Alternative Flows

An unscheduled function is placed on the timeline, receiving a date.

## Failure Conditions

- The planner lacks permission to change planning: the move or resize is refused and times stay unchanged.
- The new times are invalid, for example an end before the start: the change is refused with the reason and nothing is saved.
- Another user changed the same function in the meantime: the change is refused and the planner must reload and retry.
- A function without any times cannot be moved or resized until it is first placed on the timeline and given a date (BR-CRW-FUNCTIONS-WITHOUT-TIMES).

## Postconditions

Times are updated; prices unchanged; cost differences show in project financials.
