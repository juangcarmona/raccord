---
id: QR-CRW-IMPORT-PREVIEW
type: quality-requirement
title: "Import preview shows the first 10 rows"
status: draft
quality-attribute: "usability"
applies-to:
  - "UC-CRW-IMPORT-CREW-MEMBERS"
  - "UC-CRW-IMPORT-VEHICLES"
verification:
  - scenario: "Importing a 200-row crew file shows exactly the first 10 rows in the preview"
uses-terms:
  - "TERM-RESOURCE-IMPORT"
provenance:
  source: ".private/research/help-center/articles/360014366299.md, .private/research/help-center/articles/360014307939.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

Before confirming an import the product MUST preview the first 10 mapped rows.

## Measurement

Count of rows shown in preview equals min(10, rows in file).
