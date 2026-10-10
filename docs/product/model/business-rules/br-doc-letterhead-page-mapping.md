---
id: "BR-DOC-LETTERHEAD-PAGE-MAPPING"
type: business-rule
title: "Letterhead pages map to document pages"
status: draft
applies-to:
  - "UC-DOC-ADD-LETTERHEAD"
  - "UC-DOC-GENERATE-DOCUMENT"
uses-terms:
  - "TERM-LETTERHEAD"
provenance:
  source: ".private/research/help-center/articles/360014055700.md, .private/research/help-center/articles/360017648260.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Page n of a generated document is printed on page n of the letterhead; every document page beyond the letterhead's page count uses the letterhead's last page.

## Rationale

Allows a distinct first page (cover, full company details) and a lighter continuation page.

## Examples

See SB-DOC-LETTERHEAD-SHORTER-THAN-DOCUMENT and SB-DOC-LETTERHEAD-FIRST-PAGE-ONLY.

## Exceptions

None.
