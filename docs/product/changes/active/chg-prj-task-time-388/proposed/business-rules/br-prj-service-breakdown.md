---
id: BR-PRJ-SERVICE-BREAKDOWN
type: business-rule
title: "Service breakdown calculation"
status: active
applies-to:
  - "UC-PRJ-MANAGE-SERVICES"
  - "UC-PRJ-REPORT-TASK-TIME"
uses-terms:
  - "TERM-PRODUCTION-SERVICE"
provenance:
  source: ".private/research/help-center/articles/38423504883858.md, .private/research/help-center/articles/38423389508242.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A service's estimated cost is its estimated time times its cost per hour; planned cost is the sum of linked tasks' planned time times the cost per hour; actual cost is tracked hours on linked tasks times the cost per hour; revenue is the service price; profit is revenue less discount and costs.

## Rationale

Shows whether the price covered the work.

## Examples

See SB-PRJ-SERVICE-BREAKDOWN-FIGURES.

## Exceptions

A service without tasks has no planned or actual hours.
