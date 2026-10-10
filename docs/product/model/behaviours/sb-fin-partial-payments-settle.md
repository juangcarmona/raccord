---
id: SB-FIN-PARTIAL-PAYMENTS-SETTLE
type: structured-behaviour
title: "Two payments settle an invoice"
status: draft
illustrates:
  - "BR-FIN-OUTSTANDING-AMOUNT"
given:
  - "An invoice of 1,000.00 has a payment of 400.00 and is partially paid"
when: "A second payment of 600.00 is recorded"
then:
  - "The outstanding amount is 0.00"
  - "The invoice is paid"
uses-terms:
  - "TERM-OUTSTANDING-AMOUNT"
  - "TERM-PAYMENT-STATUS"
provenance:
  source: ".private/research/help-center/articles/18560784878610.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Partial payments accumulate.

## Boundaries

None.
