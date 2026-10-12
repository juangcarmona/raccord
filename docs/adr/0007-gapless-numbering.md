---
name: gapless-numbering
description: Invoice numbers are allocated gaplessly per series and calendar year by the domain, inside the same transaction that stores the issued invoice.
status: Accepted
date: 2026-10-06
deciders: ["Juan G. Carmona"]
tags: ["invoicing", "numbering", "concurrency", "transactions"]
---

# ADR-0007: Allocate gapless numbers per series and year in the issuing transaction

## Context

Spanish invoices need correlative numbering within a series, and issue dates should follow number order. Database sequences leave gaps on rollback.

## Decision

We will allocate invoice numbers per series and year in the domain, inside the transaction that issues the invoice.

- An `InvoiceSeries` (code: 1–10 uppercase letters/digits) has one `InvoiceNumberSequence` per calendar year, stored as a row `(series_code, year, last_number, last_issue_date)` with `xmin` concurrency.
- The number is allocated by the domain (`InvoiceNumberSequence.Allocate`) inside the same unit of work that stores the issued invoice. A rollback discards the allocation, so there are no gaps.
- An issue date earlier than the last one in the sequence, or in another year, is rejected.
- Format: `{series}{year}-{number:00000}`. Unique indexes on the number and on `(series, year, sequence)` are a second safety net.
- Two default series are created at migration: `F` (ordinary, default) and `R` (reserved for rectifying invoices).

## Consequences

- **Positive:**
    - Numbering has no gaps, because a rollback discards the allocation.
- **Negative:**
    - Concurrent finalizations conflict on the sequence row; the loser gets 409 and retries (tested with six parallel requests: numbers 1–6, no gaps).
- **Neutral:**
    - One web replica is the default in Azure; scaling out needs load testing of this contention point.

## Alternatives considered

- **Database sequences:** rejected because they leave gaps on rollback.

## References

- Related: ADR-0004, ADR-0013
