---
id: UC-CRW-PLAN-RESOURCE-ON-SHIFT
type: use-case
title: "Plan crew members and vehicles on a shift"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-RECURRING-SHIFT-PLANNING"
  - "BR-CRW-SHIFT-AVAILABILITY-BLOCKING"
  - "BR-CRW-PLANNABLE-FLAG"
  - "BR-CRW-SHIFT-FUNCTION-OVERLAP-HOURS"
uses-terms:
  - "TERM-SHIFT"
  - "TERM-CREW-ASSIGNMENT"
provenance:
  source: ".private/research/help-center/articles/17592602608018.md, .private/research/help-center/articles/26061527515538.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Shifts are staffed.

## Trigger

A shift needs people or vehicles.

## Preconditions

The shift exists.

## Main Flow

1. The planner selects the shift and chooses crew members or vehicles.
2. The planner plans them as final or draft.
3. The product creates the assignments.

## Alternative Flows

- Planning a person on all recurrences or only future ones.
- Creating a vehicle shift directly from a vehicle's timeline.

## Failure Conditions

- The planner lacks permission to plan on shifts: the assignment is refused and nothing is created.
- A crew member or vehicle not shown in planning is chosen: it cannot be planned (BR-CRW-PLANNABLE-FLAG).
- The chosen crew member is already unavailable because of another blocking shift or other work in that time: the planning is refused with the reason (BR-CRW-SHIFT-AVAILABILITY-BLOCKING).
- Another user changed the shift in the meantime: the planning is refused and the planner must reload and retry.

## Postconditions

Assignments exist on the shift.
