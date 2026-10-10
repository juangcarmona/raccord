---
id: BR-PRJ-REVENUE-WIDGET-RANGE
type: business-rule
title: "Revenue widgets cover up to 15 months each way"
status: draft
applies-to:
  - "UC-PRJ-CONFIGURE-DASHBOARD"
uses-terms:
  - "TERM-DASHBOARD-WIDGET"
provenance:
  source: ".private/research/help-center/articles/360014243139.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The revenue widget shows a relative range of at most 15 months in the past to 15 months in the future, per status (confirmed, pending, cancelled) selectable; the pie variant compares each status with the previous full month in percent.

## Rationale

Bounded, comparable windows.

## Examples

A range of 20 months back is not allowed.

## Exceptions

None.
