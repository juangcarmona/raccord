---
id: SB-LINE-NET-ROUNDING
type: structured-behaviour
title: "Line net amount is rounded once after discount"
status: draft
illustrates:
  - "BR-MONEY-ROUNDING"
given:
  - "A billable line has quantity 3, unit price 33.333 and a 10 percent discount"
when: "Its net amount is calculated"
then:
  - "The net amount is 90.00"
  - "The unit price is kept as 33.333"
uses-terms: []
---

## Intent

Rounding happens once, at the end, never on the unit price.

## Boundaries

None.
