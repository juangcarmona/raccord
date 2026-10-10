---
id: FR-CORRECTION-001
type: functional-requirement
title: "Correct issued invoices with rectifying invoices"
status: draft
derived-from:
  - "UC-CORRECT-ISSUED-INVOICE"
  - "BR-ISSUED-INVOICE-IMMUTABLE"
  - "BR-RECTIFYING-INVOICE"
verification:
  - scenario-ref: "SB-RECTIFY-BY-DIFFERENCES"
  - scenario-ref: "SB-RECTIFY-BY-SUBSTITUTION"
uses-terms:
  - "TERM-RECTIFYING-INVOICE"
---

## Requirement

The product MUST let the billing administrator correct an issued invoice only by issuing a rectifying invoice that references it.

## Rationale

Corrections without editing issued records.
