---
id: BR-LINE-PRICING-BOUNDS
type: business-rule
title: "Billable line pricing bounds"
status: draft
applies-to:
  - "UC-RECORD-BILLABLE-WORK"
uses-terms:
  - "TERM-BILLABLE-LINE"
---

## Rule

A billable line needs a description, a quantity greater than zero, a unit price of zero or more, a discount between 0 and 100 percent and a VAT rate between 0 and 100 percent.

## Rationale

Prevents meaningless or negative charges; corrections are made with rectifying invoices.

## Examples

A quantity of 0 is refused; 1.5 days is accepted.

## Exceptions

None.
