---
id: UC-FIN-SET-CUSTOMER-FINANCIAL-DEFAULTS
type: use-case
title: "Set the financial defaults of a customer"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-COMMERCIAL"
governed-by:
  - "BR-FIN-DEFAULTS-NOT-RETROACTIVE"
uses-terms:
  - "TERM-CUSTOMER-FINANCIAL-DEFAULTS"
  - "TERM-CUSTOMER"
provenance:
  source: ".private/research/help-center/articles/360013886600.md, .private/research/help-center/articles/360013890780.md, .private/research/help-center/articles/360014214060.md, .private/research/help-center/articles/360013628279.md, .private/research/help-center/articles/360013887460.md, .private/research/help-center/articles/21200846347154.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A customer's agreed conditions are stored once and proposed on every project and invoice.

## Trigger

Conditions are agreed with a customer.

## Preconditions

The customer exists.

## Main Flow

1. The administrator enters discount percentages for crew, transport, total, rental and sale equipment, and per discount group.
2. The administrator chooses the default payment term, invoice moment and VAT scheme or tax rate.
3. The product saves the defaults.

## Alternative Flows

None.

## Failure Conditions

- A discount outside zero to one hundred percent, or a reference to a missing term, moment or tax scheme, is refused with the reason and nothing is saved.
- Changed defaults do not alter projects where the customer is already client until the client is selected again or the equipment data is updated (BR-FIN-DEFAULTS-NOT-RETROACTIVE).
- An actor without permission to edit customer finances is refused and nothing changes.
- If another user changed the same customer in the meantime, the change is refused and the user must reload and retry.

## Postconditions

New projects with this client are proposed these defaults; running projects are unchanged.
