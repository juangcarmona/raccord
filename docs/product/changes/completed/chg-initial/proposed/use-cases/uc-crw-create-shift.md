---
id: UC-CRW-CREATE-SHIFT
type: use-case
title: "Create a shift"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-SHIFTS-ARE-NON-PROJECT"
  - "BR-CRW-SHIFT-AVAILABILITY-BLOCKING"
  - "BR-CRW-DEFAULT-SHIFT-COPY"
  - "BR-CRW-RECURRENCE-SERIES"
  - "BR-CRW-RECURRING-SHIFT-PLANNING"
uses-terms:
  - "TERM-SHIFT"
provenance:
  source: ".private/research/help-center/articles/17290277763090.md, .private/research/help-center/articles/17155945414674.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Internal work is scheduled and can be staffed.

## Trigger

Warehouse, delivery or office work is needed.

## Preconditions

None.

## Main Flow

1. The planner picks a date or a resource's timeline period, optionally starting from a default shift.
2. The planner sets name, times, total break, recurrence, availability blocking, quantity, crew, vehicles, remarks, tags and custom fields.
3. The product creates the shift or series.

## Alternative Flows

Recurring without end date: one shift only.

## Failure Conditions

- The planner lacks permission to plan shifts: the shift is refused and nothing is created.
- Name or times are missing or invalid, or a recurrence has no usable pattern: the shift is refused with the reason and nothing is saved (BR-CRW-RECURRENCE-SERIES).
- A recurring shift has no end date: only one shift is created rather than a series (BR-CRW-RECURRENCE-SERIES).
- A chosen crew member or vehicle is unavailable because of an overlapping blocking shift, or is not shown in planning: they cannot be planned on it (BR-CRW-SHIFT-AVAILABILITY-BLOCKING, BR-CRW-PLANNABLE-FLAG).

## Postconditions

Shifts exist with any selected resources planned.
