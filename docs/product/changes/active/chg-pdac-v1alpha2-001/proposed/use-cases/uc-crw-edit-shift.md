---
id: UC-CRW-EDIT-SHIFT
type: use-case
title: "Edit or delete a shift"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-RECURRING-EDIT-SCOPE"
uses-terms:
  - "TERM-SHIFT"
provenance:
  source: ".private/research/help-center/articles/17297296257426.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Shifts remain correct.

## Trigger

Internal work changes.

## Preconditions

The shift exists.

## Main Flow

1. The planner edits or deletes the shift.
2. For a recurring shift the planner chooses which occurrences are affected.
3. The product applies the change.

## Alternative Flows

None.

## Failure Conditions

Start and end times cannot be bulk-edited across occurrences.

## Postconditions

Shifts reflect the change.
