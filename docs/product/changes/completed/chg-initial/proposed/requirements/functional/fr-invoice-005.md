---
id: FR-INVOICE-005
type: functional-requirement
title: "Number invoices correlatively per series and year"
status: draft
derived-from:
  - "BR-GAPLESS-NUMBERING"
  - "BR-ISSUE-DATE-ORDER"
  - "CON-SPANISH-INVOICE-CONTENT"
verification:
  - scenario-ref: "SB-FIRST-INVOICE-NUMBER"
  - scenario-ref: "SB-BACKDATED-ISSUE-REFUSED"
  - scenario-ref: "SB-CONCURRENT-ISSUING-GAPLESS"
uses-terms:
  - "TERM-INVOICE-NUMBER"
---

## Requirement

The product MUST allocate invoice numbers only at issue, correlatively and without gaps per series and calendar year, and MUST refuse issue dates earlier than the last invoice of the series.

## Rationale

Legal correlative numbering.
