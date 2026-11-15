---
id: UC-PRJ-TRACK-TASK-TIME
type: use-case
title: "Track time on a task"
status: draft
primary-actor: "ACT-CREW-MEMBER"
supporting-actors:
  - "ACT-RACCORD-AUTOMATION"
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-SINGLE-RUNNING-TIMER"
  - "BR-PRJ-LONG-CLOCK-IN-REMINDER"
  - "BR-PRJ-TASK-HOURS-APPROVAL"
  - "BR-PRJ-TIME-REGISTRATION-SYNC"
  - "BR-PRJ-NON-PROJECT-TASK-HOURS"
uses-terms:
  - "TERM-TRACKED-TIME"
provenance:
  source: ".private/research/help-center/articles/38423178380050.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Actual effort is recorded against the task.

## Trigger

Work on a task is done or starts.

## Preconditions

The user can see the task.

## Main Flow

1. The user enters one or more durations on the task, or starts the timer and stops it when finished (from the task, a board, the dashboard, the mobile web app or the global clock control).
2. The product adds the time to the task and, through it, to the linked service.
3. Optionally the user opens a time registration prefilled with the day's tracked work and saves it.

## Alternative Flows

- Adjusting details in the time registration updates the task's tracked time.

## Failure Conditions

Timer running more than 12 hours: reminders are sent.

## Postconditions

Tracked time is pending approval like other registered hours.
