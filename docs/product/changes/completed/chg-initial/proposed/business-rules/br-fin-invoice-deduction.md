---
id: BR-FIN-INVOICE-DEDUCTION
type: business-rule
title: "Earlier invoices can be deducted from a new one"
status: draft
applies-to:
  - "UC-FIN-GENERATE-PROJECT-INVOICE"
  - "UC-FIN-CREATE-CREDIT-NOTE"
uses-terms:
  - "TERM-INVOICE-DEDUCTION"
provenance:
  source: ".private/research/help-center/articles/360013965120.md, .private/research/help-center/articles/360017648580.md, .private/research/help-center/articles/360017696859.md, .private/research/help-center/articles/360017700379.md"
  confidence: high
  recovered-from: documentation
---

## Rule

An invoice or credit note may list earlier invoices to subtract; their amounts are deducted from its total so staged invoices together equal the project price.

## Rationale

The final invoice must charge only the remainder.

## Examples

See SB-FIN-FINAL-INVOICE-DEDUCTS-DOWN-PAYMENT.

## Exceptions

None.
