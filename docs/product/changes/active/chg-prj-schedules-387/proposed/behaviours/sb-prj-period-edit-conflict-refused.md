---
id: SB-PRJ-PERIOD-EDIT-CONFLICT-REFUSED
type: structured-behaviour
title: An unavailable period edit leaves linked planning unchanged
status: active
illustrates:
  - UC-PRJ-MANAGE-TIME-SCHEDULE
  - BR-PRJ-PERIOD-EDIT-AVAILABILITY
given:
  - A project period is linked to equipment and crew planning
  - The proposed period edit would make a linked equipment reservation unavailable
when: The coordinator saves the period edit
then:
  - The product reports the availability conflict
  - The period retains its original times
  - Linked equipment groups, reservations and functions retain their original times
uses-terms:
  - TERM-TIME-PERIOD
  - TERM-RESERVATION
---

## Intent

Keep the project schedule and linked planning consistent when an individual period edit conflicts with equipment availability.

## Boundaries

This scenario covers an individual period edit. Whole-project shifts follow the separate rule proposed by #304.
