---
id: UC-CORRECT-ISSUED-INVOICE
type: use-case
title: "Correct an issued invoice"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-ISSUED-INVOICE-IMMUTABLE"
  - "BR-GAPLESS-NUMBERING"
  - "BR-RECTIFYING-INVOICE"
uses-terms:
  - "TERM-RECTIFYING-INVOICE"
---

## Goal

An error in an issued invoice is corrected without altering it.

## Trigger

A customer disputes or an error is found in an issued invoice.

## Preconditions

The invoice is issued.

## Main Flow

1. The billing administrator starts a rectifying invoice from an issued invoice, states the reason and chooses correction by differences or by substitution.
2. The product prepares the draft in the rectifying series, referencing the original, with the original's lines (substitution) or empty differences to complete.
3. The administrator completes and issues it; negative totals are allowed.

## Alternative Flows

- Several issued invoices can be corrected together by one rectifying invoice that references all of them.

## Failure Conditions

- A rectifying invoice cannot be started from an invoice that is not issued (for example a draft); nothing is created.
- An attempt to alter, re-issue or delete the original invoice through the correction is refused; only a new document referencing it is allowed (BR-ISSUED-INVOICE-IMMUTABLE).
- If issuing the rectifying invoice is refused or fails, no number is consumed and none is ever reused, so the rectifying series stays gapless (BR-GAPLESS-NUMBERING).
- An actor without permission to issue invoices is refused and nothing changes.

## Postconditions

The original is unchanged; both are linked.
