---
id: UC-CRW-ADD-APPOINTMENT
type: use-case
title: "Add an appointment"
status: draft
primary-actor: "ACT-CREW-MEMBER"
supporting-actors:
  - "ACT-CREW-PLANNER"
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-SELF-PRESELECTED"
  - "BR-CRW-RECURRENCE-SERIES"
  - "BR-CRW-AVAILABILITY-RIGHTS"
uses-terms:
  - "TERM-APPOINTMENT"
provenance:
  source: ".private/research/help-center/articles/20884029308306.md, .private/research/help-center/articles/23917302179218.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

A personal commitment is in the schedules of the people concerned.

## Trigger

A meeting or private engagement.

## Preconditions

The user may add appointments for the selected people.

## Main Flow

1. The user selects dates in the personal schedule.
2. The user names the appointment and optionally sets colour, location, times, recurrence, remark and other participants.
3. The product creates it.

## Alternative Flows

None.

## Failure Conditions

- The user lacks the right to add appointments for themselves or for the selected other people: the request is refused and no appointment is created (BR-CRW-AVAILABILITY-RIGHTS).
- A required entry such as the name or date is missing or invalid, or a recurrence has no usable pattern: the appointment is refused with the reason and nothing is saved (BR-CRW-RECURRENCE-SERIES).
- The user deselects everyone, leaving no participant: the appointment is refused until at least one person is selected (BR-CRW-SELF-PRESELECTED).

## Postconditions

The appointment appears in the participants' schedules.
