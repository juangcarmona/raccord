---
id: QR-PRJ-CLOCK-IN-REMINDER
type: quality-requirement
title: "Reminder after 12 hours clocked in"
status: draft
quality-attribute: "timeliness"
applies-to:
  - "UC-PRJ-TRACK-TASK-TIME"
verification:
  - scenario-ref: "SB-PRJ-FORGOTTEN-TIMER-REMINDER"
uses-terms:
  - "TERM-TRACKED-TIME"
provenance:
  source: ".private/research/help-center/articles/38423178380050.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

A person clocked in on a task for more than 12 hours MUST receive reminders through notifications in the product and by e-mail.

## Measurement

Start a timer, let 12 hours and one minute pass: a reminder is delivered on both channels.
