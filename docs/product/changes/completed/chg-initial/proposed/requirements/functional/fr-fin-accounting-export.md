---
id: FR-FIN-ACCOUNTING-EXPORT
type: functional-requirement
title: "Export accounting data as a file"
status: draft
derived-from:
  - "UC-FIN-RUN-ACCOUNTING-EXPORT"
  - "UC-FIN-EXPORT-INVOICES-SPREADSHEET"
  - "BR-FIN-EXPORT-LEDGER-ROWS"
  - "BR-FIN-EXPORT-ONLY-FINAL"
verification:
  - scenario: "Exporting a period with two finalized invoices and one payment yields one file with the invoices, their ledger rows and VAT, and the payment; draft invoices are not included."
uses-terms:
  - "TERM-ACCOUNTING-EXPORT"
  - "TERM-ACCOUNTING-CODE"
provenance:
  source: ".private/research/help-center/articles/360013492560.md, .private/research/help-center/articles/360013977700.md, .private/research/help-center/articles/360013971080.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST export finalized invoices, their payments and VAT as a CSV or spreadsheet file for the accountant, split by ledger, for a period or for selected invoices, carrying the accounting codes of customers, VAT rates and payment methods where they are set.

## Rationale

The accountant can book without retyping.
