---
id: FR-COMPLIANCE-002
type: functional-requirement
title: "Report billing records to the tax authority"
status: draft
derived-from:
  - "UC-REPORT-BILLING-RECORDS"
  - "CON-SIF-REGULATION"
  - "CON-NO-COMPLIANCE-CLAIM"
verification:
  - scenario: "When the tax authority is unreachable, pending records are retried later with the same content and the administrator sees the incident"
uses-terms:
  - "TERM-COMPLIANCE-RECORD"
---

## Requirement

The product MUST report every compliance record as a chained billing record in issue order, keep the evidence of each response, retry unreachable submissions with identical content, and surface rejections for correction.

## Rationale

Required by Spanish invoicing-software regulation; implementation follows legal review.
