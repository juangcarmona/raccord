# ADR-0006: Money, rounding and VAT calculation policy

- Status: accepted (subject to accountant review, see BR-VAT-PER-RATE and the open questions of `CHG-INITIAL`)
- Date: 2026-10-06

## Context

Tools that compute at full precision and round only for display produce one- or two-cent differences between line totals, subtotals and tax. Spanish invoices must show the taxable base, rate and quota per rate consistently.

## Decision

- Money is `decimal` with an ISO currency; amounts are rounded to the currency's minor units, **midpoint away from zero**. Combining different currencies is a rule violation. EUR only for now.
- Unit prices keep up to **four** decimals; more is rejected (`pricing.unit-price-precision`).
- Line net amount = round(quantity × unit price × (1 − discount ÷ 100)), rounded **once**.
- VAT is computed **per rate on the sum of the rounded line net amounts at that rate**, rounded once per rate.
- Invoice total = Σ taxable bases + Σ VAT amounts. Groups are ordered from the highest rate.
- The frontend never computes amounts; it formats server values.

## Consequences

- Printed numbers always reconcile: lines add up to bases, bases × rate give quotas, bases + quotas give the total.
- Splitting the same work into more lines can change the total by at most the line rounding, never by the VAT rounding.
- If per-line tax becomes necessary for a record type, it is a separate, tested calculation.
