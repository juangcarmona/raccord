---
id: BR-FIN-INVOICE-SCOPE
type: business-rule
title: "An invoice defaults to the whole project and can be reduced"
status: draft
applies-to:
  - "UC-FIN-GENERATE-PROJECT-INVOICE"
uses-terms:
  - "TERM-PARTIAL-INVOICE"
  - "TERM-PROJECT-INVOICE"
provenance:
  source: ".private/research/help-center/articles/360013965120.md, .private/research/help-center/articles/360017700379.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A new project invoice covers 100% of the project total; the user can enter a percentage of the total to make it partial.

## Rationale

Supports both one-off and staged billing.

## Examples

See SB-FIN-PARTIAL-INVOICE-PERCENTAGE.

## Exceptions

None.
