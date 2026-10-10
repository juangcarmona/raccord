---
id: FR-INVOICE-008
type: functional-requirement
title: "Provide reproducible invoice documents"
status: draft
derived-from:
  - "UC-OBTAIN-INVOICE-DOCUMENT"
  - "CON-SPANISH-INVOICE-CONTENT"
verification:
  - scenario-ref: "SB-LOST-DOCUMENT-REGENERATED"
  - scenario: "The document shows Spanish labels, both parties with tax identifiers, the number, dates, lines, VAT breakdown and total"
uses-terms:
  - "TERM-INVOICE-DOCUMENT"
---

## Requirement

The product MUST produce a PDF document for every issued invoice at issue time, store it, and reproduce it identically from the issued invoice whenever the stored copy is missing.

## Rationale

Customers and accountants need a printable invoice that always matches the record.
