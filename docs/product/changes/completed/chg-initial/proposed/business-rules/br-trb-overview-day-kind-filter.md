---
id: "BR-TRB-OVERVIEW-DAY-KIND-FILTER"
type: business-rule
title: "Day-kind selection can hide projects outside list mode"
status: draft
applies-to:
  - "UC-PRJ-BROWSE-PROJECTS"
uses-terms:
  - "TERM-PROJECT-DAY-KIND"
  - "TERM-PROJECT-OVERVIEW"
provenance:
  source: ".private/research/help-center/articles/360014514680.md"
  confidence: high
  recovered-from: documentation
---

## Rule

In per-day, calendar and timeline modes, a project is only shown on days of the selected day kinds (equipment out, equipment in, crew needed, transport needed, or all days); a project with none of the selected day kinds in the period is not shown.

## Rationale

Lets users focus on, for example, outgoing equipment days.

## Examples

See SB-TRB-DAY-KIND-HIDES-PROJECT.

## Exceptions

List mode ignores day kinds.
