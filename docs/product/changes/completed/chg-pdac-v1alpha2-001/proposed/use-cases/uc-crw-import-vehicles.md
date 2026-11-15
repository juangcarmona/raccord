---
id: UC-CRW-IMPORT-VEHICLES
type: use-case
title: "Import vehicles from a file"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-IMPORT-MATCH-COLUMN"
  - "BR-CRW-IMPORT-FOLDER-PLACEMENT"
  - "BR-CRW-IMPORT-HEADER-ROW"
  - "BR-CRW-IMPORT-ROW-RESULT"
  - "BR-CRW-IMPORT-TAG"
uses-terms:
  - "TERM-RESOURCE-IMPORT"
  - "TERM-VEHICLE"
provenance:
  source: ".private/research/help-center/articles/360014307939.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Many vehicles are created or updated at once.

## Trigger

Fleet set-up.

## Preconditions

A single-sheet spreadsheet or delimited file.

## Main Flow

1. The planner chooses the import mode, uploads the file and declares a header row.
2. The planner checks the column mapping, for example registration number versus licence plate.
3. The planner selects the root folder and, for skip mode, match columns.
4. The product previews the first 10 rows and imports.

## Alternative Flows

Update mode updates matching vehicles.

## Failure Conditions

Failed rows are listed with reasons and downloadable.

## Postconditions

Vehicles exist in their folders.
