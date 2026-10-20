---
id: BR-PRJ-CONTACT-PERSON-NOT-MOVABLE
type: business-rule
title: "Contact persons cannot move between contacts"
status: active
applies-to:
  - "UC-CON-MANAGE-CONTACT-PERSONS"
uses-terms:
  - "TERM-CONTACT-PERSON"
provenance:
  source: ".private/research/help-center/articles/360014254000.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A contact person stays with the contact it was created under; someone linked to two contacts is added to each separately. Editing a contact person's details cannot change its parent contact.

## Rationale

Each record represents a person's relationship with one contact and keeps that relationship unambiguous.

## Examples

A freelance producer is entered under both agencies as two separate contact-person records.

## Exceptions

None.
