---
id: BR-WHS-CROSS-DOCK-ELIGIBILITY
type: business-rule
title: "Cross-docking candidates"
status: draft
applies-to:
  - "UC-WHS-REVIEW-CROSS-DOCKING"
uses-terms:
  - "TERM-CROSS-DOCKING-OVERLAP"
provenance:
  source: ".private/research/help-center/articles/11884262854930.md, .private/research/help-center/articles/360013158280.md"
  confidence: high
  recovered-from: documentation
---

## Rule

An article appears in the cross-docking overview when one confirmed-or-later project returning it ends within the chosen window (12, 24 or 48 hours), another confirmed-or-later project needing it starts within the same window, and the first ends before the second starts; virtual combinations appear only through their content.

## Rationale

Only real, non-overlapping hand-overs can skip the shelf.

## Examples

See SB-WHS-CROSS-DOCK-CHAIRS.

## Exceptions

None.
