---
id: UC-CRW-RETIME-FUNCTION
type: use-case
title: "Change a function's planning period"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-USAGE-VERSUS-PLANNING-PERIOD"
  - "BR-CRW-RETIME-WITH-PLANNED-RESOURCES"
  - "BR-CRW-NO-AUTOMATIC-SCHEDULE-NOTIFICATION"
uses-terms:
  - "TERM-CREW-PLANNING-PERIOD"
provenance:
  source: ".private/research/help-center/articles/360014594620.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Crew are called at different times without changing the price.

## Trigger

Crew must arrive earlier or leave later.

## Preconditions

The function exists.

## Main Flow

1. The planner edits the function's planning times.
2. If resources are planned, the planner chooses to apply the new period to them, keep their periods, or remove them.
3. The product saves.

## Alternative Flows

Per assignment: the planner changes one crew member's planning period only.

## Failure Conditions

- The planner lacks permission to change planning: the new period is refused and nothing changes.
- The new period is invalid, such as an end before the start: it is refused with the reason and nothing is saved.
- Resources are planned and the planner has not chosen to apply, keep or remove them: the retiming is not saved until a choice is made (BR-CRW-RETIME-WITH-PLANNED-RESOURCES).
- A function's usage period cannot be changed on a single function; the price changes only through the group's usage period (BR-CRW-USAGE-PERIOD-EDIT-AT-GROUP).
- Crew are not notified of the new times by themselves (BR-CRW-NO-AUTOMATIC-SCHEDULE-NOTIFICATION).
- Another user changed the function in the meantime: the change is refused and the planner must reload and retry.

## Postconditions

Occupancy and planned cost follow the new period; the price is unchanged.
