---
id: BR-VAT-PER-RATE
type: business-rule
title: "VAT is calculated per rate on the summed base"
status: draft
applies-to:
  - "BC-INVOICING"
uses-terms:
  - "TERM-VAT-BREAKDOWN"
---

## Rule

VAT is calculated for each VAT rate on the sum of the rounded net amounts of the lines at that rate and rounded once per rate; the total is the sum of the bases plus the sum of the VAT amounts.

## Rationale

Splitting work into more lines must not change the VAT; printed bases, quotas and totals always reconcile. Public Spanish and EU invoicing sources require the rate-level amounts but do not mandate line-level VAT rounding, so this is Raccord's explicit product convention.

## Examples

See SB-VAT-BASE-ROUNDING.

## Exceptions

None.

## Decision evidence

See [ADR-0006](../../../adr/0006-money-rounding-and-vat.md) for the public-source review and the rejected line-level alternative.
