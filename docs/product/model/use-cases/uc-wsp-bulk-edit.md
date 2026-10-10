---
id: UC-WSP-BULK-EDIT
type: use-case
title: "Edit many records at once"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
supporting-actors:
  - "ACT-OPERATIONS-COORDINATOR"
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-WSP-BULK-EDIT-KIND-FIELDS"
  - "BR-WSP-MASTER-CHANGES-NOT-RETROACTIVE"
uses-terms:
  - "TERM-BULK-EDIT"
provenance:
  source: ".private/research/help-center/articles/360013838520.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Several records receive the same change in one step.

## Trigger

A common change applies to many records (discount, price, contract hours).

## Preconditions

- The user may edit the records concerned.

## Main Flow

1. The user selects records in a module, optionally filtering by kind first.
2. The user asks to edit the selection and chooses the fields to change.
3. The user enters the new values and saves.
4. The product applies the values to every selected record.

## Alternative Flows

- Different values per record: export, edit and update by import instead.

## Failure Conditions

- A field does not apply to all selected kinds: it is not offered.

## Postconditions

- All selected records hold the new values; already planned lines are unchanged.
