---
id: FR-COMPLIANCE-001
type: functional-requirement
title: "Record the reporting obligation of every issued invoice"
status: draft
derived-from:
  - "BR-COMPLIANCE-OBLIGATION"
  - "CON-SIF-REGULATION"
verification:
  - scenario: "Issuing an invoice creates exactly one pending compliance record for it"
  - scenario: "Deleting a compliance record is refused"
uses-terms:
  - "TERM-COMPLIANCE-RECORD"
---

## Requirement

The product MUST create a compliance record together with every issued invoice and MUST never delete compliance records.

## Rationale

No invoice escapes future reporting.
