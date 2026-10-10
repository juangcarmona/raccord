---
id: FR-FIN-STANDALONE-DUPLICATE
type: functional-requirement
title: "Standalone and duplicated invoices"
status: draft
derived-from:
  - "UC-FIN-CREATE-STANDALONE-INVOICE"
  - "UC-FIN-DUPLICATE-INVOICE"
  - "BR-FIN-DUPLICATE-RECOMPUTES"
verification:
  - scenario-ref: "SB-FIN-DUPLICATE-USES-CURRENT-PRICE"
uses-terms:
  - "TERM-STANDALONE-INVOICE"
provenance:
  source: ".private/research/help-center/articles/360013965120.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST allow invoices without a project and duplication of invoices that copies configuration but recomputes content.

## Rationale

Not all sales are projects; repeated billing should be quick.
