---
id: SB-MIXED-RATE-TOTALS
type: structured-behaviour
title: "Totals with two VAT rates"
status: draft
illustrates:
  - "BR-VAT-PER-RATE"
  - "BR-MONEY-ROUNDING"
given:
  - "A draft has one line of 1,200.00 and one line of 4 × 75.50 with a 10 percent discount at 21 percent VAT, and one line of 2 × 100.00 at 10 percent VAT"
when: "Its totals are calculated"
then:
  - "The 21 percent group has base 1,471.80 and VAT 309.08"
  - "The 10 percent group has base 200.00 and VAT 20.00"
  - "The invoice total is 2,000.88"
  - "The groups are listed from the highest rate"
uses-terms: []
---

## Intent

Fixes the reconciliation of bases, quotas and total.

## Boundaries

None.
