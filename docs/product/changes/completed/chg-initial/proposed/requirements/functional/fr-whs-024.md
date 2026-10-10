---
id: FR-WHS-024
type: functional-requirement
title: "Ready-to-pack handover"
status: draft
derived-from:
  - "UC-WHS-MARK-READY-TO-PACK"
  - "BR-WHS-READY-TO-PACK-VISIBILITY"
  - "BR-WHS-READY-TO-PACK-LOCK"
  - "BR-WHS-BOOKING-MARKS-READY"
verification:
  - scenario-ref: "SB-WHS-NOT-READY-HIDDEN"
  - scenario-ref: "SB-WHS-READY-TO-PACK-LOCKED"
  - scenario-ref: "SB-WHS-BOOKING-UNMARKED-SUBPROJECT"
uses-terms:
  - "TERM-READY-TO-PACK"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST gate warehouse visibility of projects behind a ready-to-pack flag (with a role permission to see all), lock it after first booking, and set it automatically after warning when an unmarked project is booked.

## Rationale

Clean handover.
