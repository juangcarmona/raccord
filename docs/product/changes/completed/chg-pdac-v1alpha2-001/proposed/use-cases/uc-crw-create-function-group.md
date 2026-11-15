---
id: UC-CRW-CREATE-FUNCTION-GROUP
type: use-case
title: "Create a function group"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-FUNCTION-TIMING-PROPAGATION"
uses-terms:
  - "TERM-FUNCTION-GROUP"
  - "TERM-TIME-SCHEDULE"
provenance:
  source: ".private/research/help-center/articles/14319493249170.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Functions are grouped and share times.

## Trigger

Organising a project's roles.

## Preconditions

The project exists.

## Main Flow

1. The planner selects time-schedule periods and copies them as function groups, or creates a group with a name and usage and planning period.
2. The product creates the group.

## Alternative Flows

The group is given only a duration.

## Failure Conditions

- The planner lacks permission to change the project's planning: the group is refused and nothing is created.
- The group has no name, or its usage or planning period is invalid, such as an end before the start: it is refused with the reason and nothing is saved.
- A group linked to schedule periods takes its times from them, so own times cannot be set on it at the same time (BR-CRW-FUNCTION-TIMING-PROPAGATION).
- Another user changed the project in the meantime: the creation is refused and the planner must reload and retry.

## Postconditions

Functions added to the group follow its times.
