---
id: FR-FIN-PAYMENTS
type: functional-requirement
title: "Record full and partial payments"
status: draft
derived-from:
  - "UC-FIN-RECORD-PAYMENT"
  - "BR-FIN-OUTSTANDING-AMOUNT"
  - "BR-FIN-DUE-DATE-FROM-TERM"
verification:
  - scenario-ref: "SB-FIN-PARTIAL-PAYMENTS-SETTLE"
  - scenario-ref: "SB-FIN-DUE-DATE-14-DAYS"
uses-terms:
  - "TERM-PAYMENT"
  - "TERM-OUTSTANDING-AMOUNT"
provenance:
  source: ".private/research/help-center/articles/18560784878610.md, .private/research/help-center/articles/360014214060.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST record payments with date, method and amount, compute the outstanding amount and payment status, and derive due dates from payment terms.

## Rationale

Receivables must be known.
