---
id: UC-PRJ-BULK-EDIT-CONTACTS
type: use-case
title: "Edit several contacts at once"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PARTIES"
governed-by: []
uses-terms:
  - "TERM-CONTACT"
provenance:
  source: ".private/research/help-center/articles/360014254000.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Many contacts get the same change quickly.

## Trigger

A common value changes.

## Preconditions

The user may edit contacts.

## Main Flow

1. The user selects contacts and the fields to change.
2. The product applies the values to all selected contacts.

## Alternative Flows

- Export, edit and re-import.

## Failure Conditions

- Applying a value that is invalid for a field is refused with the reason and none of the selected contacts is changed.
- An actor without permission to edit contacts is refused and nothing changes.
- If another user changed a selected contact in the meantime, that change is refused and the user must reload and retry.
- An imported file with unrecognised contacts or columns is rejected with the reason and nothing is changed.

## Postconditions

Contacts updated.
