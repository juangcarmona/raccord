---
id: UC-CRW-CONSULT-OWN-SCHEDULE
type: use-case
title: "Consult one's own schedule"
status: draft
primary-actor: "ACT-CREW-MEMBER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-DRAFT-INVISIBLE"
  - "BR-CRW-RESERVATION-WITHOUT-DETAILS"
uses-terms:
  - "TERM-PERSONAL-SCHEDULE"
provenance:
  source: ".private/research/help-center/articles/360014672919.md, .private/research/help-center/articles/360014685859.md, .private/research/help-center/articles/13885448785682.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

The crew member sees up-to-date work, appointments and reservations.

## Trigger

The crew member checks what is next.

## Preconditions

The crew member can log in.

## Main Flow

1. The crew member opens their personal schedule on the web or the mobile web app.
2. The product shows final assignments with remarks, appointments and reservations.
3. They open a project to see details and add notes or tasks.

## Alternative Flows

None.

## Failure Conditions

- Assignments still in draft are not shown, so work being prepared by the planner does not yet appear (BR-CRW-DRAFT-INVISIBLE).
- Nothing is planned in the chosen period: the schedule is shown empty.
- The product cannot be reached: the schedule fails to load visibly and can be retried.

## Postconditions

No change.
