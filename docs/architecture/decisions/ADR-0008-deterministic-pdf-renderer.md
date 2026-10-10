# ADR-0008: In-house deterministic PDF renderer behind a port

- Status: accepted
- Date: 2026-10-06

## Context

Invoices need a PDF. Candidate libraries either carry licenses that conflict with free self-hosting by larger companies, need font resolution on Linux containers, or embed timestamps that make output non-reproducible.

## Decision

- `IInvoiceDocumentRenderer` (Application) is implemented by `PdfInvoiceRenderer` (Infrastructure): a small PDF 1.7 writer using the standard Helvetica fonts with WinAnsi encoding (Spanish characters and €), Adobe metric tables for alignment, word wrapping and pagination.
- Output has no creation date or random ID: the same invoice always produces the same bytes. Numbers and dates are formatted without culture data.
- The PDF is rendered from the issued invoice only, stored at `invoices/{id}/{number}.pdf`, and its SHA-256 is audited. Missing documents are regenerated identically.
- Labels are Spanish for now (assumption A-5).

## Consequences

- No new dependency; tests check structure (xref offsets), encoding, determinism and pagination.
- Characters outside WinAnsi print as `?`. Configurable templates, logos, QR codes (needed for VERI*FACTU) and PDF/A-3 (needed for hybrid e-invoices) will justify revisiting this ADR; the port keeps that change local.
