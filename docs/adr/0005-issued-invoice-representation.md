---
name: issued-invoice-representation
description: An issued invoice is stored as deterministic canonical JSON with a SHA-256 fingerprint that is verified on every read.
status: Accepted
date: 2026-10-06
deciders: ["Juan G. Carmona"]
tags: ["invoicing", "integrity", "persistence", "immutability"]
---

# ADR-0005: Store issued invoices as a canonical representation with a SHA-256 fingerprint

## Context

An issued invoice must be reproducible exactly: for the PDF, for compliance records, for audits and after restores. Mapping a rich aggregate into many mutable columns makes silent drift possible.

## Decision

We will store every issued invoice as a canonical representation and verify its SHA-256 fingerprint on every read.

- At finalization the domain produces `IssuedInvoiceRepresentation`: deterministic JSON (schema `raccord.issued-invoice/1`, camelCase, fixed property order, numbers written as fixed-point strings, dates as ISO strings, UTC timestamp with 7 fractional digits) and the lowercase hex SHA-256 of its UTF-8 bytes.
- The JSON is the source of truth, stored as `text` (not `jsonb`, which would normalize it). Indexed columns (number, date, recipient, total) exist only for queries.
- Every read verifies the hash before restoring the invoice. A mismatch is an integrity failure: the invoice is not served, the API returns 500 with code `issued-invoice.integrity-violation`, and a critical log entry is written.
- The fingerprint is shown in the UI, written to the audit event and printed in the PDF footer reference.

## Consequences

- **Positive:**
    - An issued invoice is reproducible exactly, and a changed representation is detected before the invoice is served.
- **Negative:**
    - The schema identifier must be bumped if the representation ever changes; old representations remain readable by version.
- **Neutral:**
    - This fingerprint is Raccord's integrity mechanism. It is **not** the VERI\*FACTU `Huella`, which has its own field list and chaining (see the compliance seam).

## Alternatives considered

- **Map the aggregate into many mutable columns:** rejected because it makes silent drift possible.
- **Store the JSON as `jsonb`:** rejected because `jsonb` would normalize it.

## References

- Related: ADR-0004, ADR-0010
