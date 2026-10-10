---
id: SB-FIN-PARTIAL-INVOICE-PERCENTAGE
type: structured-behaviour
title: "Thirty percent down payment invoice"
status: draft
illustrates:
  - "BR-FIN-INVOICE-SCOPE"
given:
  - "A project totals 10,000.00 before VAT"
when: "The administrator enters 30% when generating the invoice"
then:
  - "The invoice amounts to 3,000.00 before VAT"
  - "The invoice records its 30% share"
uses-terms:
  - "TERM-PARTIAL-INVOICE"
provenance:
  source: ".private/research/help-center/articles/360013965120.md, .private/research/help-center/articles/360017700379.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Percentage invoicing.

## Boundaries

None.
