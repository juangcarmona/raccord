---
id: BR-FIN-ACTUAL-COST-SOURCES
type: business-rule
title: "Actual costs come from registered hours"
status: draft
applies-to:
  - "UC-FIN-REVIEW-PROJECT-FINANCIALS"
uses-terms:
  - "TERM-ACTUAL-COST"
provenance:
  source: ".private/research/help-center/articles/360013264720.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Actual crew cost is computed only from worked hours registered against a function of the project.

## Rationale

Hours not linked to a function cannot be attributed to a project.

## Examples

Eight hours registered against a function, costed at 20.00 per hour, give an actual crew cost of 160.00.

## Exceptions

Registered hours without a function do not appear in any project's actual cost.
