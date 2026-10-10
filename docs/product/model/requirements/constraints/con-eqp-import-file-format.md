---
id: CON-EQP-IMPORT-FILE-FORMAT
type: constraint
title: "Equipment import file format"
status: draft
applies-to:
  - "UC-EQP-IMPORT-EQUIPMENT"
  - "UC-EQP-MASS-UPDATE-EQUIPMENT"
  - "UC-EQP-IMPORT-KIT-CONTENT"
uses-terms:
  - "TERM-EQUIPMENT-IMPORT"
  - "TERM-SCAN-CODE"
provenance:
  source: ".private/research/help-center/articles/13725992768786.md, .private/research/help-center/articles/360014141299.md, .private/research/help-center/articles/24338169105298.md, .private/research/help-center/articles/24320695034642.md"
  confidence: high
  recovered-from: documentation
---

## Constraint

Equipment import files must be spreadsheet (.xlsx) or CSV files with:

- a single sheet;
- all values entered as plain text without formatting (no bold, italics or colours);
- at most 64 columns per import;
- no embedded images;
- scan codes of at most 25 characters (longer values are truncated).

## Rationale

Fixed boundary of the import function as documented; larger or richer files fail or behave unpredictably.

## Consequences

- Large catalogs must be split into several files.
- Images and kit structure are added in the product after import.
- Existing labels longer than 25 characters must be re-coded.
