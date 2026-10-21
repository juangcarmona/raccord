---
id: BR-FIN-DUE-DATE-FROM-TERM
type: business-rule
title: "Due date is the invoice date plus the payment term"
status: active
applies-to:
  - "UC-FIN-GENERATE-PROJECT-INVOICE"
  - "UC-FIN-CONFIGURE-PAYMENT-TERMS"
uses-terms:
  - "TERM-DUE-DATE"
  - "TERM-PAYMENT-TERM"
provenance:
  source: ".private/research/help-center/articles/360014214060.md, .private/research/help-center/articles/360013965120.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The due date of an invoice is its date plus the number of days of its payment term, unless a due date is set manually.

## Rationale

Customers and the company share one deadline.

## Examples

See SB-FIN-DUE-DATE-14-DAYS.

## Exceptions

None.
