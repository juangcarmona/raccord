---
id: BR-PRJ-DEFAULT-PERIODS
type: business-rule
title: "Every schedule starts with a planning and a usage period"
status: draft
applies-to:
  - "UC-PRJ-MANAGE-TIME-SCHEDULE"
  - "UC-CREATE-PROJECT"
uses-terms:
  - "TERM-PLANNING-PERIOD"
  - "TERM-USAGE-PERIOD"
provenance:
  source: ".private/research/help-center/articles/360013599680.md, .private/research/help-center/articles/360017700339.md, .private/research/help-center/articles/360013668139.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A new time schedule contains by default one period designated as planning period and one designated as usage period, and new equipment groups are linked to these by default.

## Rationale

These two periods are the minimum needed for availability and pricing.

## Examples

A blank project shows a planning period and a usage period ready to be timed.

## Exceptions

Projects created from templates take the template's periods.
