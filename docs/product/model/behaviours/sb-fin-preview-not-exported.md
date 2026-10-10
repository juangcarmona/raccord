---
id: SB-FIN-PREVIEW-NOT-EXPORTED
type: structured-behaviour
title: "Draft invoice is skipped by the export"
status: draft
illustrates:
  - "BR-FIN-DRAFT-PREVIEW-MARK"
  - "BR-FIN-EXPORT-ONLY-FINAL"
given:
  - "An invoice is generated but not finalized"
when: "The nightly accounting export runs"
then:
  - "The draft is not exported"
  - "Its document still carries the preview mark"
uses-terms:
  - "TERM-INVOICE-PREVIEW"
provenance:
  source: ".private/research/help-center/articles/6819935599378.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Only final invoices reach the books.

## Boundaries

None.
