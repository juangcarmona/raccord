---
name: money-rounding-and-vat
description: Money is rounded once per line to the currency's minor units, and VAT is computed per rate on the sum of the rounded line net amounts.
status: Accepted
date: 2026-10-06
deciders: ["Juan G. Carmona"]
tags: ["money", "rounding", "vat", "invoicing"]
---

# ADR-0006: Round money once per line and compute VAT per rate on rounded line amounts

## Context

Tools that compute at full precision and round only for display produce one- or two-cent differences between line totals, subtotals and tax. Spanish invoices must show the taxable base, rate and quota per rate consistently.

## Decision

We will round each line net amount once and compute VAT per rate on the rounded line amounts.

- Money is `decimal` with an ISO currency; amounts are rounded to the currency's minor units, **midpoint away from zero**. Combining different currencies is a rule violation. EUR only for now.
- Unit prices keep up to **four** decimals; more is rejected (`pricing.unit-price-precision`).
- Line net amount = round(quantity × unit price × (1 − discount ÷ 100)), rounded **once**.
- VAT is computed **per rate on the sum of the rounded line net amounts at that rate**, rounded once per rate.
- Invoice total = Σ taxable bases + Σ VAT amounts. Groups are ordered from the highest rate.
- The frontend never computes amounts; it formats server values.

## Consequences

- **Positive:**
    - Printed numbers always reconcile: lines add up to bases, bases × rate give quotas, bases + quotas give the total.
- **Negative:**
    - Splitting the same work into more lines can change the total by at most the line rounding, never by the VAT rounding.
- **Neutral:**
    - If per-line tax becomes necessary for a record type, it is a separate, tested calculation.
    - The policy is accepted subject to accountant review (see BR-VAT-PER-RATE and the open questions of `CHG-INITIAL`).

## Alternatives considered

- **Compute at full precision and round only for display:** rejected because it produces one- or two-cent differences between line totals, subtotals and tax.
- **Compute VAT per line as the default:** rejected because quotas per rate would no longer equal base × rate, so the printed numbers would not reconcile; it remains possible as a separate calculation for a record type that needs it.

## References

- Business rule: BR-VAT-PER-RATE
- Open questions: Product Change `CHG-INITIAL`
