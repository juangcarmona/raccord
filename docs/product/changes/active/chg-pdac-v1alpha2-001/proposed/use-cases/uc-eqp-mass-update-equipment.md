---
id: UC-EQP-MASS-UPDATE-EQUIPMENT
type: use-case
title: "Update many items through export and re-import"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-EQP-IMPORT-MATCH-UNIQUE"
uses-terms:
  - "TERM-EQUIPMENT-IMPORT"
  - "TERM-IMPORT-MATCH-COLUMN"
  - "TERM-IMPORT-MODE"
provenance:
  source: ".private/research/help-center/articles/360014293220.md, .private/research/help-center/articles/360014141299.md, .private/research/help-center/articles/24338169105298.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Many items change at once (prices, measurements, missing fields) without manual editing.

## Trigger

A broad data change is needed, such as a price increase.

## Preconditions

- Items exist.

## Main Flow

1. The equipment manager exports the items including the record identifiers of the items and the columns to change.
2. The manager edits values in a spreadsheet and saves it.
3. The manager imports it in update mode, maps columns and chooses the record identifier as match column.
4. The product updates the differing values of each matched item and reports the result.

## Alternative Flows

- 3a. Another unique column such as code is used as match column, provided it is not a column being changed.

## Failure Conditions

- Duplicate values in a match column: rows are mismatched or duplicates are created; the manager groups by the column to find and fix doubles.

## Postconditions

- Items carry the new values.
