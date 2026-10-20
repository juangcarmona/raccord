---
id: UC-PRJ-VIEW-PERSONAL-SCHEDULE
type: use-case
title: "Consult the personal schedule"
status: active
primary-actor: "ACT-CREW-MEMBER"
supporting-actors:
  - "ACT-OPERATIONS-COORDINATOR"
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-CALENDAR-QUICK-CREATE"
  - "BR-PRJ-OVERVIEW-DAY-SCOPE"
uses-terms:
  - "TERM-PERSONAL-SCHEDULE"
  - "TERM-PROJECT-DAY-KIND"
provenance:
  source: ".private/research/help-center/articles/360013084979.md, .private/research/help-center/articles/39016808810258.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A person sees their commitments and the projects' key days.

## Trigger

Planning a day or answering a customer on the phone.

## Preconditions

The user has a profile.

## Main Flow

1. The user shows appointments, scheduled functions and project days (equipment out, in, need crew, need transport), for self or, with rights, colleagues.
2. The product shows availability colours.
3. The user may start a project from a day.

## Alternative Flows

- Days limited to usage period.

## Failure Conditions

- Viewing colleagues' schedules without the right is refused; only the user's own schedule is shown.
- A day with no commitments shows empty.
- Starting a project from a day without permission to create projects is refused and nothing is created (BR-PRJ-CALENDAR-QUICK-CREATE).
- Projects outside the chosen day scope are not shown (BR-PRJ-OVERVIEW-DAY-SCOPE).

## Postconditions

None (read-only).
