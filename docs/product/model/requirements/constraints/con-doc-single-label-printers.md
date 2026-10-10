---
id: "CON-DOC-SINGLE-LABEL-PRINTERS"
type: constraint
title: "Labels require single-label printers"
status: draft
applies-to:
  - "UC-DOC-DESIGN-EQUIPMENT-LABEL"
  - "UC-WHS-PRINT-CODE-LABELS"
uses-terms:
  - "TERM-EQUIPMENT-LABEL"
provenance:
  source: ".private/research/help-center/articles/20257253618322.md, .private/research/help-center/articles/360017700539.md"
  confidence: high
  recovered-from: documentation
---

## Constraint

Equipment labels are produced one label per page and print correctly only on printers that print one label at a time; sheet label printers are not supported.

## Rationale

Each label is a page sized by the label letterhead.

## Consequences

Companies must use a roll-fed label printer or print the PDF with the printer's own software.
