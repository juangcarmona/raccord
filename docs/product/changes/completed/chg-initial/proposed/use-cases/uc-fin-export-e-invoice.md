---
id: UC-FIN-EXPORT-E-INVOICE
type: use-case
title: "Export an invoice as e-invoice"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-FIN-E-INVOICE-IDENTIFIERS"
uses-terms:
  - "TERM-E-INVOICE"
  - "TERM-E-INVOICE-FORMAT"
provenance:
  source: ".private/research/help-center/articles/360014032939.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A structured file of an invoice is available for the customer or bookkeeping.

## Trigger

A customer requires an e-invoice.

## Preconditions

The invoice is finalized and e-invoicing configured.

## Main Flow

1. The administrator downloads the PDF files or exports for accounting and e-invoices.
2. The administrator chooses a format.
3. The product produces PDF and XML with aligned file names.

## Alternative Flows

None.

## Failure Conditions

Validation shows missing identifiers.

## Postconditions

The administrator has the e-invoice.
