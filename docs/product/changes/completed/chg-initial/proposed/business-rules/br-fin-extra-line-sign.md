---
id: BR-FIN-EXTRA-LINE-SIGN
type: business-rule
title: "Extra invoice lines may be positive or negative"
status: draft
applies-to:
  - "UC-FIN-GENERATE-PROJECT-INVOICE"
  - "UC-FIN-CREATE-STANDALONE-INVOICE"
  - "UC-FIN-CREATE-CREDIT-NOTE"
uses-terms:
  - "TERM-INVOICE-EXTRA-LINE"
provenance:
  source: ".private/research/help-center/articles/360013965120.md, .private/research/help-center/articles/360017648580.md, .private/research/help-center/articles/360017696859.md"
  confidence: high
  recovered-from: documentation
---

## Rule

An extra invoice line adds its amount to the invoice total when positive and subtracts it when negative.

## Rationale

Covers surcharges, goodwill and freestanding refunds.

## Examples

A line of -50.00 reduces a 1,000.00 invoice to 950.00.

## Exceptions

None.
