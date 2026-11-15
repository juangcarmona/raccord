---
id: UC-CRW-SWITCH-PLAN-STATUS
type: use-case
title: "Switch planning between draft and final"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-DEFAULT-PLAN-STATUS-NEW-ONLY"
  - "BR-CRW-DRAFT-INVISIBLE"
  - "BR-CRW-STAFFING-STATUS-DERIVATION"
  - "BR-CRW-DRAFT-INVITATION-EFFECT"
uses-terms:
  - "TERM-PLAN-STATUS"
provenance:
  source: ".private/research/help-center/articles/360014607700.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Crew see planning only when it is ready.

## Trigger

Planning is finished or reopened.

## Preconditions

Assignments exist.

## Main Flow

1. The planner selects one assignment or a whole function.
2. The planner switches between draft and final.
3. The product updates visibility and staffing status.

## Alternative Flows

The planner sets the default plan status for new assignments.

## Failure Conditions

- The planner lacks permission to change planning: the switch is refused and the status stays as it was.
- Another user changed the same assignment in the meantime: the switch is refused and the planner must reload and retry.
- Changing the default plan status does not change assignments that already exist; they keep their status (BR-CRW-DEFAULT-PLAN-STATUS-NEW-ONLY).
- An assignment still in draft stays hidden from the crew member and the function is not shown as fully planned to crew until it is made final (BR-CRW-DRAFT-INVISIBLE, BR-CRW-STAFFING-STATUS-DERIVATION).

## Postconditions

Final assignments are visible to crew; draft ones are not.
