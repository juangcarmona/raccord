---
id: BR-WORKDAY-APPEND-ONLY
type: business-rule
title: "Workday records are append-only"
status: draft
applies-to:
  - "UC-CLOCK-WORKDAY"
  - "UC-CORRECT-WORKDAY-RECORD"
uses-terms:
  - "TERM-WORKDAY-RECORD"
---

## Rule

Clock events take the server's time and can never be edited or deleted; every correction is a new entry with reason, requester and approver, and the original remains visible.

## Rationale

CON-WORKDAY-RECORD-REGULATION requires reliable, tamper-evident records.

## Examples

See SB-WORKDAY-CORRECTION-KEEPS-ORIGINAL.

## Exceptions

None.
