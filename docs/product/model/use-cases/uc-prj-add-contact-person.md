---
id: UC-PRJ-ADD-CONTACT-PERSON
type: use-case
title: Add a contact person
status: active
primary-actor: ACT-OPERATIONS-COORDINATOR
supporting-actors: []
bounded-context: BC-CONTACTS
governed-by:
  - BR-PRJ-CONTACT-PERSON-NOT-MOVABLE
  - BR-PRJ-INVOICE-RECIPIENT
  - BR-CON-CONTACT-PERSON-DESIGNATIONS
uses-terms:
  - TERM-CONTACT-PERSON
  - TERM-DEFAULT-CONTACT-PERSON
  - TERM-ADMINISTRATIVE-CONTACT-PERSON
provenance:
  source: .private/research/help-center/articles/360014254000.md
  confidence: high
  recovered-from: documentation
---

## Goal

The right individuals at a contact are reachable.

## Trigger

A new counterpart or invoice recipient is known.

## Preconditions

The contact exists.

## Main Flow

1. The actor opens an existing contact and adds a person with their name and supported contact details.
2. Optional default and administrative flags follow BR-CON-CONTACT-PERSON-DESIGNATIONS.
3. The product saves the person under that contact. Listing, reading and subsequent edits follow UC-CON-MANAGE-CONTACT-PERSONS.

## Alternative Flows

None.

## Failure Conditions

A contact person cannot be moved to another contact.

## Postconditions

The person remains attached to the parent contact. This creation entry does not change existing project associations or issued-invoice party snapshots. Later recipient selection remains governed by BR-PRJ-INVOICE-RECIPIENT.
