---
id: BR-FIN-EXPORT-ONLY-FINAL
type: business-rule
title: "Only finalized invoices are exported"
status: draft
applies-to:
  - "UC-FIN-RUN-ACCOUNTING-EXPORT"
uses-terms:
  - "TERM-ACCOUNTING-EXPORT"
provenance:
  source: ".private/research/help-center/articles/6819935599378.md, .private/research/help-center/articles/33228429263378.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Only finalized invoices are included in the accounting export; drafts and previews never are.

## Rationale

Books must not contain provisional data.

## Examples

See SB-FIN-PREVIEW-NOT-EXPORTED.

## Exceptions

None.
