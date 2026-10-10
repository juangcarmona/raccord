---
id: SB-VAT-BASE-ROUNDING
type: structured-behaviour
title: "VAT is rounded once per rate"
status: draft
illustrates:
  - "BR-VAT-PER-RATE"
given:
  - "A draft has three lines of 0.15 each at a 10 percent VAT rate"
when: "Its VAT breakdown is calculated"
then:
  - "The taxable base at 10 percent is 0.45"
  - "The VAT amount at 10 percent is 0.05"
uses-terms: []
---

## Intent

Per-line rounding would give 0.06; the product uses the base-level result.

## Boundaries

None.
