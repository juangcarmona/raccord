---
id: BR-PRJ-SCHEDULE-REQUIRED
type: business-rule
title: "A project cannot be saved without a time schedule"
status: active
applies-to:
  - "UC-CREATE-PROJECT"
  - "UC-PRJ-CREATE-FROM-TEMPLATE"
uses-terms:
  - "TERM-TIME-SCHEDULE"
provenance:
  source: ".private/research/help-center/articles/13724369155218.md, .private/research/help-center/articles/13793720770194.md, .private/research/help-center/articles/29017857097618.md, .private/research/help-center/articles/360013599680.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A new project of any category except the without-times sale category can only be saved once its time schedule contains its periods (at least a planning period and a usage period).

## Rationale

Availability, pricing and overviews all depend on the project's periods.

## Examples

A coordinator fills in name and client but no times: saving is refused until periods are entered.

## Exceptions

Without-times sale projects (see BR-PRJ-SALE-PERIOD-OPTIONAL).
