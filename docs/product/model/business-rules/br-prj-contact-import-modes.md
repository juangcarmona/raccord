---
id: BR-PRJ-CONTACT-IMPORT-MODES
type: business-rule
title: "Contact import modes and match columns"
status: draft
applies-to:
  - "UC-PRJ-IMPORT-CONTACTS"
uses-terms:
  - "TERM-CONTACT"
provenance:
  source: ".private/research/help-center/articles/360013553000.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A contact import either creates a contact per row, updates existing contacts, or adds new ones while skipping existing ones; the update and skip modes identify existing records by user-chosen match columns that must hold unique values in the file and must not be the columns being updated.

## Rationale

Prevents duplicates and wrong updates.

## Examples

See SB-PRJ-CONTACT-IMPORT-SKIP-EXISTING.

## Exceptions

None.
