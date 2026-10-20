---
id: CHG-PRJ-TASK-TIME-388
type: product-change
title: Accept project task time tracking and comparison workflows
status: proposed
base-revision: "5a92fa0cc2ad8841410e007f10391e469492915f"
operations:
  add:
    - SB-PRJ-PLANNED-VS-ACTUAL-TASK-HOURS
  modify:
    - UC-PRJ-TRACK-TASK-TIME
    - UC-PRJ-REPORT-TASK-TIME
    - FR-PRJ-025
    - BR-PRJ-SINGLE-RUNNING-TIMER
    - BR-PRJ-LONG-CLOCK-IN-REMINDER
    - BR-PRJ-TASK-HOURS-APPROVAL
    - BR-PRJ-TIME-REGISTRATION-SYNC
    - BR-PRJ-NON-PROJECT-TASK-HOURS
    - BR-PRJ-SERVICE-BREAKDOWN
    - BR-PRJ-UNAPPROVED-HOURS-EXPORT-WARNING
  remove: []
---

## Problem

The Product Definition already describes task time entry, approval, registration synchronization and planned-versus-tracked reporting, but those workflows and their governing rules remain drafts. Issue #388 cannot be implemented against an accepted product contract while these artifacts are draft.

## Intended Product Outcome

Crew members can record durations or use one synchronized task timer, with tracked effort flowing into approval, time-registration and service totals as defined by the existing rules. Coordinators can compare planned and tracked task hours, inspect service breakdowns and export only information they are authorized to see, with a visible warning when exports contain unapproved hours.

## Rationale

Issue #388 turns existing product intent into user-facing project time workflows. Accepting the existing rules before implementation keeps time entry, approval, service aggregation and reporting aligned without changing payroll, invoicing or the meaning of existing time records.

## Affected Product Areas

Project task time entry and timers, approval and time registration, service effort breakdowns, planned-versus-tracked reporting and time exports.

## Open Questions

- Confirm the supported task and service types that users may record time against in the web workflow; the issue says supported project tasks and services but does not enumerate them. Recommendation: use the currently modeled task and service relationships and do not introduce new task types.
- Confirm whether the first web release includes starting and stopping timers as well as manually entering durations. Recommendation: include both because they are explicit alternatives in UC-PRJ-TRACK-TASK-TIME; cover each with browser evidence where the user journey permits.
- Confirm the export format and whether export is required in this issue's first slice. Recommendation: defer export-format expansion and keep the accepted warning, visibility and retry rules; #388's required browser journey focuses on recording and comparing time.

## Product Acceptance

- The Product Owner resolves each open question and approves this Product Change.
- The accepted definition makes the two use cases, their requirement and the referenced timer, approval, synchronization, service-breakdown and export-warning rules active and coherent.
- The #388 browser journey can demonstrate synthetic time entry and a planned-versus-tracked review while preserving access boundaries and source records.

## Out of Scope

Changing time-accounting semantics, payroll, invoice calculations or invoice lifecycle; implementing project requests, schedules, board behavior or general service setup; adding new task types; technical design, code, release or deployment.
