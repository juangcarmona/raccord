---
id: BR-FIN-E-INVOICE-IDENTIFIERS
type: business-rule
title: "E-invoices require electronic identifiers of both parties"
status: draft
applies-to:
  - "UC-FIN-CONFIGURE-E-INVOICING"
uses-terms:
  - "TERM-ELECTRONIC-IDENTIFIER"
provenance:
  source: ".private/research/help-center/articles/360014032939.md, .private/research/help-center/articles/31583505898898.md"
  confidence: high
  recovered-from: documentation
---

## Rule

An e-invoice requires the identification number and scheme of the company and of the customer; an invoice lacking them is flagged.

## Rationale

The identifiers name the parties of the invoice.

## Examples

An invoice for a customer without identification number is flagged.

## Exceptions

None.
