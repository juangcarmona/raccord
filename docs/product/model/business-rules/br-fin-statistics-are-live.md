---
id: BR-FIN-STATISTICS-ARE-LIVE
type: business-rule
title: "Statistics are recalculated from current data"
status: draft
applies-to:
  - "UC-FIN-ANALYZE-STATISTICS"
uses-terms:
  - "TERM-STATISTICS-VIEW"
provenance:
  source: ".private/research/help-center/articles/360013767499.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Statistics are computed at the moment they are viewed from the current projects, equipment and configuration; no historical snapshot is kept, so changes also alter figures for past periods.

## Rationale

One source of truth; preserving a result requires exporting it.

## Examples

See SB-FIN-STATISTICS-CHANGE-WITH-DATA.

## Exceptions

None.
