---
id: BR-PRJ-PARTIES-FROM-CONTACTS
type: business-rule
title: "Clients and locations come from the contact database"
status: active
applies-to:
  - "UC-PRJ-EDIT-PROJECT-DETAILS"
uses-terms:
  - "TERM-CONTACT"
  - "TERM-CUSTOMER"
  - "TERM-VENUE"
provenance:
  source: ".private/research/help-center/articles/360013668139.md, .private/research/help-center/articles/29017857097618.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A project's client, contact person and location must be contacts in the contact database; a missing one can be created without leaving the project.

## Rationale

One source of truth for parties.

## Examples

Typing an unknown client offers creating it.

## Exceptions

None.
