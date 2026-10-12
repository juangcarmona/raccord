---
id: UC-PRJ-IMPORT-CONTACTS
type: use-case
title: "Import contacts"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors:
  - "ACT-OPERATIONS-COORDINATOR"
bounded-context: "BC-CONTACTS"
governed-by:
  - "BR-PRJ-CONTACT-IMPORT-MODES"
  - "BR-PRJ-CONTACT-PERSON-IMPORT-ROWS"
  - "BR-PRJ-IMPORT-FOLDER-PLACEMENT"
  - "BR-PRJ-IMPORT-RESULT-REPORT"
  - "BR-PRJ-IMPORT-TAG"
uses-terms:
  - "TERM-CONTACT"
  - "TERM-CONTACT-PERSON"
  - "TERM-RECORD-FOLDER"
  - "TERM-TAG"
provenance:
  source: ".private/research/help-center/articles/360013553000.md, .private/research/help-center/articles/360015867120.md, .private/research/help-center/articles/360013700660.md"
  confidence: high
  recovered-from: documentation
---

## Goal

An existing contact list is loaded in bulk, with contact persons and folders.

## Trigger

Setting up or updating the contact database.

## Preconditions

A file meets the import file format.

## Main Flow

1. The user chooses to add all rows as new, update existing items, or add new and skip existing; and whether contact persons are included.
2. The user uploads the file, states whether it has a header and confirms column matches against contact and contact person fields (including custom fields).
3. The user chooses a root folder; rows are placed under it by their folder path.
4. For update or skip modes the user chooses match columns.
5. The product previews ten rows, imports, tags the records and reports results.

## Alternative Flows

- Undo: delete records carrying the import tag.
- Bulk update: export, edit, re-import in update mode.

## Failure Conditions

Failed rows are listed with reasons and downloadable.

## Postconditions

Contacts and contact persons exist.
