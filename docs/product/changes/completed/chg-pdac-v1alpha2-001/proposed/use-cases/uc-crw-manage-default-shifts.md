---
id: UC-CRW-MANAGE-DEFAULT-SHIFTS
type: use-case
title: "Manage default shifts"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-DEFAULT-SHIFT-COPY"
uses-terms:
  - "TERM-DEFAULT-SHIFT"
provenance:
  source: ".private/research/help-center/articles/17290277763090.md, .private/research/help-center/articles/17297296257426.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Reusable shift templates exist.

## Trigger

Repeated kinds of internal work.

## Preconditions

None.

## Main Flow

1. The planner creates, edits or deletes a default shift with name, duration, break, optional start and end, blocking flag, quantity, remarks and tags.
2. The product saves.

## Alternative Flows

None.

## Failure Conditions

- The planner lacks permission to manage default shifts: the change is refused and nothing is saved.
- The name is missing or the duration, break or start and end are invalid, for example a break longer than the duration: it is refused with the reason and nothing is saved.
- Another user changed or deleted the same default shift in the meantime: the change is refused and the planner must reload and retry.
- Editing or deleting a default shift does not alter shifts already created from it (BR-CRW-DEFAULT-SHIFT-COPY).

## Postconditions

Shifts can be created from the template.
