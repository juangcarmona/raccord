---
id: UC-CRW-ADD-FUNCTION-TO-PROJECT
type: use-case
title: "Add functions to a project"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors:
  - "ACT-OPERATIONS-COORDINATOR"
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-ONE-OFF-NOT-REUSABLE"
  - "BR-CRW-DEFAULT-FUNCTION-COPY"
  - "BR-CRW-FUNCTION-TIMING-PROPAGATION"
  - "BR-CRW-EXCLUDE-FROM-PRICE"
  - "BR-CRW-FUNCTION-KIND-PRICING"
uses-terms:
  - "TERM-PROJECT-FUNCTION"
  - "TERM-ONE-OFF-FUNCTION"
  - "TERM-DEFAULT-FUNCTION"
  - "TERM-FUNCTION-TIMING-MODE"
provenance:
  source: ".private/research/help-center/articles/360013223940.md, .private/research/help-center/articles/14232209807506.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

The project states which roles are needed, how many and when.

## Trigger

Quoting or preparing a job.

## Preconditions

The project exists.

## Main Flow

1. The planner selects a function group or the ungrouped set.
2. The planner adds a default function, or creates a one-off crew or transport function.
3. The planner sets quantity and timing: linked to schedule periods, own times, a duration or no date.
4. The product adds it and uses it for the quote.

## Alternative Flows

A one-off function is converted to a default function for reuse.

## Failure Conditions

- The planner tries to add a one-off function from another project: it is refused because a one-off function belongs only to the project where it was created, unless it is first converted into a default function (BR-CRW-ONE-OFF-NOT-REUSABLE).
- Quantity, timing or the price and cost basis is missing or inconsistent for the function kind (for example a break length on a transport function): it is refused with the reason and the function is not added (BR-CRW-FUNCTION-KIND-PRICING).
- The planner lacks permission to change the project's planning: the function is refused and the project is unchanged.
- Another user changed the project in the meantime: the addition is refused and the planner must reload and retry.

## Postconditions

Functions exist and await planning.
