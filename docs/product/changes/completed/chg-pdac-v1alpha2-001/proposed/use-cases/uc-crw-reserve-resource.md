---
id: UC-CRW-RESERVE-RESOURCE
type: use-case
title: "Reserve a crew member or vehicle"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-RESERVATION-WITHOUT-DETAILS"
uses-terms:
  - "TERM-CREW-RESERVATION"
provenance:
  source: ".private/research/help-center/articles/13885448785682.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

People or vehicles are held for a busy period.

## Trigger

Demand is expected but not yet detailed.

## Preconditions

Resources exist.

## Main Flow

1. The planner selects crew members or vehicles and a period.
2. The planner chooses reservation, adjusts times and adds a remark.
3. The product records the reservation.

## Alternative Flows

Reservations through invitations (see UC-CRW-INVITE-CREW-FOR-PERIOD).

## Failure Conditions

- The planner lacks permission to reserve resources: the reservation is refused and nothing is recorded.
- No resource is selected, or the period is missing or invalid, such as an end before the start: it is refused with the reason and nothing is saved.
- A crew member or vehicle not shown in planning is chosen: it cannot be reserved (BR-CRW-PLANNABLE-FLAG).
- Another user changed the same resource's planning in the meantime: the reservation is refused and the planner must reload and retry.

## Postconditions

The reservation shows in the resource's timeline, the crew member's schedule and optionally the project timeline.
