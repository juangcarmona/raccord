---
id: BR-MONEY-ROUNDING
type: business-rule
title: "Amounts are rounded once to cents, half away from zero"
status: draft
applies-to:
  - "BC-INVOICING"
  - "BC-PROJECTS"
uses-terms:
  - "TERM-BILLABLE-LINE"
---

## Rule

Every monetary amount is expressed in the invoice currency and rounded to cents with midpoint away from zero; a line's net amount is quantity × unit price × (1 − discount) rounded once.

## Rationale

Rounding at one defined point keeps printed numbers reconcilable.

## Examples

See SB-LINE-NET-ROUNDING.

## Exceptions

None.
