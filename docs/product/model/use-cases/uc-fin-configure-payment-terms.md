---
id: UC-FIN-CONFIGURE-PAYMENT-TERMS
type: use-case
title: "Configure payment terms"
status: active
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-FIN-DUE-DATE-FROM-TERM"
uses-terms:
  - "TERM-PAYMENT-TERM"
  - "TERM-PAYMENT-METHOD"
provenance:
  source: ".private/research/help-center/articles/360014214060.md, .private/research/help-center/articles/360017699439.md, .private/research/help-center/articles/360013628279.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Invoices carry correct payment deadlines.

## Trigger

Setting up invoicing.

## Preconditions

None.

## Main Flow

1. The administrator creates payment terms with name, printed text, accounting code, number of days and payment method.
2. The administrator assigns default terms to customers.

## Alternative Flows

None.

## Failure Conditions

- A term with missing required details, or a number of days that is negative or not a number, is refused with the reason and nothing is saved (BR-FIN-DUE-DATE-FROM-TERM).
- An actor without permission to configure invoicing is refused and nothing changes.
- If another user changed the same term in the meantime, the change is refused and the user must reload and retry.

## Postconditions

Invoices receive due dates from their terms.
