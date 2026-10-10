---
id: FR-INVOICE-003
type: functional-requirement
title: "Calculate amounts, VAT breakdown and totals"
status: draft
derived-from:
  - "BR-MONEY-ROUNDING"
  - "BR-VAT-PER-RATE"
verification:
  - scenario-ref: "SB-VAT-BASE-ROUNDING"
  - scenario-ref: "SB-MIXED-RATE-TOTALS"
uses-terms:
  - "TERM-VAT-BREAKDOWN"
---

## Requirement

The product MUST calculate line net amounts, the VAT breakdown per rate and the invoice totals, and every place that shows them MUST show the product's calculated values.

## Rationale

One calculation, everywhere, avoids cent differences between screens and documents.
