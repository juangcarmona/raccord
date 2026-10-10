---
id: BR-FIN-EXPORT-LEDGER-ROWS
type: business-rule
title: "Invoices are exported as ledger rows"
status: draft
applies-to:
  - "UC-FIN-EXPORT-INVOICES-SPREADSHEET"
  - "UC-FIN-RUN-ACCOUNTING-EXPORT"
uses-terms:
  - "TERM-LEDGER-ACCOUNT"
  - "TERM-ACCOUNTING-EXPORT"
provenance:
  source: ".private/research/help-center/articles/360013971080.md, .private/research/help-center/articles/360013977700.md, .private/research/help-center/articles/360013492560.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Exported invoices are split into one row per ledger with the total of that ledger; individual equipment items and functions are not included in the accounting file.

## Rationale

Accounting needs revenue by account, not by item.

## Examples

An invoice with 30 items over two ledgers exports two rows.

## Exceptions

None.
