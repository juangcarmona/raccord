---
id: JRN-PRJ-SET-UP-CONTACT-DATABASE
type: journey
title: Set up the contact database
status: draft
primary-actor: ACT-WORKSPACE-ADMINISTRATOR
steps:
  - use-case: UC-CON-MANAGE-CONTACT-DIRECTORY
  - use-case: UC-CON-IMPORT-CONTACTS
  - use-case: UC-CON-MANAGE-CONTACT-PERSONS
  - use-case: UC-CON-TAG-CONTACTS
  - use-case: UC-CON-MANAGE-CONTACT-FOLDERS
provenance:
  source: .private/research/help-center/articles/360013553000.md, .private/research/help-center/articles/360013700660.md, .private/research/help-center/articles/360014254000.md, .private/research/help-center/articles/360013931999.md, .private/research/help-center/articles/360013628759.md
  confidence: high
  recovered-from: documentation
---

## Intended Outcome

Customers, venues and suppliers are available in the Contacts directory, with optional people and organization.

## Entry Conditions

The company starts using the product with an existing contact list.

## Journey Narrative

The administrator maintains contacts through UC-CON-MANAGE-CONTACT-DIRECTORY or creates them from a supported file through UC-CON-IMPORT-CONTACTS. Contact people are maintained separately through UC-CON-MANAGE-CONTACT-PERSONS. Tags and optional folders follow UC-CON-TAG-CONTACTS and UC-CON-MANAGE-CONTACT-FOLDERS; contacts may remain unfiled.

## Variants and Branches

- The create-only import reports rejected rows in the product; it does not import people, tags or folders and supplies no undo.
- The distinct extended profile UC-PRJ-IMPORT-CONTACTS describes updates, imported people, folder paths, import tags and undo; those capabilities do not expand the create-only profile.
- Single new contacts: UC-PRJ-ADD-CONTACT.
- Commercial terms are maintained separately through UC-PRJ-SET-CONTACT-COMMERCIAL-DEFAULTS.

## Completion Conditions

Saved contacts are available for selection. This journey does not require commercial defaults, folder placement or the extended import profile to complete directory setup.
