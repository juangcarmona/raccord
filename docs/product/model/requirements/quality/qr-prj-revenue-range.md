---
id: QR-PRJ-REVENUE-RANGE
type: quality-requirement
title: "Revenue widget range"
status: draft
quality-attribute: "capacity"
applies-to:
  - "UC-PRJ-CONFIGURE-DASHBOARD"
verification:
  - scenario: "A range of 15 months back and forward renders 31 monthly bars"
uses-terms:
  - "TERM-DASHBOARD-WIDGET"
provenance:
  source: ".private/research/help-center/articles/360014243139.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

Revenue widgets MUST support relative ranges from up to 15 months in the past to up to 15 months in the future.

## Measurement

Configure 15 months back and 15 forward: accepted and rendered; 16 months: not selectable.
