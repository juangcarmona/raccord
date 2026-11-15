---
id: UC-CLOCK-WORKDAY
type: use-case
title: "Clock in and out of the working day"
status: draft
primary-actor: "ACT-CREW-MEMBER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-WORKDAY-APPEND-ONLY"
  - "BR-WORKDAY-OPEN-DAY"
uses-terms:
  - "TERM-WORKDAY-RECORD"
---

## Goal

The employee's working day is recorded as the law requires.

## Trigger

The employee starts or ends work or a break.

## Preconditions

The employee has a user account.

## Main Flow

1. The employee clocks in from the mobile web app or an office computer; the server time is recorded.
2. The employee starts and ends breaks as they happen.
3. The employee clocks out; the day becomes Finished and its worked time is shown.

## Alternative Flows

- The employee may use the recorded day to prefill project hours (time registration).

## Failure Conditions

- Clocking in twice without clocking out is refused; the open day is shown instead.
- Without a network connection the clock event is not recorded and the employee is told to retry.

## Postconditions

The day's entries exist and cannot be altered, only corrected.
