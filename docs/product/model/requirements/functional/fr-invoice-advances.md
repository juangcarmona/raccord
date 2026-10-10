---
id: FR-INVOICE-ADVANCES
type: functional-requirement
title: "Bill advances and deduct them automatically"
status: draft
derived-from:
  - "BR-INVOICE-BASIS"
  - "UC-FIN-GENERATE-PROJECT-INVOICE"
verification:
  - scenario-ref: "SB-ADVANCE-DEDUCTED-ON-FINAL"
uses-terms:
  - "TERM-INVOICE-DEDUCTION"
---

## Requirement

The product MUST let the billing administrator issue an advance invoice for a percentage of a project's total and MUST deduct issued advances automatically from that project's later invoices.

## Rationale

Decided by the product owner: invoices from lines and from advances are both supported.
