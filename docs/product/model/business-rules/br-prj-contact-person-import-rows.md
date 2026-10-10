---
id: BR-PRJ-CONTACT-PERSON-IMPORT-ROWS
type: business-rule
title: "Contact persons are imported one row each"
status: draft
applies-to:
  - "UC-PRJ-IMPORT-CONTACTS"
uses-terms:
  - "TERM-CONTACT-PERSON"
provenance:
  source: ".private/research/help-center/articles/360013553000.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When contact persons are imported, each contact person occupies its own row repeating the contact's data, and columns are matched to either contact or contact person fields.

## Rationale

Allows several persons per contact in one file.

## Examples

Two rows for 'Acme' with different persons produce one contact with two persons.

## Exceptions

None.
