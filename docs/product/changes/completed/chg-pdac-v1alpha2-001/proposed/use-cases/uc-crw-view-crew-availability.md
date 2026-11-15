---
id: UC-CRW-VIEW-CREW-AVAILABILITY
type: use-case
title: "View crew availability for a function"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-AVAILABILITY-DERIVATION"
  - "BR-CRW-PARTIAL-AVAILABILITY"
uses-terms:
  - "TERM-AVAILABILITY-STATE"
provenance:
  source: ".private/research/help-center/articles/360014685859.md, .private/research/help-center/articles/360014594620.md, .private/research/help-center/articles/360014625560.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

The planner sees who can work at a function's time.

## Trigger

Planning a function.

## Preconditions

Functions and crew exist.

## Main Flow

1. The planner selects a function or period.
2. The product shows each crew member or vehicle as available, unavailable, invited, reserved or unknown, and a timeline with planning, appointments and reservations.

## Alternative Flows

Viewed on a crew member's schedule overview or within a project.

## Failure Conditions

- The planner lacks permission to see crew availability: the view is refused and nothing is shown.
- Crew with any known clash within the function's period are shown as unavailable, even if free for the rest of it (BR-CRW-PARTIAL-AVAILABILITY).
- Crew with no declared availability, planning or answer for the period are shown as unknown rather than refused (BR-CRW-AVAILABILITY-DERIVATION).
- No crew or vehicles match: the view is shown empty.

## Postconditions

No change.
