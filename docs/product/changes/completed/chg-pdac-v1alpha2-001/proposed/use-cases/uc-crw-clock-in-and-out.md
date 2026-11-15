---
id: UC-CRW-CLOCK-IN-AND-OUT
type: use-case
title: "Clock in and out on the mobile web app"
status: draft
primary-actor: "ACT-CREW-MEMBER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-TIME-REGISTRATION-RIGHTS"
uses-terms:
  - "TERM-REGISTRATION-CAPTURE-MODE"
provenance:
  source: ".private/research/help-center/articles/360015498439.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Project hours are captured live with a timer for costing; this is not the legal workday record (UC-CLOCK-WORKDAY).

## Trigger

Starting work.

## Preconditions

The role allows clocking in from the mobile web app.

## Main Flow

1. The crew member clocks in; a timer runs, also in the background.
2. They stop the timer.
3. They complete options and activities and save.

## Alternative Flows

None.

## Failure Conditions

Without the right clock-in is not offered.

## Postconditions

A registration exists.
