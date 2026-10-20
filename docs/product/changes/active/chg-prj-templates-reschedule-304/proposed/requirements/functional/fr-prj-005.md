---
id: FR-PRJ-005
type: functional-requirement
title: "Reschedule project times"
status: active
derived-from:
  - "UC-PRJ-SHIFT-PROJECT-TIMES"
  - "BR-PRJ-SHIFT-BY-OFFSET"
  - "BR-PRJ-SHIFT-TO-START"
  - "BR-PRJ-SHIFT-RECHECKS-AVAILABILITY"
verification:
  - scenario-ref: "SB-PRJ-SHIFT-NEGATIVE-OFFSET"
  - scenario-ref: "SB-PRJ-SHIFT-TO-NEW-START"
  - scenario-ref: "SB-PRJ-SHIFT-CONFLICT-REFUSED"
uses-terms:
  - "TERM-TIME-PERIOD"
provenance:
  source: ".private/research/help-center/articles/13992531637522.md, .private/research/help-center/articles/360017700399.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST shift selected periods or the whole project by a signed amount or to a new start date-time with a preview before applying, MUST recheck availability atomically through equipment reservations, and MUST leave every period and reservation unchanged if the new schedule conflicts. It MUST allow moving and resizing periods directly.

## Rationale

Delays and postponements are routine.
