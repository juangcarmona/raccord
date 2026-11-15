---
id: UC-CRW-FIND-CREW-WITH-LISTS
type: use-case
title: "Find suitable crew with lists"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-TAG-MATCH-EXACT"
  - "BR-CRW-AVAILABLE-LIST-ORDER"
  - "BR-CRW-DYNAMIC-LIST-MEMBERSHIP"
uses-terms:
  - "TERM-CREW-LIST"
provenance:
  source: ".private/research/help-center/articles/12866138319506.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

The planner sees suitable people for a function.

## Trigger

Staffing a function.

## Preconditions

A function is selected.

## Main Flow

1. The planner opens a list: planned in project, available, matching tags or a custom list.
2. The product shows its members for the function's time.
3. The planner plans from the list.

## Alternative Flows

The list view runs in a second window kept in sync with function, period and plan status selection.

## Failure Conditions

- The planner lacks permission to see crew for planning: the list is refused and no members are shown.
- A matching-tags list is requested for a function whose tags differ from the crew's tags in spelling or form: those crew are not listed (BR-CRW-TAG-MATCH-EXACT).
- The list has no members for the function's time: it is shown empty.
- Crew or vehicles not shown in planning never appear in any list and cannot be planned from one (BR-CRW-PLANNABLE-FLAG).

## Postconditions

No change unless the planner plans.
