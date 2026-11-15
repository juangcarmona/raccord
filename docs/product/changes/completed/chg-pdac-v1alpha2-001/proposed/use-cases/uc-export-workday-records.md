---
id: UC-EXPORT-WORKDAY-RECORDS
type: use-case
title: "Provide workday records to employees and the inspectorate"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors:
  - "ACT-CREW-MEMBER"
bounded-context: "BC-CREW"
governed-by:
  - "BR-WORKDAY-RETENTION"
uses-terms:
  - "TERM-WORKDAY-RECORD"
---

## Goal

Records are available to whoever is legally entitled to them.

## Trigger

An employee, a legal representative or the labour inspectorate asks for records.

## Preconditions

Records exist for the requested period.

## Main Flow

1. Each employee can always see and download their own records.
2. An administrator chooses employees and a period.
3. The product produces a signed export with daily times, breaks, totals and corrections.

## Alternative Flows

- An administrator grants the labour inspectorate time-limited, read-only remote access to the records of chosen employees and periods; every access is logged.

## Failure Conditions

- A user without the permission can only export their own records.

## Postconditions

The export reflects the records exactly, including corrections.
