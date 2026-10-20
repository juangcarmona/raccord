---
id: UC-CON-MANAGE-CONTACT-PERSONS
type: use-case
title: "Manage a contact's people and designations"
status: active
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-CONTACTS"
governed-by:
  - "BR-CON-CONTACT-PERSON-DESIGNATIONS"
  - "BR-PRJ-CONTACT-PERSON-NOT-MOVABLE"
uses-terms:
  - "TERM-CONTACT"
  - "TERM-CONTACT-PERSON"
  - "TERM-DEFAULT-CONTACT-PERSON"
  - "TERM-ADMINISTRATIVE-CONTACT-PERSON"
---

## Goal

People associated with a contact and their supported designations are maintained accurately.

## Trigger

A contact gains a counterpart, or the person serving as its default or administrative contact changes.

## Preconditions

The actor may maintain Contacts and the parent contact exists.

## Main Flow

1. The actor opens a contact and reviews its contact persons.
2. The actor adds or edits a person's name and supported contact details.
3. The actor may designate the person as the contact's default person and/or administrative person.
4. The product saves the person under that contact and shows the saved designations.

## Alternative Flows

- When a person becomes the default, the product clears the previous default for that contact.
- A person associated with a second contact is recorded separately under that contact.

## Failure Conditions

- A missing required name or invalid contact detail is refused and nothing is saved.
- A person cannot be moved from its original contact to another contact.
- An actor without permission to edit Contacts is refused and nothing changes.

## Postconditions

The contact's people and their saved designations are available for later contact and project workflows. This use case does not select a person on a project or dispatch an invoice or message.
