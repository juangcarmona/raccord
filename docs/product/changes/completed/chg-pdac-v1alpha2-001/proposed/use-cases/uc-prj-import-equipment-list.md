---
id: UC-PRJ-IMPORT-EQUIPMENT-LIST
type: use-case
title: "Import an equipment list into a project"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-EQUIPMENT-IMPORT-MODES"
  - "BR-PRJ-EQUIPMENT-IMPORT-MATCHING"
  - "BR-PRJ-EQUIPMENT-IMPORT-PLACEMENT"
  - "BR-PRJ-IMPORT-RESULT-REPORT"
  - "BR-PRJ-SAVED-MATCH-REUSE"
uses-terms:
  - "TERM-SAVED-IMPORT-MATCH"
provenance:
  source: ".private/research/help-center/articles/4413562661138.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Equipment designed elsewhere (for example in a drawing tool) is planned on the project without retyping.

## Trigger

A drawing or design tool produced an equipment list for the job.

## Preconditions

The project exists; a spreadsheet or comma-separated file with item identifiers and quantities is available.

## Main Flow

1. The coordinator chooses to add all rows as new lines, or to delete the planned equipment and import all rows.
2. The coordinator uploads the file and maps its columns to item name, code, a design-tool code or a custom field, and the quantity column.
3. The product matches rows to catalogue items, using saved matches; the coordinator corrects or completes matches and may save them.
4. The coordinator adds temporary items for unmatched rows, individually or in bulk.
5. The product plans the items in one group dedicated to imported equipment and shows an import report.

## Alternative Flows

- Rows failed: the coordinator downloads the failed rows with reasons, fixes them and re-imports.

## Failure Conditions

Accessories are not added; partial overwriting of already planned items is impossible.

## Postconditions

The project contains the imported equipment.
