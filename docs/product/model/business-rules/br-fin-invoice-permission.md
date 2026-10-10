---
id: BR-FIN-INVOICE-PERMISSION
type: business-rule
title: "Only users with financial access can generate invoices"
status: draft
applies-to:
  - "UC-FIN-GENERATE-PROJECT-INVOICE"
  - "UC-FIN-CREATE-STANDALONE-INVOICE"
uses-terms:
  - "TERM-FINANCIAL-DETAILS"
provenance:
  source: ".private/research/help-center/articles/360017648580.md, .private/research/help-center/articles/4409044768658.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Generating invoices requires a role with access to financial information.

## Rationale

Billing is a controlled activity.

## Examples

A warehouse role without financial visibility cannot create an invoice.

## Exceptions

None.
