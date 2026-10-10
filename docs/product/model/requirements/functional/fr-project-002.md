---
id: FR-PROJECT-002
type: functional-requirement
title: "Record priced billable work"
status: draft
derived-from:
  - "UC-RECORD-BILLABLE-WORK"
  - "BR-LINE-PRICING-BOUNDS"
  - "BR-UNIT-PRICE-PRECISION"
  - "BR-LINE-ORDER"
verification:
  - scenario-ref: "SB-LINE-NET-ROUNDING"
  - scenario: "A unit price with five decimals is refused"
uses-terms:
  - "TERM-BILLABLE-LINE"
  - "TERM-BILLING-STATE"
---

## Requirement

The product MUST record billable lines of the defined kinds with their pricing, calculate each net amount, keep the recorded order and show each line's billing state.

## Rationale

Billable work recorded once is never re-typed at billing time.
