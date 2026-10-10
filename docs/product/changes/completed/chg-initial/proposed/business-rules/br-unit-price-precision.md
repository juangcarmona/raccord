---
id: BR-UNIT-PRICE-PRECISION
type: business-rule
title: "Unit prices keep up to four decimals"
status: draft
applies-to:
  - "UC-RECORD-BILLABLE-WORK"
uses-terms: []
---

## Rule

A unit price may have up to four decimals and is never rounded to cents before calculating the line amount; more than four decimals is refused.

## Rationale

Rental and consumable prices are often quoted with sub-cent precision.

## Examples

1,000 units at 0.1234 give a net amount of 123.40.

## Exceptions

None.
