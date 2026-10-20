---
id: BR-PRJ-USAGE-WITHIN-PLANNING
type: business-rule
title: "The usage period is shorter than the planning period"
status: active
applies-to:
  - "UC-PRJ-MANAGE-TIME-SCHEDULE"
  - "BC-PROJECTS"
uses-terms:
  - "TERM-PLANNING-PERIOD"
  - "TERM-USAGE-PERIOD"
provenance:
  source: ".private/research/help-center/articles/360017700339.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The usage period of a project must always be shorter than its planning period.

## Rationale

The customer cannot use equipment longer than it is away from the warehouse.

## Examples

Planning period Friday 08:00 to Monday 18:00, usage period Saturday 10:00 to Sunday 23:00: valid.

## Exceptions

The product refuses to save a usage period outside the planning period and explains which boundary is invalid.
