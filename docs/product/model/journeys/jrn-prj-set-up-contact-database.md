---
id: JRN-PRJ-SET-UP-CONTACT-DATABASE
type: journey
title: "Set up the contact database"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
steps:
  - use-case: "UC-PRJ-MANAGE-RECORD-FOLDERS"
  - use-case: "UC-PRJ-IMPORT-CONTACTS"
  - use-case: "UC-PRJ-TAG-RECORDS"
  - use-case: "UC-PRJ-SET-CONTACT-COMMERCIAL-DEFAULTS"
provenance:
  source: ".private/research/help-center/articles/360013553000.md, .private/research/help-center/articles/360013700660.md, .private/research/help-center/articles/360014254000.md, .private/research/help-center/articles/360013931999.md, .private/research/help-center/articles/360013628759.md"
  confidence: high
  recovered-from: documentation
---

## Intended Outcome

Customers, venues and suppliers are available, organised and carrying their commercial terms.

## Entry Conditions

The company starts using the product with an existing contact list.

## Journey Narrative

The administrator designs the contact folder structure, imports contacts and contact persons with folder paths, tags contacts by role and priority, and records special commercial terms per customer.

## Variants and Branches

- Import partly wrong: delete by import tag and re-import.
- Single new contacts: UC-PRJ-ADD-CONTACT.

## Completion Conditions

Contacts are selectable on projects with their defaults.
