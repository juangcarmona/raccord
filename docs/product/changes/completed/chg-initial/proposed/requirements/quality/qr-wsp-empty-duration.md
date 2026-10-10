---
id: QR-WSP-EMPTY-DURATION
type: quality-requirement
title: "Emptying a workspace completes within five minutes"
status: draft
quality-attribute: "performance"
applies-to:
  - "UC-WSP-EMPTY-SAMPLE-DATA"
verification:
  - scenario: "Emptying a workspace filled with sample data completes in at most five minutes"
uses-terms:
  - "TERM-SAMPLE-DATA"
provenance:
  source: ".private/research/help-center/articles/360013982380.md"
  confidence: medium
  recovered-from: documentation
---

## Requirement

Emptying an evaluation workspace MUST complete within about five minutes.

## Measurement

Time the operation from confirmation to completion on a sample-data workspace; threshold five minutes.
