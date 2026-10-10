---
id: FR-FIN-CREDIT-NOTES
type: functional-requirement
title: "Correct invoices with credit notes"
status: draft
derived-from:
  - "UC-FIN-CREATE-CREDIT-NOTE"
  - "BR-FIN-CREDIT-NOTE-CORRECTION"
  - "BR-FIN-CREDIT-NOTE-SERIES"
verification:
  - scenario: "A credit note for part of a finalized invoice leaves the original unchanged and reduces the amount owed."
uses-terms:
  - "TERM-CREDIT-NOTE"
provenance:
  source: ".private/research/help-center/articles/360017696859.md, .private/research/help-center/articles/6819935599378.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST create credit notes that fully or partly cancel a finalized invoice, or document a freestanding refund, without changing the original.

## Rationale

Finalized invoices are immutable.
