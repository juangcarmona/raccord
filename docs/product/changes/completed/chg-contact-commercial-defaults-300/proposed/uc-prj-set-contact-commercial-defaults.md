---
id: UC-PRJ-SET-CONTACT-COMMERCIAL-DEFAULTS
type: use-case
title: "Set commercial defaults for a contact"
status: active
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors:
  - "ACT-OPERATIONS-COORDINATOR"
bounded-context: "BC-CONTACTS"
governed-by:
  - "BR-PRJ-CLIENT-COMMERCIAL-OVERRIDE"
uses-terms:
  - "TERM-CONTACT-COMMERCIAL-DEFAULTS"
---

## Goal

Store agreed commercial values on a contact for use when creating that contact's projects.

## Trigger

Special terms are agreed with a customer.

## Preconditions

The contact exists and the actor may edit its commercial defaults.

## Main Flow

1. The user sets the contact's invoice moment, payment term, VAT scheme, optional insurance percentage, and rental and sale discount groups.
2. The product validates and saves the selected values.
3. When the contact is selected as client while creating a new project, the product copies the saved values into that project's initial commercial settings.

## Alternative Flows

- A value not set on the contact leaves the corresponding project value to its existing project and project-type defaults.

## Failure Conditions

- A percentage outside zero to one hundred, or a reference to a missing moment, term, VAT scheme or discount group, is refused and nothing is saved.
- An actor without permission to edit Contacts is refused and nothing changes.
- If another user changed the contact in the meantime, the change is refused and the user must reload and retry.

## Postconditions

The saved defaults are available to initialize future projects. Existing projects do not synchronize with later contact changes.
