---
id: FR-CRW-103
type: functional-requirement
title: "Review a crew member's schedule before planning"
status: draft
derived-from:
  - "UC-CRW-VIEW-CREW-SCHEDULE-OVERVIEW"
verification:
  - id: "CREW-SCHEDULE-OVERVIEW"
    scenario: "Given a crew member booked on a three-day job and an appointment on another date, when a planner opens the member's overview, the days with events are marked, and choosing the job date lists the project with its day number (for example day 2 of 3) and choosing the other date lists the appointment; the view changes nothing."
  - id: "CREW-OPEN-INVITATIONS"
    scenario: "Given a crew member with an open invitation, when the planner opens the overview, the invitation is listed and can be answered, withdrawn or reminded."
uses-terms:
  - "TERM-CREW-SCHEDULE-CARD"
provenance:
  source: ".private/research/help-center/articles/17390187948690.md, .private/research/help-center/articles/360013323900.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST show a planner, for a chosen crew member, which days hold events and, for a chosen date, the projects and appointments of that person including the day number within multi-day jobs, and MUST let the planner open a listed project. The product MUST also list the person's open invitations with the ability to answer, withdraw or remind them, and MUST NOT change any data by viewing.

## Rationale

A planner can only staff fairly and without conflict when a person's existing commitments are visible before the decision.
