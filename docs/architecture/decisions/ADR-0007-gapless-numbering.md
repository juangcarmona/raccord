# ADR-0007: Gapless numbering per series and year, allocated in the issuing transaction

- Status: accepted
- Date: 2026-10-06

## Context

Spanish invoices need correlative numbering within a series, and issue dates should follow number order. Database sequences leave gaps on rollback.

## Decision

- An `InvoiceSeries` (code: 1–10 uppercase letters/digits) has one `InvoiceNumberSequence` per calendar year, stored as a row `(series_code, year, last_number, last_issue_date)` with `xmin` concurrency.
- The number is allocated by the domain (`InvoiceNumberSequence.Allocate`) inside the same unit of work that stores the issued invoice. A rollback discards the allocation, so there are no gaps.
- An issue date earlier than the last one in the sequence, or in another year, is rejected.
- Format: `{series}{year}-{number:00000}`. Unique indexes on the number and on `(series, year, sequence)` are a second safety net.
- Two default series are created at migration: `F` (ordinary, default) and `R` (reserved for rectifying invoices).

## Consequences

- Concurrent finalizations conflict on the sequence row; the loser gets 409 and retries (tested with six parallel requests: numbers 1–6, no gaps).
- One web replica is the default in Azure; scaling out needs load testing of this contention point.
