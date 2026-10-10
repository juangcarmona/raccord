---
id: QR-WHS-001
type: quality-requirement
title: "Tracking log history depth and list size"
status: draft
quality-attribute: "auditability"
applies-to:
  - "UC-WHS-TRACE-TRACKING-LOG"
  - "BC-WAREHOUSE"
verification:
  - scenario: "The log shows actions from at least the preceding 6 months and returns up to 5,000 rows per list"
uses-terms:
  - "TERM-WAREHOUSE-TRACKING-LOG"
provenance:
  source: ".private/research/help-center/articles/20860250675474.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The tracking log MUST retain warehouse actions from at least 6 months back and keep accumulating, and MUST display up to 5,000 rows per list, showing 10 filter values per section by default with search for others.

## Measurement

Query history after 6 months of use; count rows returned for a broad filter.
