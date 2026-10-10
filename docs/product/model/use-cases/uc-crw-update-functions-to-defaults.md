---
id: UC-CRW-UPDATE-FUNCTIONS-TO-DEFAULTS
type: use-case
title: "Update project functions to current defaults"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-DEFAULT-FUNCTION-COPY"
uses-terms:
  - "TERM-DEFAULT-FUNCTION"
provenance:
  source: ".private/research/help-center/articles/34615607831314.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Project functions use the latest default data.

## Trigger

Defaults changed, for example new yearly expenses, after the project was created or duplicated.

## Preconditions

The project has functions from defaults.

## Main Flow

1. The planner selects functions in the project.
2. The planner chooses which fields to update.
3. The product overwrites those fields from the defaults.

## Alternative Flows

None.

## Failure Conditions

- The planner lacks permission to change the project's functions: the update is refused and nothing is overwritten.
- No function or no field is selected, or a selected function no longer has a default to take values from: the update is refused with the reason and nothing is changed (BR-CRW-DEFAULT-FUNCTION-COPY).
- Another user changed the same functions in the meantime: the update is refused and the planner must reload and retry.
- Functions and fields that are not selected keep their current values, so a quoted project is never changed silently (BR-CRW-DEFAULT-FUNCTION-COPY).

## Postconditions

Only selected fields of selected functions match the defaults.
