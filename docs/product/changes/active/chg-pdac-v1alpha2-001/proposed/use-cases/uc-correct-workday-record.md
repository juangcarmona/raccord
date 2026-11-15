---
id: UC-CORRECT-WORKDAY-RECORD
type: use-case
title: "Correct a workday record"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors:
  - "ACT-CREW-MEMBER"
bounded-context: "BC-CREW"
governed-by:
  - "BR-WORKDAY-APPEND-ONLY"
  - "BR-WORKDAY-OPEN-DAY"
uses-terms:
  - "TERM-WORKDAY-RECORD"
---

## Goal

A wrong or missing clock event is fixed without losing the original.

## Trigger

An employee forgot to clock out, or clocked at the wrong time.

## Preconditions

The person correcting has the permission to correct workday records.

## Main Flow

1. The employee requests a correction with the right time and a reason, or a manager starts one.
2. A manager approves it.
3. The product adds a correction entry referencing the original, with time, reason, requester and approver.

## Alternative Flows

- The manager rejects the request; nothing changes and the employee is informed.

## Failure Conditions

- A correction without a reason is refused.
- An employee cannot approve their own correction.

## Postconditions

The day shows the corrected times and the full history.
