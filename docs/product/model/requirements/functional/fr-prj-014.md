---
id: FR-PRJ-014
type: functional-requirement
title: "Support sale projects without mandatory times"
status: draft
derived-from:
  - "UC-PRJ-CREATE-SALE-PROJECT"
  - "BR-PRJ-SALE-PERIOD-OPTIONAL"
  - "BR-PRJ-SALE-STOCK-ON-BOOKING"
verification:
  - scenario-ref: "SB-PRJ-SALE-WITHOUT-PERIOD-NO-STOCK-CHANGE"
  - scenario: "A sale project with one period reduces stock when items are booked out"
uses-terms:
  - "TERM-SALE-PROJECT"
provenance:
  source: ".private/research/help-center/articles/28922703497490.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST allow sale projects to be saved without periods and MUST adjust sale stock on booking when the project has a period.

## Rationale

Sales leave permanently.
