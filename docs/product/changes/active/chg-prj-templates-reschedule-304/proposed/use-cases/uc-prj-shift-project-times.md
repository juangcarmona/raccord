---
id: UC-PRJ-SHIFT-PROJECT-TIMES
type: use-case
title: "Reschedule project times"
status: active
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-SHIFT-BY-OFFSET"
  - "BR-PRJ-SHIFT-TO-START"
  - "BR-PRJ-LINKED-PERIOD-PROPAGATION"
  - "BR-PRJ-SHIFT-RECHECKS-AVAILABILITY"
uses-terms:
  - "TERM-TIME-PERIOD"
provenance:
  source: ".private/research/help-center/articles/13992531637522.md, .private/research/help-center/articles/360017700399.md, .private/research/help-center/articles/360013599680.md"
  confidence: high
  recovered-from: documentation
---

## Goal

All or some periods of a project move consistently after a date change or delay.

## Trigger

The job is postponed, brought forward or delayed.

## Preconditions

The project has periods.

## Main Flow

1. The coordinator selects the whole project schedule or one or more periods.
2. The coordinator chooses to move them by a signed amount (minutes, hours, days, months or years), or to a new start date-time.
3. The product previews the resulting times.
4. The coordinator applies the change; the product checks availability and atomically updates the periods and everything linked to them, or leaves all data unchanged when the new schedule is unavailable.

## Alternative Flows

- The coordinator drags a period on the timeline or edits start and end directly.

## Failure Conditions

The coordinator abandons the preview: nothing changes.

## Postconditions

Periods and linked planning have new times; availability is recalculated.
