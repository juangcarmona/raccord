---
id: UC-FIN-EXPORT-DATA
type: use-case
title: "Export records to a spreadsheet"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-COMMERCIAL"
governed-by:
  - "BR-FIN-SPREADSHEET-EXPORT"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360014696379.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Selected records are available as a spreadsheet outside the product.

## Trigger

Data is needed in another tool (CRM, insurance list, mailing).

## Preconditions

Records exist in a list.

## Main Flow

1. The user selects records in a list (projects, invoices, equipment, contacts, crew, vehicles, tasks, time registrations, statistics).
2. The user chooses columns, all default columns, custom columns or extra fields.
3. The product produces a spreadsheet file.

## Alternative Flows

Optional column groups (contact persons, crew rates, activities) are added with suffixed names.

## Failure Conditions

- An export with no records selected is refused with the reason and no file is produced.
- A user without access to some of the selected data is refused for that data and the file contains no unauthorised records.
- If the file cannot be produced, the export fails visibly and can be retried (BR-FIN-SPREADSHEET-EXPORT).

## Postconditions

The user has the file.
