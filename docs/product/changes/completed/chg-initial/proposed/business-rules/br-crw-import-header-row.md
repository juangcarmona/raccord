---
id: BR-CRW-IMPORT-HEADER-ROW
type: business-rule
title: "A declared header row is not imported"
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

When the file is declared to contain a header, its first row is used for column matching and is not imported as a record.

## Rationale

Headers describe columns, not data.

## Examples

A 51-row file with header yields at most 50 records.

## Exceptions

None.
