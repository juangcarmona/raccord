# ADR-0010: Compliance seam before any VERI*FACTU implementation

- Status: accepted
- Date: 2026-10-06

## Context

VERI*FACTU requires chained billing records, QR codes, submission with evidence, and strict immutability. The legal responsibility of an open-source, self-hosted producer is unresolved. Building submission now would freeze guesses into code.

## Decision

- `IComplianceRecorder` (Application) is called for every issued invoice inside the issuing unit of work.
- Today's adapter, `PendingComplianceRecorder`, inserts a `compliance_records` row (invoice id, number, content hash, kind `invoice-issued`, status `pending`, attempts, last error). The table cannot lose rows.
- No submission, hashing chain or QR is implemented yet. The full contract — inputs, outputs, evidence, retries, errors and immutability rules — is specified in [compliance-seam.md](../compliance-seam.md).
- Tax-authority vocabulary (XSD names) is confined to the future adapter; the domain keeps its own terms.

## Consequences

- No invoice can exist without its compliance obligation being recorded, so the future adapter can process history.
- The VERI*FACTU record must be generated before the document is released; when the adapter lands, record generation moves before PDF rendering inside finalization.
