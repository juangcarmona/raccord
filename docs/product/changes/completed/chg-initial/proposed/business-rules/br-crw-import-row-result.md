---
id: BR-CRW-IMPORT-ROW-RESULT
type: business-rule
title: "Import results report each failed row with its reason"
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

## Rule

After an import the product reports which rows were imported and which failed, each failure with its reason, and offers the failed rows with reasons as a downloadable file; successful rows are kept even when others fail.

## Rationale

Users correct only what failed.

## Examples

Rows with formatting markup fail with a formatting reason while other rows are imported.

## Exceptions

None.
