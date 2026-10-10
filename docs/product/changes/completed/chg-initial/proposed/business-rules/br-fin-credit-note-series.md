---
id: BR-FIN-CREDIT-NOTE-SERIES
type: business-rule
title: "Credit notes can use their own number series"
status: draft
applies-to:
  - "UC-FIN-CREATE-CREDIT-NOTE"
  - "UC-FIN-CONFIGURE-FINANCIAL-SETTINGS"
uses-terms:
  - "TERM-CREDIT-NOTE"
  - "TERM-INVOICE-SERIES"
provenance:
  source: ".private/research/help-center/articles/6819935599378.md"
  confidence: medium
  recovered-from: documentation
---

## Rule

A separate invoice/credit-note number series can be created and set as the default for credit notes per project type; otherwise credit notes are numbered in the invoice series.

## Rationale

Many jurisdictions and accountants expect credit notes to be identifiable by number.

## Examples

Credit notes numbered C-0001, C-0002 while invoices continue in their own series.

## Exceptions

None.
