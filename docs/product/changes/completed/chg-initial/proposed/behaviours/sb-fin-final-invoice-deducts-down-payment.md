---
id: SB-FIN-FINAL-INVOICE-DEDUCTS-DOWN-PAYMENT
type: structured-behaviour
title: "Final invoice deducts the down payment"
status: draft
illustrates:
  - "BR-FIN-INVOICE-DEDUCTION"
given:
  - "A 10,000.00 project has a down payment invoice of 3,000.00"
when: "The administrator generates the final invoice for 100% and subtracts the down payment invoice"
then:
  - "The final invoice amounts to 7,000.00 before VAT"
uses-terms:
  - "TERM-INVOICE-DEDUCTION"
provenance:
  source: ".private/research/help-center/articles/360013965120.md, .private/research/help-center/articles/360017700379.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Staged invoices add up to the price.

## Boundaries

None.
