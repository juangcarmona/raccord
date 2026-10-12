---
name: compliance-seam
description: Every issued invoice records a pending compliance obligation through a compliance-recorder port, before any VERI*FACTU submission is implemented.
status: Accepted
date: 2026-10-06
deciders: ["Juan G. Carmona"]
tags: ["compliance", "verifactu", "invoicing", "ports"]
---

# ADR-0010: Record compliance obligations through a seam before implementing VERI\*FACTU

## Context

VERI\*FACTU requires chained billing records, QR codes, submission with evidence, and strict immutability. The legal responsibility of an open-source, self-hosted producer is unresolved. Building submission now would freeze guesses into code.

## Decision

We will record a compliance obligation for every issued invoice through a seam, and defer VERI\*FACTU submission.

- `IComplianceRecorder` (Application) is called for every issued invoice inside the issuing unit of work.
- Today's adapter, `PendingComplianceRecorder`, inserts a `compliance_records` row (invoice id, number, content hash, kind `invoice-issued`, status `pending`, attempts, last error). The table cannot lose rows.
- No submission, hashing chain or QR is implemented yet. The full contract — inputs, outputs, evidence, retries, errors and immutability rules — is specified in [compliance-seam.md](../architecture/compliance-seam.md).
- Tax-authority vocabulary (XSD names) is confined to the future adapter; the domain keeps its own terms.

## Consequences

- **Positive:**
    - No invoice can exist without its compliance obligation being recorded, so the future adapter can process history.
- **Negative:**
    - No submission, hashing chain or QR exists until the adapter is implemented.
- **Neutral:**
    - The VERI\*FACTU record must be generated before the document is released; when the adapter lands, record generation moves before PDF rendering inside finalization.

## Alternatives considered

- **Build submission now:** rejected because the legal responsibility of an open-source, self-hosted producer is unresolved, and it would freeze guesses into code.
- **Use tax-authority vocabulary in the domain:** rejected; it is confined to the future adapter so the domain keeps its own terms.

## References

- Contract: [compliance-seam.md](../architecture/compliance-seam.md)
- Related: ADR-0004, ADR-0005, ADR-0008
