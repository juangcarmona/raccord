---
id: UC-CRW-RETIME-FUNCTION-GROUP
type: use-case
title: "Change a function group's planning or usage period"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-USAGE-VERSUS-PLANNING-PERIOD"
  - "BR-CRW-USAGE-PERIOD-EDIT-AT-GROUP"
  - "BR-CRW-RETIME-WITH-PLANNED-RESOURCES"
uses-terms:
  - "TERM-FUNCTION-GROUP"
  - "TERM-FUNCTION-USAGE-PERIOD"
provenance:
  source: ".private/research/help-center/articles/360014594620.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

A whole group is retimed, with or without a price change.

## Trigger

The job's timing or charged period changes.

## Preconditions

The group exists.

## Main Flow

1. The planner edits the group's planning period, or its usage period to change the price.
2. If resources are planned, the planner chooses keep, apply, remove, or decide per function.
3. The product saves.

## Alternative Flows

None.

## Failure Conditions

- The planner lacks permission to change planning: the new period is refused and nothing changes.
- The new period is invalid, such as an end before the start: it is refused with the reason and nothing is saved.
- Resources are planned and the planner has not chosen keep, apply, remove or per function: the retiming is not saved until a choice is made (BR-CRW-RETIME-WITH-PLANNED-RESOURCES).
- Changing only the planning period never changes the customer price; the price changes only through the usage period (BR-CRW-USAGE-VERSUS-PLANNING-PERIOD).
- Another user changed the group in the meantime: the change is refused and the planner must reload and retry.

## Postconditions

Functions in the group carry the new times; price changes only with the usage period.
