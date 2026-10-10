---
id: BR-FIN-QUOTE-SNAPSHOTS-PLAN
type: business-rule
title: "A quote copies the plan at generation"
status: draft
applies-to:
  - "UC-FIN-GENERATE-QUOTE"
  - "UC-FIN-REGENERATE-QUOTE"
uses-terms:
  - "TERM-QUOTE"
provenance:
  source: ".private/research/help-center/articles/360012594659.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Generating a quote or contract copies the prices of the project plan into the document; later plan changes appear in the document only after it is regenerated.

## Rationale

The customer must see a stable offer.

## Examples

See SB-FIN-PLAN-CHANGE-NOT-IN-QUOTE.

## Exceptions

None.
