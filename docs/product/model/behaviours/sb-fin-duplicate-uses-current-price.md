---
id: SB-FIN-DUPLICATE-USES-CURRENT-PRICE
type: structured-behaviour
title: "Duplicate reflects today's project price"
status: draft
illustrates:
  - "BR-FIN-DUPLICATE-RECOMPUTES"
given:
  - "An invoice for 1,000.00 exists"
  - "The project price has since risen to 1,200.00"
when: "The administrator duplicates the invoice"
then:
  - "The duplicate amounts to 1,200.00"
  - "Document text matches the original"
uses-terms:
  - "TERM-PROJECT-INVOICE"
provenance:
  source: ".private/research/help-center/articles/360013965120.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Duplicates are not copies of amounts.

## Boundaries

None.
