---
id: UC-CRW-CHANGE-ASSIGNMENT-DETAILS
type: use-case
title: "Change a planned resource's details"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-DEFAULT-RATE-ON-ASSIGNMENT"
  - "BR-CRW-TRAVEL-TIME-EXTENDS-CALL"
  - "BR-CRW-USAGE-VERSUS-PLANNING-PERIOD"
uses-terms:
  - "TERM-CREW-ASSIGNMENT"
  - "TERM-TRAVEL-OPTION"
provenance:
  source: ".private/research/help-center/articles/360014687739.md, .private/research/help-center/articles/360016488759.md, .private/research/help-center/articles/360014594620.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

An assignment carries the right rate, times, travel and remark.

## Trigger

Individual arrangements.

## Preconditions

The assignment exists.

## Main Flow

1. The planner opens the assignment.
2. The planner sets project leader, crew rate, travel option, planning period and remark.
3. The product saves; communicated times include travel.

## Alternative Flows

Bulk-editing the travel option of several assignments.

## Failure Conditions

- The planner lacks permission to change the assignment: the change is refused and nothing is saved.
- A rate not linked to the person, a travel option the function cannot support, or an invalid planning period is entered: it is refused with the reason and nothing is saved (BR-CRW-DEFAULT-RATE-ON-ASSIGNMENT, BR-CRW-TRAVEL-TIME-EXTENDS-CALL).
- Another user changed the same assignment in the meantime: the change is refused and the planner must reload and retry.

## Postconditions

The assignment's cost and communicated times reflect the change.
