---
id: FR-PRJ-025
type: functional-requirement
title: "Track time on tasks"
status: active
derived-from:
  - "UC-PRJ-TRACK-TASK-TIME"
  - "BR-PRJ-SINGLE-RUNNING-TIMER"
  - "BR-PRJ-LONG-CLOCK-IN-REMINDER"
  - "BR-PRJ-TASK-HOURS-APPROVAL"
verification:
  - scenario-ref: "SB-PRJ-FORGOTTEN-TIMER-REMINDER"
  - scenario: "A timer started on the mobile web app can be stopped on the web"
uses-terms:
  - "TERM-TRACKED-TIME"
provenance:
  source: ".private/research/help-center/articles/38423178380050.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let users register durations or run a single timer on tasks, feed the hours into approvals, time registrations and services, and remind users clocked in over 12 hours.

## Rationale

Estimates must be compared with reality.
