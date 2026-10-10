---
id: FR-WHS-005
type: functional-requirement
title: "Prices follow line usage"
status: draft
derived-from:
  - "BR-WHS-PRICE-FROM-USAGE"
verification:
  - scenario: "A line used 3 of 5 project days has its factor computed on 3 days"
uses-terms:
  - "TERM-USAGE-PERIOD"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST recompute price factor and line total from each line's own usage period or duration.

## Rationale

Bill what is used.
