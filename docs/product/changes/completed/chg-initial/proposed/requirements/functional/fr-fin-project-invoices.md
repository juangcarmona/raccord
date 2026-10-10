---
id: FR-FIN-PROJECT-INVOICES
type: functional-requirement
title: "Generate full and partial project invoices"
status: draft
derived-from:
  - "UC-FIN-GENERATE-PROJECT-INVOICE"
  - "BR-FIN-INVOICE-SCOPE"
  - "BR-FIN-INVOICE-DEDUCTION"
  - "BR-FIN-EXTRA-LINE-SIGN"
verification:
  - scenario-ref: "SB-FIN-PARTIAL-INVOICE-PERCENTAGE"
  - scenario-ref: "SB-FIN-FINAL-INVOICE-DEDUCTS-DOWN-PAYMENT"
uses-terms:
  - "TERM-PROJECT-INVOICE"
provenance:
  source: ".private/research/help-center/articles/360013965120.md, .private/research/help-center/articles/360017648580.md, .private/research/help-center/articles/360017700379.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST generate invoices for a whole project or a percentage, with extra positive or negative lines and deductions of earlier invoices.

## Rationale

Billing must follow the commercial agreement.
