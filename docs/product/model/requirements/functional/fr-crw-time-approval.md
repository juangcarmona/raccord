---
id: FR-CRW-TIME-APPROVAL
type: functional-requirement
title: "Approve time registrations"
status: draft
derived-from:
  - "UC-CRW-APPROVE-TIME-REGISTRATIONS"
  - "UC-CRW-REVERT-TIME-REGISTRATION-APPROVAL"
  - "BR-CRW-APPROVAL-ROUTING"
  - "BR-CRW-APPROVED-READ-ONLY"
  - "BR-CRW-PENDING-NOT-IN-TOTAL"
  - "BR-CRW-APPROVAL-CHECK-THRESHOLDS"
  - "BR-CRW-MINIMUM-PAID-HOURS-CHECK"
  - "BR-CRW-REGISTRATION-HISTORY"
verification:
  - scenario-ref: "SB-CRW-NEW-REGISTRATION-PENDING"
  - scenario-ref: "SB-CRW-REVERT-THEN-EDIT"
  - scenario-ref: "SB-CRW-APPROVAL-CHECK-SMALL-DIFFERENCE"
  - scenario-ref: "SB-CRW-APPROVAL-CHECK-LARGE-DIFFERENCE"
  - scenario-ref: "SB-CRW-BELOW-MINIMUM-PAID"
uses-terms:
  - "TERM-TIME-REGISTRATION-STATUS"
  - "TERM-APPROVAL-CHECK"
provenance:
  source: ".private/research/help-center/articles/27336532785298.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST hold registrations of approval-requiring hour types as pending, show approval signals, allow single, bulk and sequential approval, make approved registrations read-only until reverted, and log every change.

## Rationale

Payroll accuracy.
