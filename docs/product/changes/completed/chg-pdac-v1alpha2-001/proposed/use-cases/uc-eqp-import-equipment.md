---
id: UC-EQP-IMPORT-EQUIPMENT
type: use-case
title: "Import equipment from a file"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-EQP-IMPORT-MATCH-UNIQUE"
  - "BR-EQP-IMPORT-QUANTITY-BULK-ONLY"
  - "BR-EQP-IMPORT-DUPLICATE-CODES"
  - "BR-EQP-IMPORT-NO-STRUCTURE"
  - "BR-EQP-AUTO-SCAN-CODE"
  - "BR-PRJ-IMPORT-FOLDER-PLACEMENT"
  - "BR-PRJ-IMPORT-TAG"
  - "BR-PRJ-IMPORT-RESULT-REPORT"
uses-terms:
  - "TERM-EQUIPMENT-IMPORT"
  - "TERM-IMPORT-MODE"
  - "TERM-IMPORT-MATCH-COLUMN"
  - "TERM-EQUIPMENT-FOLDER"
  - "TERM-TAG"
  - "TERM-CUSTOM-FIELD"
provenance:
  source: ".private/research/help-center/articles/360014141299.md, .private/research/help-center/articles/24338169105298.md, .private/research/help-center/articles/24320695034642.md, .private/research/help-center/articles/13725992768786.md, .private/research/help-center/articles/360013580119.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A whole catalog, or part of it, is loaded from a spreadsheet instead of typed in.

## Trigger

The company starts using the product or adds a large batch of equipment.

## Preconditions

- A spreadsheet or CSV file in the expected format.
- Ideally, the folder structure exists or folder paths are given in the file.

## Main Flow

1. The equipment manager starts an equipment import, optionally downloading the example file first.
2. The manager chooses the import mode.
3. The manager uploads the file and states whether the first row is a header.
4. The product proposes a mapping of file columns to equipment fields and custom fields; the manager corrects it.
5. The manager chooses a root folder under which items are placed following their folder paths.
6. For skip or update modes, the manager chooses match columns.
7. The product previews the first 10 rows; the manager starts the import.
8. The product creates or updates items, tags all with an import tag, and shows the result.

## Alternative Flows

- 2a. Update mode: the manager also chooses whether scan codes from the file replace existing codes or are added to them.
- 8a. Partial failure: the result lists failed rows with reasons and offers them as a downloadable file; the manager fixes and re-imports them.
- 8b. Badly wrong import: the manager archives all items with that import tag and imports again.

## Failure Conditions

- Scan code already present in the catalog: that row is rejected.
- Scan codes longer than 25 characters are cut, which may create duplicates.
- Very large files may stop partway; the manager deletes the import and splits the file.
- Multiple sheets, formatted cells or embedded images: the import fails or behaves unpredictably.

## Postconditions

- Imported items exist under the root folder with their tag; kit content and accessories still have to be added.
