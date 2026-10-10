---
id: UC-PRJ-ADD-CONTACT-PERSON
type: use-case
title: "Add a contact person"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PARTIES"
governed-by:
  - "BR-PRJ-CONTACT-PERSON-NOT-MOVABLE"
  - "BR-PRJ-INVOICE-RECIPIENT"
uses-terms:
  - "TERM-CONTACT-PERSON"
  - "TERM-DEFAULT-CONTACT-PERSON"
  - "TERM-ADMINISTRATIVE-CONTACT-PERSON"
provenance:
  source: ".private/research/help-center/articles/360014254000.md"
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

1. The user adds a contact person with own details.
2. Optionally marks it default and/or administrative.
3. The product saves.

## Alternative Flows

None.

## Failure Conditions

A contact person cannot be moved to another contact.

## Postconditions

The person is selectable with the contact.
