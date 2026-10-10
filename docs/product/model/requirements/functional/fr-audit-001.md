---
id: FR-AUDIT-001
type: functional-requirement
title: "Keep an append-only audit trail"
status: draft
derived-from:
  - "BR-AUDITED-ACTIONS"
  - "UC-REVIEW-AUDIT-TRAIL"
verification:
  - scenario: "Issuing an invoice records who issued it, when, its number, issue date, total and fingerprint, followed by the storage of its document"
  - scenario: "Attempts to change or delete audit events are refused"
uses-terms:
  - "TERM-AUDIT-EVENT"
---

## Requirement

The product MUST record an audit event for each audited action in the same operation as the action, MUST show the trail per record, and MUST prevent changing or deleting audit events.

## Rationale

Traceability of financial actions.
