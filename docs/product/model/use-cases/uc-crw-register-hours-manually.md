---
id: UC-CRW-REGISTER-HOURS-MANUALLY
type: use-case
title: "Register hours manually"
status: draft
primary-actor: "ACT-CREW-MEMBER"
supporting-actors:
  - "ACT-CREW-PLANNER"
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-TIME-REGISTRATION-RIGHTS"
  - "BR-CRW-DAYS-BACK-LIMIT"
  - "BR-CRW-DEFAULT-BREAK"
  - "BR-CRW-BREAK-AND-TRAVEL-EXCLUDED"
  - "BR-CRW-HOUR-TYPE-TOTALS"
  - "BR-CRW-OVERTIME-FROM-REGISTERED-HOURS"
uses-terms:
  - "TERM-TIME-REGISTRATION"
  - "TERM-HOUR-TYPE"
provenance:
  source: ".private/research/help-center/articles/360015498439.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Worked time is recorded.

## Trigger

Work was done.

## Preconditions

The user has rights for the persons concerned.

## Main Flow

1. The user enters from and until.
2. The user selects the crew members concerned.
3. The user sets break, optional travel time, hour type, distance and lunch.
4. The user adds activities, linked to functions or shifts of that day or as free text.
5. The product saves the registration, pending if approval is required.

## Alternative Flows

Save and start another registration.

## Failure Conditions

Outside the days-back window or without rights: refused.

## Postconditions

A registration exists.
