---
id: "BR-DOC-HIDE-ZERO-PRICED"
type: business-rule
title: "Zero-priced lines can be hidden on financial documents"
status: draft
applies-to:
  - "UC-DOC-HIDE-ITEMS-ON-FINANCIAL-DOCUMENTS"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360017647860.md, .private/research/help-center/articles/360017647940.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When financial documents hide lines with a price of zero, every line whose resulting price is zero is omitted, including lines discounted by 100 percent.

## Rationale

Accessories priced at zero clutter quotations.

## Examples

See SB-DOC-FULLY-DISCOUNTED-LINE-HIDDEN.

## Exceptions

Packing slips are not affected.
