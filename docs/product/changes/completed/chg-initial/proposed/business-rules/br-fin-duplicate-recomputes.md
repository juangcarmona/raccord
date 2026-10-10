---
id: BR-FIN-DUPLICATE-RECOMPUTES
type: business-rule
title: "A duplicated invoice copies settings but recomputes content"
status: draft
applies-to:
  - "UC-FIN-DUPLICATE-INVOICE"
uses-terms:
  - "TERM-PROJECT-INVOICE"
provenance:
  source: ".private/research/help-center/articles/360013965120.md, .private/research/help-center/articles/6819935599378.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Duplicating an invoice copies its configuration (document text, advanced settings) and computes its content, such as the project price, from the project data at the time of duplication, not from the original invoice.

## Rationale

Saves configuration effort without billing stale prices.

## Examples

See SB-FIN-DUPLICATE-USES-CURRENT-PRICE.

## Exceptions

None.
