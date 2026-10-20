---
id: UC-CON-MANAGE-CONTACT-DIRECTORY
type: use-case
title: "Manage the contact directory"
status: active
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors:
  - "ACT-BILLING-ADMINISTRATOR"
bounded-context: "BC-CONTACTS"
governed-by:
  - "BR-PRJ-CONTACT-NUMBER-ASSIGNMENT"
  - "BR-PRJ-CONTACT-ROLES-BY-TAG"
  - "BR-PARTY-SNAPSHOT"
uses-terms:
  - "TERM-CONTACT"
  - "TERM-CONTACT-TYPE"
  - "TERM-CUSTOMER"
  - "TERM-SUPPLIER-CONTACT"
  - "TERM-VENUE"
  - "TERM-CONTACT-NUMBER"
---

## Goal

Customers, suppliers and venues are maintained as reusable contact records.

## Trigger

A party is added, its details change, or a user needs to find or review it.

## Preconditions

The actor has permission to access the Contacts directory.

## Main Flow

1. The user opens the contact directory and browses its paginated records.
2. The user creates a company or private-person contact and assigns one or more supported roles: customer, supplier or venue.
3. The product assigns the next contact number when none is supplied and saves the contact.
4. The user opens a contact to review its details.
5. The user edits contact details or roles and saves the change.
6. The contact remains selectable in the project and billing workflows that use its roles.

## Alternative Flows

A customer may be registered with only its legal name and completed with tax and address details later.

## Failure Conditions

- A missing legal name or invalid required detail is refused with the reason and nothing is saved.
- A contact number already used by another contact is refused.
- An actor without permission to create or edit contacts is refused and nothing changes.
- Updating a customer never changes the party snapshot of an issued invoice.

## Postconditions

The contact directory reflects the saved contact and role changes, and the contact can be selected in workflows appropriate to its roles.
