---
id: UC-CRW-PLAN-RESOURCE-ON-FUNCTION
type: use-case
title: "Plan crew members and vehicles on a function"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-PLANNABLE-FLAG"
  - "BR-CRW-DEFAULT-RATE-ON-ASSIGNMENT"
  - "BR-CRW-DEFAULT-PLAN-STATUS-NEW-ONLY"
  - "BR-CRW-STAFFING-STATUS-DERIVATION"
  - "BR-CRW-AVAILABILITY-ADVISORY"
  - "BR-CRW-AGENCY-MULTIPLE-BOOKING"
  - "BR-CRW-FUNCTIONS-WITHOUT-TIMES"
  - "BR-CRW-SHIFT-FUNCTION-OVERLAP-HOURS"
uses-terms:
  - "TERM-CREW-ASSIGNMENT"
  - "TERM-STAFFING-STATUS"
provenance:
  source: ".private/research/help-center/articles/360014687739.md, .private/research/help-center/articles/360014594620.md, .private/research/help-center/articles/360014372420.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Functions are staffed.

## Trigger

Availability is known or the planner decides.

## Preconditions

The function exists; resources are shown in planning.

## Main Flow

1. The planner selects a function.
2. The planner picks one or several crew members (crew function) or vehicles (transport function), possibly from crew lists.
3. The product creates assignments with the default plan status and the crew member's default rate, and updates the staffing status.

## Alternative Flows

- Several functions are planned at once on the timeline.
- Undated functions are planned only in the cross-project planner.

## Failure Conditions

Hidden resources cannot be chosen.

## Postconditions

Assignments exist.
