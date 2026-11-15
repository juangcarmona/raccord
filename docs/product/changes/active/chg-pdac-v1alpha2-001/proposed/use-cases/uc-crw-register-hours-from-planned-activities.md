---
id: UC-CRW-REGISTER-HOURS-FROM-PLANNED-ACTIVITIES
type: use-case
title: "Register hours from planned activities"
status: draft
primary-actor: "ACT-CREW-MEMBER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-PLANNED-ACTIVITIES-WINDOW"
  - "BR-CRW-DEFAULT-BREAK"
  - "BR-CRW-ACTIVITY-DURATION-CONSISTENCY"
  - "BR-CRW-DAYS-BACK-LIMIT"
  - "BR-CRW-TIME-REGISTRATION-RIGHTS"
uses-terms:
  - "TERM-PLANNED-ACTIVITY"
  - "TERM-REGISTRATION-ACTIVITY"
provenance:
  source: ".private/research/help-center/articles/360015498439.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Hours are registered quickly against scheduled work.

## Trigger

Scheduled work ended.

## Preconditions

The person was scheduled on functions or shifts.

## Main Flow

1. The user picks scheduled activities (in the mobile web app, from the list still to register).
2. The product fills from and until and the break.
3. The user adjusts durations and options.
4. The product saves; the activity shows as registered.

## Alternative Flows

Several activities registered together for selected crew members.

## Failure Conditions

Changed times without matching durations leave hours without activities.

## Postconditions

Registered activities are linked.
