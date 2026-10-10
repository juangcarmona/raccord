---
id: SB-ADVANCE-DEDUCTED-ON-FINAL
type: structured-behaviour
title: "An advance is deducted from the final invoice"
status: draft
illustrates:
  - "BR-INVOICE-BASIS"
given:
  - "A project totals 10,000.00 before VAT"
  - "An advance invoice of 30 percent (3,000.00 before VAT) has been issued"
when: "The billing administrator issues the final invoice for all remaining lines of the project"
then:
  - "The final invoice deducts the 3,000.00 advance"
  - "The final invoice charges 7,000.00 before VAT"
  - "The advance is marked as consumed"
uses-terms: []
---

## Intent

Staged billing never charges twice.

## Boundaries

VAT is calculated on each invoice's own taxable base.
