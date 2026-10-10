---
id: FR-CRW-TIME-RECONCILIATION
type: functional-requirement
title: "Reconcile planned and registered time"
status: draft
derived-from:
  - "UC-CRW-RECONCILE-PLANNED-AND-REGISTERED-TIME"
  - "BR-CRW-RECONCILIATION-STATUS-DERIVATION"
  - "BR-CRW-ACTUAL-COST-FROM-REGISTRATIONS"
  - "BR-CRW-OVERTIME-FROM-REGISTERED-HOURS"
verification:
  - scenario-ref: "SB-CRW-RECONCILIATION-EXCEEDS"
  - scenario-ref: "SB-CRW-RECONCILIATION-NO-REGISTRATION"
uses-terms:
  - "TERM-RECONCILIATION-STATUS"
provenance:
  source: ".private/research/help-center/articles/23814779870610.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST show for each activity planned, registered and difference time, planned, actual and difference cost and a reconciliation status, per project, registration and crew member.

## Rationale

Budget control and fair pay.
