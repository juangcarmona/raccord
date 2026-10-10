---
id: BR-FIN-OUTSTANDING-AMOUNT
type: business-rule
title: "Outstanding amount and paid status follow recorded payments"
status: draft
applies-to:
  - "UC-FIN-RECORD-PAYMENT"
uses-terms:
  - "TERM-OUTSTANDING-AMOUNT"
  - "TERM-PAYMENT-STATUS"
provenance:
  source: ".private/research/help-center/articles/18560784878610.md, .private/research/help-center/articles/360017700379.md, .private/research/help-center/articles/360017696859.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The outstanding amount of an invoice is its total minus all recorded payments; when it reaches zero the invoice becomes paid, and recording the full outstanding amount in one step settles it.

## Rationale

Payment status must follow money actually received.

## Examples

See SB-FIN-PARTIAL-PAYMENTS-SETTLE.

## Exceptions

On credit notes payments may be negative or positive.
