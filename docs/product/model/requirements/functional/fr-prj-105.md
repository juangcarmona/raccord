---
id: FR-PRJ-105
type: functional-requirement
title: "Consult one's schedule and the key project days"
status: draft
derived-from:
  - "UC-PRJ-VIEW-PERSONAL-SCHEDULE"
  - "BR-PRJ-CALENDAR-QUICK-CREATE"
  - "BR-PRJ-OVERVIEW-DAY-SCOPE"
verification:
  - id: "PERSONAL-SCHEDULE"
    scenario: "Given a user with an appointment, a scheduled function and a project with equipment-out day, when the user opens the schedule, all are shown with availability colours; with the right to see colleagues the same view can be shown for a colleague, and a project can be started from a chosen day."
uses-terms:
  - "TERM-PERSONAL-SCHEDULE"
  - "TERM-PROJECT-DAY-KIND"
provenance:
  source: ".private/research/help-center/articles/360013084979.md, .private/research/help-center/articles/39016808810258.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST show a user appointments, scheduled functions and project days (equipment out, equipment in, need crew, need transport) for themselves or, with the right, for colleagues, MUST show availability colours, MUST allow limiting days to the usage period and MUST let the user start a project from a day.

## Rationale

A person planning a day or answering a customer needs commitments and key project days in one place.
