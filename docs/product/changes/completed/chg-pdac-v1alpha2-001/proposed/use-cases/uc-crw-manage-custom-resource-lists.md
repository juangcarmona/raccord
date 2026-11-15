---
id: UC-CRW-MANAGE-CUSTOM-RESOURCE-LISTS
type: use-case
title: "Manage custom crew and vehicle lists"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-CUSTOM-LISTS-SHARED"
  - "BR-CRW-DYNAMIC-LIST-MEMBERSHIP"
uses-terms:
  - "TERM-CREW-LIST"
provenance:
  source: ".private/research/help-center/articles/14924701052050.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Planners have reusable groups of people or vehicles.

## Trigger

Recurring team compositions.

## Preconditions

None.

## Main Flow

1. The planner creates a crew or vehicle list with title and description.
2. Members are chosen individually, by folder or by tags.
3. The product saves and shares the list.

## Alternative Flows

- Edit or delete a list (affects everyone).
- Hide or show a list for oneself only.
- Switch between lists with shortcuts.

## Failure Conditions

- The planner lacks permission to manage lists: the list is refused and nothing is saved.
- The list has no title, or a chosen folder or tag does not exist: it is refused with the reason and nothing is saved.
- Another planner edited or deleted the same list in the meantime: the change is refused and the planner must reload and retry; since edits and deletions affect everyone, they apply to the shared list only (BR-CRW-CUSTOM-LISTS-SHARED).
- A list defined by folder or tags with no matching members is saved but shows no members until some match (BR-CRW-DYNAMIC-LIST-MEMBERSHIP).

## Postconditions

The list is available to all planners.
