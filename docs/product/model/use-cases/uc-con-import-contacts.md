---
id: UC-CON-IMPORT-CONTACTS
type: use-case
title: "Import contacts from a spreadsheet"
status: active
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors:
  - "ACT-OPERATIONS-COORDINATOR"
bounded-context: "BC-CONTACTS"
governed-by:
  - "BR-CON-IMPORT-ROW-ATOMICITY"
  - "BR-CON-IMPORT-AUDIT"
uses-terms:
  - "TERM-CONTACT"
---

## Goal

Create contacts from a spreadsheet after the user has reviewed its field mapping and validation results.

## Trigger

An administrator is setting up or extending the Contacts directory from an existing contact list.

## Preconditions

The actor may create contacts and the selected file meets the accepted import file format.

## Main Flow

1. The actor uploads a spreadsheet and maps its columns to supported contact fields.
2. The product previews the first 10 data rows and reports row-level validation results.
3. The actor confirms the import after reviewing the mapping and results.
4. The product creates each valid contact as one complete record, reports rejected rows with reasons, and records an audit outcome.

## Alternative Flows

- If every row is invalid, the product creates no contacts and shows the row errors.

## Failure Conditions

- An unsupported file or invalid mapping is refused before import and no contacts are created.
- A row with invalid data is not partially saved.
- An actor without permission to create contacts is refused and nothing changes.

## Postconditions

Each accepted row has created one complete contact; rejected rows remain unsaved and are reported. The import outcome is auditable.
