---
id: UC-FIN-CREATE-CREDIT-NOTE
type: use-case
title: "Create a credit note"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-FIN-CREDIT-NOTE-CORRECTION"
  - "BR-FIN-INVOICE-DEDUCTION"
  - "BR-FIN-CREDIT-NOTE-SERIES"
  - "BR-FIN-EXTRA-LINE-SIGN"
  - "BR-ISSUED-INVOICE-IMMUTABLE"
uses-terms:
  - "TERM-CREDIT-NOTE"
  - "TERM-INVOICE-DEDUCTION"
provenance:
  source: ".private/research/help-center/articles/360017696859.md, .private/research/help-center/articles/6819935599378.md, .private/research/help-center/articles/18560784878610.md, .private/research/help-center/articles/360013264720.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A finalized invoice is cancelled fully or partly, or a refund is documented.

## Trigger

An invoice was wrong, the customer is refunded, or a deposit is returned.

## Preconditions

For crediting: the invoice is finalized.

## Main Flow

1. The administrator creates a credit note from the invoice.
2. The product prepares a new document that subtracts the selected invoice, in the credit-note series.
3. The administrator optionally limits it to part of the original amount.
4. The administrator generates and finalizes it like an invoice.

## Alternative Flows

Freestanding refund: a new invoice with negative lines and no source. Partial refund: payments of any sign are recorded on the credit note.

## Failure Conditions

- Creating a credit note from an invoice that is not finalized is refused; nothing is created.
- The original invoice is never changed, re-issued or deleted by the credit note (BR-ISSUED-INVOICE-IMMUTABLE, BR-FIN-CREDIT-NOTE-CORRECTION).
- Missing or invalid required input, such as an amount that is not a number, is refused with the reason and nothing is saved.
- If finalizing is refused, no number is consumed (BR-FIN-CREDIT-NOTE-SERIES).
- An actor without permission to issue invoices is refused and nothing changes.

## Postconditions

The original invoice is unchanged; the credit note reduces what is owed.
