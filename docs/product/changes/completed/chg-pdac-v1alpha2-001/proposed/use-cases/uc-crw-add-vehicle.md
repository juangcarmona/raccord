---
id: UC-CRW-ADD-VEHICLE
type: use-case
title: "Add or edit a vehicle"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-PLANNABLE-FLAG"
uses-terms:
  - "TERM-VEHICLE"
provenance:
  source: ".private/research/help-center/articles/360014373260.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

A vehicle can be planned on transport functions and shifts.

## Trigger

The company acquires or changes a vehicle.

## Preconditions

None.

## Main Flow

1. The planner enters registration number, seats, folder, tags and whether it is shown in planning.
2. The product saves the vehicle.

## Alternative Flows

Editing an existing vehicle follows the same steps.

## Failure Conditions

- The planner lacks permission to manage vehicles: the vehicle is refused and nothing is saved.
- Required details such as the registration number are missing or invalid: the vehicle is refused with the reason and nothing is saved.
- For an edit, another user changed the same vehicle in the meantime: the change is refused and the planner must reload and retry.
- A vehicle not shown in planning is not offered on transport functions or shifts, so trying to plan it there is refused (BR-CRW-PLANNABLE-FLAG).

## Postconditions

The vehicle is available for transport planning when shown in planning.
