---
id: SB-PRJ-SCHEDULE-AND-PERSONAL-SCHEDULE-JOURNEY
type: structured-behaviour
title: "Edit a project schedule and view assigned work personally"
status: active
illustrates:
  - "UC-PRJ-MANAGE-TIME-SCHEDULE"
  - "UC-PRJ-VIEW-PERSONAL-SCHEDULE"
given:
  - "A project has planning and usage periods with an equipment group and a crew function linked to a period"
  - "A team member is assigned to the crew function"
when: "The coordinator changes the period time and the team member opens their personal schedule"
then:
  - "The period and its linked equipment and crew planning show the updated time"
  - "The team member can see their assigned function on the personal schedule"
  - "The team member cannot see another person's schedule without the required permission"
uses-terms:
  - "TERM-TIME-PERIOD"
  - "TERM-PERSONAL-SCHEDULE"
  - "TERM-RESERVATION"
---

## Intent

Demonstrates that project planning and a crew member's personal view stay aligned after a schedule edit.

## Boundaries

This is the normal successful edit path. An unavailable edit is refused by SB-PRJ-PERIOD-EDIT-CONFLICT-REFUSED; whole-project shifts and templates remain under #304.
