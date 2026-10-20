---
id: BR-PRJ-TAG-FILTER-MODES
type: business-rule
title: "Tag filters match any or all"
status: active
applies-to:
  - "UC-PRJ-TAG-RECORDS"
  - "UC-PRJ-BROWSE-PROJECTS"
uses-terms:
  - "TERM-TAG"
provenance:
  source: ".private/research/help-center/articles/360013931999.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Filtering by several tags returns records having at least one of them, or records having all of them, as chosen.

## Rationale

Both union and intersection are needed.

## Examples

See SB-PRJ-TAG-FILTER-ALL.

## Exceptions

None.
