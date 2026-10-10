---
id: FR-CRW-CONFLICTS
type: functional-requirement
title: "Expose and resolve planning conflicts"
status: draft
derived-from:
  - "UC-CRW-RESOLVE-PLANNING-CONFLICTS"
  - "BR-CRW-NO-AUTOMATIC-SCHEDULE-NOTIFICATION"
verification:
  - scenario-ref: "SB-CRW-DOUBLE-BOOKING-AFTER-SHIFT"
  - scenario-ref: "SB-CRW-CHANGE-NOT-NOTIFIED"
uses-terms:
  - "TERM-PLANNING-CONFLICT"
provenance:
  source: ".private/research/help-center/articles/14488851931026.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

The product MUST make double bookings and assignments during unavailability visible and let planners remove all or selected assignments to re-plan.

## Rationale

Shifting projects breaks staffing.
