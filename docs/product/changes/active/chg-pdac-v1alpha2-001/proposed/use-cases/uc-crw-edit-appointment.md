---
id: UC-CRW-EDIT-APPOINTMENT
type: use-case
title: "Edit or delete an appointment"
status: draft
primary-actor: "ACT-CREW-MEMBER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-RECURRING-EDIT-SCOPE"
uses-terms:
  - "TERM-APPOINTMENT"
provenance:
  source: ".private/research/help-center/articles/23976860498834.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Appointments stay accurate.

## Trigger

A commitment changes or is cancelled.

## Preconditions

The appointment exists.

## Main Flow

1. The user opens the appointment.
2. The user changes or deletes it.
3. For a recurring appointment the user chooses which occurrences are affected.
4. The product applies the change.

## Alternative Flows

None.

## Failure Conditions

- The user lacks the right to change this appointment: the edit or deletion is refused and nothing changes.
- For a recurring appointment, a change of start or end time is attempted across several occurrences: it is refused, and times can be changed only one occurrence at a time (BR-CRW-RECURRING-EDIT-SCOPE).
- Invalid values, such as a missing name or an end before the start: the edit is refused with the reason and nothing is saved.
- Another user changed or deleted the appointment in the meantime: the change is refused and the user must reload and retry.

## Postconditions

Schedules reflect the change.
