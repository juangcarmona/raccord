---
id: UC-FIN-RUN-ACCOUNTING-EXPORT
type: use-case
title: "Export invoices, payments and VAT for the accountant"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-FIN-EXPORT-LEDGER-ROWS"
  - "BR-FIN-EXPORT-ONLY-FINAL"
  - "BR-FIN-DRAFT-PREVIEW-MARK"
uses-terms:
  - "TERM-ACCOUNTING-EXPORT"
provenance:
  source: ".private/research/help-center/articles/360013492560.md, .private/research/help-center/articles/360013977700.md, .private/research/help-center/articles/6819935599378.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The accountant receives the finalized invoices, their payments and the VAT of a period as a file.

## Trigger

Periodic bookkeeping, or the accountant asks for the figures.

## Preconditions

Finalized invoices exist.

## Main Flow

1. The administrator chooses a period and starts the export.
2. The product collects the finalized invoices of the period with number, date, customer, ledger rows, VAT by rate, payment term and the payments received.
3. The product produces a CSV or spreadsheet file.

## Alternative Flows

The administrator exports selected invoices only (UC-FIN-EXPORT-INVOICES-SPREADSHEET).

## Failure Conditions

- A period without finalized invoices is refused and no file is produced.
- Drafts and previews are never included (BR-FIN-EXPORT-ONLY-FINAL).
- An actor without permission to export invoices is refused and nothing is produced.

## Postconditions

The administrator has the file.
