---
id: BR-PRJ-TEMPLATE-START-ANCHOR
type: business-rule
title: "Copied schedules are anchored to the chosen start date"
status: draft
applies-to:
  - "UC-PRJ-CREATE-FROM-TEMPLATE"
  - "UC-PRJ-DUPLICATE-PROJECT"
uses-terms:
  - "TERM-PROJECT-TEMPLATE"
provenance:
  source: ".private/research/help-center/articles/360013590720.md, .private/research/help-center/articles/360013367659.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When a project is created from a template or by duplication, all periods are copied and moved so that the schedule starts on the chosen start date, keeping relative offsets; times of day may then be adjusted.

## Rationale

Templates must be reusable on any date.

## Examples

See SB-PRJ-TEMPLATE-ANCHORED-SCHEDULE.

## Exceptions

None.
