---
id: "BR-TRB-OVERVIEW-PERIOD-OVERLAP"
type: business-rule
title: "The project overview shows projects whose planning period overlaps the selected period"
status: draft
applies-to:
  - "UC-PRJ-BROWSE-PROJECTS"
uses-terms:
  - "TERM-PROJECT-OVERVIEW"
  - "TERM-PLANNING-PERIOD"
  - "TERM-SALE-PROJECT"
provenance:
  source: ".private/research/help-center/articles/360014514680.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A project appears in the project overview for a selected period only when (part of) its planning period falls within that period; projects without any time period appear only when the without-date selection is chosen.

## Rationale

The overview is a time-based working list.

## Examples

See SB-TRB-PROJECT-OUTSIDE-PERIOD, SB-TRB-PROJECT-PARTIAL-OVERLAP and SB-TRB-UNDATED-SALE-PROJECT.

## Exceptions

None.
