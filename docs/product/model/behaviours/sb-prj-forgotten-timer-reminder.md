---
id: SB-PRJ-FORGOTTEN-TIMER-REMINDER
type: structured-behaviour
title: "Forgotten timer triggers reminders"
status: draft
illustrates:
  - "BR-PRJ-LONG-CLOCK-IN-REMINDER"
  - "UC-PRJ-TRACK-TASK-TIME"
given:
  - "A crew member started a task timer at 08:00"
when: "The timer is still running at 20:01 the same day"
then:
  - "The crew member receives reminders by notification in the product and by e-mail"
uses-terms:
  - "TERM-TRACKED-TIME"
provenance:
  source: ".private/research/help-center/articles/38423178380050.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Threshold of 12 hours.

## Boundaries

None.
