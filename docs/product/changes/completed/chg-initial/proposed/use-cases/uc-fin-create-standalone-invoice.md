---
id: UC-FIN-CREATE-STANDALONE-INVOICE
type: use-case
title: "Create an invoice without a project"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-FIN-EXTRA-LINE-SIGN"
  - "BR-FIN-INVOICE-PERMISSION"
uses-terms:
  - "TERM-STANDALONE-INVOICE"
  - "TERM-INVOICE-EXTRA-LINE"
provenance:
  source: ".private/research/help-center/articles/360013965120.md, .private/research/help-center/articles/360017648580.md, .private/research/help-center/articles/360017696859.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Something not tied to a project is invoiced.

## Trigger

A one-off sale or service.

## Preconditions

The customer exists.

## Main Flow

1. The administrator adds an invoice in the invoice list.
2. The administrator fills in client and details and adds invoice lines.
3. Optionally earlier invoices are subtracted.
4. The product generates the invoice draft.

## Alternative Flows

Negative lines produce a freestanding refund.

## Failure Conditions

User without financial access: refused.

## Postconditions

A standalone invoice draft exists.
