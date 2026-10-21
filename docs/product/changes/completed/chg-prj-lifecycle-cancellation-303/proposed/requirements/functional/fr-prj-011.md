---
id: FR-PRJ-011
type: functional-requirement
title: "Cancel projects with follow-up actions"
status: active
derived-from:
  - "UC-PRJ-CANCEL-PROJECT"
  - "BR-PRJ-CANCELLATION-ACTIONS-APPLICABLE"
  - "BR-PRJ-CANCEL-RELEASES-EQUIPMENT"
  - "BR-PRJ-CANCEL-OPEN-ITEMS-WARNING"
  - "BR-PRJ-CANCEL-PRESERVES-INVOICED-WORK"
verification:
  - scenario-ref: "SB-PRJ-CANCEL-FREES-STOCK"
  - scenario-ref: "SB-PRJ-CANCEL-WITHOUT-ACTIONS-WARNS"
  - scenario-ref: "SB-PRJ-CANCEL-WITHDRAWS-INVITATIONS"
  - scenario-ref: "SB-PRJ-CANCEL-PRESERVES-INVOICED-WORK"
uses-terms:
  - "TERM-CANCELLATION-ACTION"
provenance:
  source: ".private/research/help-center/articles/28083930405778.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST, on cancelling a project, offer the applicable cancellation actions, perform those confirmed, release equipment reservations immediately and warn while crew or tasks remain open.

## Rationale

Last-minute cancellations must reach everyone affected.
