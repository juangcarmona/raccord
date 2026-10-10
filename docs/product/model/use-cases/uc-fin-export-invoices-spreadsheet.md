---
id: UC-FIN-EXPORT-INVOICES-SPREADSHEET
type: use-case
title: "Export invoices to a spreadsheet"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-FIN-EXPORT-LEDGER-ROWS"
uses-terms:
  - "TERM-LEDGER-ACCOUNT"
provenance:
  source: ".private/research/help-center/articles/360013971080.md, .private/research/help-center/articles/360017648700.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Selected invoices are available as a file for bookkeeping.

## Trigger

Periodic bookkeeping.

## Preconditions

Invoices exist.

## Main Flow

1. The administrator selects invoices and exports them.
2. The product produces a spreadsheet split by ledger, with payment term, invoice moment, number, VAT scheme, contact and project information.

## Alternative Flows

None.

## Failure Conditions

- Exporting with no invoices selected is refused and no file is produced.
- An actor without permission to export invoices is refused and nothing is produced.
- If the file cannot be produced, the export fails visibly and can be retried (BR-FIN-EXPORT-LEDGER-ROWS).
- An invoice with an amount that cannot be attributed to a ledger is reported rather than silently dropped.

## Postconditions

The administrator has the file.
