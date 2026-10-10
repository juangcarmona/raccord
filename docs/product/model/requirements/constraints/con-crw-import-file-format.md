---
id: CON-CRW-IMPORT-FILE-FORMAT
type: constraint
title: "Crew and vehicle import file format"
status: draft
applies-to:
  - "UC-CRW-IMPORT-CREW-MEMBERS"
  - "UC-CRW-IMPORT-VEHICLES"
uses-terms:
  - "TERM-RESOURCE-IMPORT"
provenance:
  source: ".private/research/help-center/articles/360014366299.md, .private/research/help-center/articles/360014307939.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Constraint

Import files are single-sheet spreadsheets or delimited text files with at most 64 columns, plain-text values without formatting markup or images, and optional header row.

## Rationale

Fixed by the product's import capability.

## Consequences

Files with several sheets, formatting or more columns are rejected or partially fail; large files should be split.
