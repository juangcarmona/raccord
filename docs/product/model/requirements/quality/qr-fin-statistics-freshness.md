---
id: QR-FIN-STATISTICS-FRESHNESS
type: quality-requirement
title: "Statistics reflect current data"
status: draft
quality-attribute: "data currency"
applies-to:
  - "UC-FIN-ANALYZE-STATISTICS"
verification:
  - scenario-ref: "SB-FIN-STATISTICS-CHANGE-WITH-DATA"
uses-terms:
  - "TERM-STATISTICS-VIEW"
provenance:
  source: ".private/research/help-center/articles/360013767499.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

Statistics MUST reflect every saved change to projects, equipment and configuration the next time a view is opened.

## Measurement

Change a project price, reopen the view: 100% of tested changes are reflected; no cached snapshot is served.
