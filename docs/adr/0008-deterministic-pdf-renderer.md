---
name: deterministic-pdf-renderer
description: Invoice PDFs are produced by a small in-house PDF writer behind a renderer port, so the same invoice always yields the same bytes.
status: Accepted
date: 2026-10-06
deciders: ["Juan G. Carmona"]
tags: ["documents", "pdf", "determinism", "dependencies"]
---

# ADR-0008: Render invoice PDFs with an in-house deterministic writer behind a port

## Context

Invoices need a PDF. Candidate libraries either carry licenses that conflict with free self-hosting by larger companies, need font resolution on Linux containers, or embed timestamps that make output non-reproducible.

## Decision

We will render invoice PDFs with a small in-house writer behind a renderer port.

- `IInvoiceDocumentRenderer` (Application) is implemented by `PdfInvoiceRenderer` (Infrastructure): a small PDF 1.7 writer using the standard Helvetica fonts with WinAnsi encoding (Spanish characters and €), Adobe metric tables for alignment, word wrapping and pagination.
- Output has no creation date or random ID: the same invoice always produces the same bytes. Numbers and dates are formatted without culture data.
- The PDF is rendered from the issued invoice only, stored at `invoices/{id}/{number}.pdf`, and its SHA-256 is audited. Missing documents are regenerated identically.
- Labels are Spanish for now (assumption A-5).

## Consequences

- **Positive:**
    - No new dependency; tests check structure (xref offsets), encoding, determinism and pagination.
- **Negative:**
    - Characters outside WinAnsi print as `?`.
- **Neutral:**
    - Configurable templates, logos, QR codes (needed for VERI\*FACTU) and PDF/A-3 (needed for hybrid e-invoices) will justify revisiting this ADR; the port keeps that change local.

## Alternatives considered

- **A third-party PDF library:** rejected because the candidates either carry licenses that conflict with free self-hosting by larger companies, need font resolution on Linux containers, or embed timestamps that make output non-reproducible.

## References

- Related: ADR-0005, ADR-0009, ADR-0010
