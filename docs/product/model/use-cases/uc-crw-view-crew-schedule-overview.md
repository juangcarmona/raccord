---
id: UC-CRW-VIEW-CREW-SCHEDULE-OVERVIEW
type: use-case
title: "Review a crew member's schedule overview"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by: []
uses-terms:
  - "TERM-CREW-SCHEDULE-CARD"
provenance:
  source: ".private/research/help-center/articles/17390187948690.md, .private/research/help-center/articles/360013323900.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

The planner understands a person's commitments before planning.

## Trigger

A planning decision about one person.

## Preconditions

The crew member exists.

## Main Flow

1. The planner opens the crew member.
2. The product shows days with events, and for a chosen date the projects and appointments, with the day number within multi-day jobs.
3. The planner opens a listed project if needed.

## Alternative Flows

Open invitations of the person are listed and can be answered, withdrawn or reminded.

## Failure Conditions

- The planner lacks permission to see this crew member's schedule: the overview is refused and nothing is shown.
- The crew member has no events on the chosen date: that day is shown empty.
- The product cannot be reached: the overview fails to load visibly and can be retried.

## Postconditions

No change.
