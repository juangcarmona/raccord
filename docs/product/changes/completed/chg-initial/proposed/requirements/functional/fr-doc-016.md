---
id: "FR-DOC-016"
type: functional-requirement
title: "Produce equipment labels"
status: draft
derived-from:
  - "UC-DOC-DESIGN-EQUIPMENT-LABEL"
  - "BR-DOC-LETTERHEAD-PAGE-SIZE"
  - "BR-DOC-LABEL-SAVE-TO-ITEM"
verification:
  - scenario-ref: "SB-DOC-LABEL-SAVED-TO-ITEM"
  - scenario-ref: "SB-DOC-LABEL-OVERFLOW"
uses-terms:
  - "TERM-EQUIPMENT-LABEL"
provenance:
  source: ".private/research/help-center/articles/20257253618322.md, .private/research/help-center/articles/360017700539.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST produce label-sized equipment slips, one per selected article, downloadable as PDF and savable to the article.

## Rationale

Tagging equipment is core to tracking.
