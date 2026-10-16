---
name: money-rounding-and-vat
description: Money is rounded once per line to the currency's minor units, and VAT is computed per rate on the sum of the rounded line net amounts; public Spanish and EU sources do not mandate a competing rounding algorithm.
status: Accepted
date: 2026-10-16
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

The public-source review for issue #60 found no Spanish or EU rule that mandates rounding VAT per line instead of once per VAT rate. The selected policy is therefore an explicit product and accounting convention, not a claim that the law requires one of those algorithms.

## Consequences

- **Positive:**
  - Printed numbers always reconcile: lines add up to bases, bases × rate give quotas, bases + quotas give the total.
- **Negative:**
  - Splitting the same work into more lines can change the total by at most the line rounding, never by the VAT rounding.
- **Neutral:**
  - If per-line tax becomes necessary for a record type, it is a separate, tested calculation.
  - The policy is accepted after review of the Spanish invoicing requirements, the EU VAT invoice requirements and public Spanish tax guidance; an accountant is not a prerequisite for this product decision.

## Alternatives considered

- **Compute at full precision and round only for display:** rejected because it produces one- or two-cent differences between line totals, subtotals and tax.
- **Compute VAT per line as the default:** rejected because quotas per rate would no longer equal base × rate, so the printed numbers would not reconcile; it remains possible as a separate calculation for a record type that needs it.

## Public-source review

- [Spanish invoicing regulation, RD 1619/2012, Article 6](https://www.boe.es/eli/es/rd/2012/11/30/1619/): requires the invoice to identify the taxable base, rate and quota, but does not prescribe line-level versus rate-level VAT rounding.
- [EU VAT Directive 2006/112/EC, Article 226](https://eur-lex.europa.eu/legal-content/EN/TXT/HTML/?uri=CELEX%3A02006L0112-20240101): requires the taxable amount per rate and VAT payable, but does not prescribe the rounding algorithm.
- [AEAT invoice content guidance](https://www3.agenciatributaria.gob.es/Sede/iva/facturacion-registro/facturacion-iva/contenido-facturas.html): confirms the Spanish invoice fields represented by this decision.
- [AEAT public rounding guidance](https://sede.agenciatributaria.gob.es/static_files/Sede/Procedimiento_ayuda/GC44/castellano/FAQs_ITF_03_05_23.pdf): states that the cited tax law does not establish a specific rounding method and describes nearest-cent rounding with half-cent values rounded upward.

The last source concerns a different tax, so it supports the general absence of a mandated method rather than proving that this exact VAT algorithm is legally compulsory.

## References

- Business rule: BR-VAT-PER-RATE
- Open questions: Product Change `CHG-INITIAL`
