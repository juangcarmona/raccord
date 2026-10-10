---
id: UC-PRJ-SET-CONTACT-COMMERCIAL-DEFAULTS
type: use-case
title: "Set commercial defaults for a contact"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors:
  - "ACT-OPERATIONS-COORDINATOR"
bounded-context: "BC-PARTIES"
governed-by:
  - "BR-PRJ-CLIENT-COMMERCIAL-OVERRIDE"
uses-terms:
  - "TERM-CONTACT-COMMERCIAL-DEFAULTS"
provenance:
  source: ".private/research/help-center/articles/360014254000.md, .private/research/help-center/articles/360013628759.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Agreed terms apply automatically on the customer's projects.

## Trigger

Special terms are agreed with a customer.

## Preconditions

The contact exists.

## Main Flow

1. The user sets invoice moment, payment term, VAT scheme, specific insurance percentage and rental and sale discount groups.
2. The product saves; future projects with this client use them.

## Alternative Flows

None.

## Failure Conditions

- A percentage outside zero to one hundred, or a reference to a missing moment, term, VAT scheme or discount group, is refused with the reason and nothing is saved.
- Values saved here override the project's own and its type's defaults only on future projects of this client (BR-PRJ-CLIENT-COMMERCIAL-OVERRIDE).
- An actor without permission to edit contacts is refused and nothing changes.
- If another user changed the contact in the meantime, the change is refused and the user must reload and retry.

## Postconditions

Defaults stored.
