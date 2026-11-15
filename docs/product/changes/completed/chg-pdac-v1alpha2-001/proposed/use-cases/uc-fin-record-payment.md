---
id: UC-FIN-RECORD-PAYMENT
type: use-case
title: "Record a payment on an invoice"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-FIN-OUTSTANDING-AMOUNT"
uses-terms:
  - "TERM-PAYMENT"
  - "TERM-OUTSTANDING-AMOUNT"
  - "TERM-PAYMENT-STATUS"
provenance:
  source: ".private/research/help-center/articles/18560784878610.md, .private/research/help-center/articles/360017700379.md, .private/research/help-center/articles/360013965120.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The invoice shows what has been paid and what remains.

## Trigger

Money is received.

## Preconditions

The invoice is issued.

## Main Flow

1. The administrator opens the payments of the invoice (from the project, the invoice list or the invoice).
2. The administrator fulfils the outstanding amount, or adds a payment with date, method, amount and description.
3. The product recalculates the outstanding amount and payment status.

## Alternative Flows

Selecting several invoices shows the total paid. Partial payments can be printed on the invoice when the template includes them.

## Failure Conditions

- Recording a payment on an invoice that is not issued is refused and nothing is saved.
- A payment with a missing or invalid date, method or amount is refused with the reason and nothing is saved.
- A payment that would bring the outstanding amount below zero is refused with the reason (BR-FIN-OUTSTANDING-AMOUNT).
- If another user changed the payments in the meantime, the change is refused and the administrator must reload and retry.

## Postconditions

Payment history and status are updated.
