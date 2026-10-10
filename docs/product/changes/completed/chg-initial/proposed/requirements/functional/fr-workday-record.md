---
id: FR-WORKDAY-RECORD
type: functional-requirement
title: "Keep a legal workday record for every employee"
status: draft
derived-from:
  - "UC-CLOCK-WORKDAY"
  - "UC-CORRECT-WORKDAY-RECORD"
  - "UC-EXPORT-WORKDAY-RECORDS"
  - "BR-WORKDAY-APPEND-ONLY"
  - "BR-WORKDAY-OPEN-DAY"
  - "BR-WORKDAY-RETENTION"
  - "CON-WORKDAY-RECORD-REGULATION"
verification:
  - scenario-ref: "SB-WORKDAY-CORRECTION-KEEPS-ORIGINAL"
  - scenario-ref: "SB-WORKDAY-FORGOTTEN-CLOCK-OUT"
uses-terms:
  - "TERM-WORKDAY-RECORD"
---

## Requirement

The product MUST let every employee clock in and out and record breaks at the moment they happen, MUST keep those records append-only and tamper-evident with audited corrections for at least four years, and MUST provide them to employees, by export, and through time-limited read-only remote access for the labour inspectorate.

## Rationale

Spanish labour law; decided by the product owner as a must-have.
