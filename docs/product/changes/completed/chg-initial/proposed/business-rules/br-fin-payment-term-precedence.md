---
id: BR-FIN-PAYMENT-TERM-PRECEDENCE
type: business-rule
title: "Invoice payment term overrides the customer default"
status: draft
applies-to:
  - "UC-FIN-GENERATE-PROJECT-INVOICE"
  - "UC-FIN-EDIT-INVOICE-DRAFT"
uses-terms:
  - "TERM-PAYMENT-TERM"
  - "TERM-CUSTOMER-FINANCIAL-DEFAULTS"
provenance:
  source: ".private/research/help-center/articles/360014214060.md, .private/research/help-center/articles/360013965120.md"
  confidence: high
  recovered-from: documentation
---

## Rule

An invoice uses the payment term of its customer unless another term or due date is chosen for that invoice, which then applies to that invoice only.

## Rationale

Exceptions are common but must not change the customer agreement.

## Examples

A customer on 30 days gets one invoice on 7 days.

## Exceptions

None.
